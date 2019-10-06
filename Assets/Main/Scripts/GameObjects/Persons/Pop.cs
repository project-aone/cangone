using UnityEngine;
using UnityEngine.SceneManagement;
using Popcorn.Bases;
using Popcorn.Managers;
using Popcorn.Metadatas;
using System.Collections;
using Popcorn.ObjectsServices;
using Popcorn.ObjectsModifiers;
using Popcorn.GameObjects.Elementies;
using MathExt = Popcorn.Extensions.MathExt;
using HelpersTags = Popcorn.Metadatas.Tags.Helpers;
using PersonsTags = Popcorn.Metadatas.Tags.Persons;
using ObjectsTags = Popcorn.Metadatas.Tags.Objects;
using GameStates = Popcorn.GameObjects.Elementies.GameBehavior.GameStates;
using System;
using UnityEngine.UI;

namespace Popcorn.GameObjects.Persons
{

    public class Pop : PlayerBase
    {
        [SerializeField] float timeToRestatIdle = 4.5f;
        [SerializeField] float velocity = 4f;
        [SerializeField] float jumpForce = 900f;
        [SerializeField] float hitForce = 300f;
        public GameObject startpoint;
        public GameObject playerpoint;

        float timeInStandBy = 0f;
        bool isJumping = false;
        float lastDir = Transforms.Direction.Right;
        private bool dirRight = true;
        static int points;
        bool jumpair = false;
        public Button pauseButton;
        private int i;
        private bool a;
        Jump jump = new Jump();
        Move move = new Move();

        private void Start()
        {
            pauseButton.enabled = true;
            a = true;
        }
        void FixedUpdate()
        {
            timeInStandBy += Time.deltaTime;

            if (timeInStandBy >= timeToRestatIdle)
            {
                animator.SetTrigger(AnimationParameters.IdleTrigger.ToString());
                timeInStandBy = 0;
            }
            
        }

        void Update()
        {
            
            if (CheckIfDontCanMove())
            {
                timeInStandBy = 0;
                return;
            }
            CleanVelocityX();
            
//            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) &&
//                !leftColliderHelper.IsColliding)
//            {
//                ExecuteMove(Transforms.Direction.Left);
//            }
            if (dirRight==true &&
                !rightColliderHelper.IsColliding)
            {
                ExecuteMove(Transforms.Direction.Right);
            }
            
            
            if (i==1 && !isJumping && a == true  && !(Input.GetTouch(0).position.x > Screen.width *7.5 /8 && Input.GetTouch(0).position.y > Screen.height * 3 / 4))
            {   
                ExecuteJump(jumpForce);
                jumpair = true;
                i = 0;
            }
           
            animator.SetFloat(AnimationParameters.Velocity.ToString(), GetAbsRunVelocity());
            isJumping = !bottomColliderHelper.IsColliding;
            animator.SetBool(AnimationParameters.IsJump.ToString(), isJumping);
            CheckAliveConditions();
            
            if (i == 2 && jumpair && !(Input.GetTouch(0).position.x > Screen.width * 7.5 / 8 && Input.GetTouch(0).position.y > Screen.height * 3 / 4))
            {
                ExecuteJump(jumpForce);
                jumpair = false;
            }
            paused();
        }

        //Rayhan & win, 22/09/2019 for fixing bug in pause
        public void paused()
        {
            if (pauseButton.enabled == false)
            {
                i = 0;
                a = false;
            }
            else
            {
                if (a == true)
                    i = Input.GetTouch(0).tapCount;
                else
                    a = true;
            }
        }

        void ExecuteMove(float dir)
        {
            move.Execute(rb, velocity * dir);
            lastDir = dir;
            timeInStandBy = 0;
            spriteRenderer.flipX = dir < 0;

        }

        void ExecuteJump(float force)
        {
            jump.Execute(rb, force);
            AudioManager.Instance.PlaySoundOnce(caller: this, sound: jumpAudioSource);
            timeInStandBy = 0;
        }

        void CleanVelocityX()
        {
            Vector2 vel = rb.velocity;
            vel.x = 0;
            rb.velocity = vel;
        }

        bool CheckIfDontCanMove()
        {
            return !IsAlive ||
                GameBehavior.GameState == GameStates.Paused ||
                animator.GetBool(AnimationParameters.Hit.ToString());
        }

        void CheckAliveConditions()
        {
            if (this.transform.position.y <= bottomLimit) { Kill(jumpForce * 2); }

            if (GameBehavior.GameState == GameStates.TimeOut) { Kill(jumpForce); }
        }

        void OnCollisionEnter2D(Collision2D otherCollider2D)
        {
            if (otherCollider2D.gameObject.CompareTag(PersonsTags.Enemy.ToString()))
            {
                ContactPoint2D contactPoint2D = otherCollider2D.contacts[0];
                //Win, 23/09/2019, condition changed
                if (!contactPoint2D.collider.CompareTag(HelpersTags.WeakPoint.ToString()) && GameStatus.lives>1)
                        { lifeagain(); }
                else if (!contactPoint2D.collider.CompareTag(HelpersTags.WeakPoint.ToString()) && GameStatus.lives==1)
                        { Kill(jumpForce); }
                else { ExecuteJump(jumpForce - 50);}

            }
            else if (otherCollider2D.gameObject.CompareTag(ObjectsTags.Hit.ToString()))
            {
                Hit();
            }
            else if (otherCollider2D.gameObject.tag == "clover")
            {
                Pop.points += 1;
                Destroy(gameObject);
            }
                
        }

        void OnCollisionStay2D(Collision2D otherCollider2D)
        {
            if (otherCollider2D.gameObject.CompareTag(ObjectsTags.Hit.ToString())) { Hit(); }
        }

        void OnTriggerEnter2D(Collider2D otherCollider2D)
        {
            if (otherCollider2D.gameObject.CompareTag(ObjectsTags.EndPoint.ToString())) { Win(); }
        }

        void Win()
        {
            AudioManager.Instance.PlaySoundOnce(caller: this, sound: winAudioSource);

            if (isJumping) { animator.SetTrigger(AnimationParameters.WinTrigger.ToString()); }
            else { StartCoroutine(WinAnimation()); }
        }

        IEnumerator WinAnimation()
        {
            yield return new WaitForSeconds(Times.Waits.MinimunPlus);
            animator.SetTrigger(AnimationParameters.WinTrigger.ToString());
        }

        public float GetAbsRunVelocity() { return Mathf.Abs(rb.velocity.x); }

        void Kill(float forceToUp)
        {
            if (IsAlive)
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = Transforms.Gravity.Without;
                IsAlive = false;
                animator.SetBool(AnimationParameters.IsAlive.ToString(), IsAlive);
                StartCoroutine(KillAnimation(forceToUp));
                GameStatus.lives--;
            }
        }

        private void WaitForSeconds(int v)
        {
            throw new NotImplementedException();
        }

        IEnumerator KillAnimation(float forceToUp)
        {
            yield return new WaitForSeconds(Times.Waits.Minimun);
            AudioManager.Instance.PlaySoundOnce(caller: this, sound: deathAudioSource);
            rb.gravityScale = Transforms.Gravity.Hard;
            jump.Execute(rb, forceToUp);
            (Getter.Component(this, gameObject, typeof(Collider2D)) as Collider2D).isTrigger = true;

            rb.transform.localScale = new Vector3(Transforms.Scale.NormalPlus
            , Transforms.Scale.NormalPlus
            , Transforms.Scale.NormalPlus);

            spriteRenderer.sortingOrder = (int)Layers.OrdersInDefaultLayer.Max;
            //21.09.19 Raihan untuk kembali ke awal jika player mati
            yield return new WaitForSeconds(3);
            GameStatus.score = 0;
        }

        //Win, 23/09/209, change player position back to start
        void lifeagain()
        {
            playerpoint.transform.position = new Vector2(startpoint.transform.position.x, startpoint.transform.position.y);
            base.Awake();
            GameStatus.lives--;
        }

        void Hit()
        {
            if (!animator.GetBool(AnimationParameters.Hit.ToString()))
            {
                Vector2 vectorHit = new Vector2();
                float timeInWait = Times.Waits.Minimun;

                animator.SetBool(AnimationParameters.Hit.ToString(), true);
                vectorHit.x = MathExt.GetInvertValue(lastDir) * hitForce;

                if (isJumping)
                {
                    vectorHit.x = MathExt.GetPercent(value: vectorHit.x, percent: 70);
                    vectorHit.y = MathExt.GetPercent(value: hitForce, percent: 50);
                    timeInWait = Times.Waits.MinimunPlus;
                }
                rb.velocity = Vector2.zero;
                rb.AddForce(vectorHit);
                StartCoroutine(EndHitAnimation(timeInWait));
   
            }
        }

        IEnumerator EndHitAnimation(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            animator.SetBool(AnimationParameters.Hit.ToString(), false);
            
        }

    }
    
}