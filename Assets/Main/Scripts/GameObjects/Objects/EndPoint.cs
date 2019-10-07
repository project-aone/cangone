using UnityEngine;
using Popcorn.Bases;
using UnityEngine.UI;
using Popcorn.Managers;
using System.Collections;
using Popcorn.ObjectsServices;
using Popcorn.GameObjects.Objects;
using Times = Popcorn.Metadatas.Times;
using Errors = Popcorn.Metadatas.Strings.Errors;
using ObjectsTags = Popcorn.Metadatas.Tags.Objects;
using PersonsTags = Popcorn.Metadatas.Tags.Persons;
using ElementiesTags = Popcorn.Metadatas.Tags.Elementies;
using UIElementiesTags = Popcorn.Metadatas.Tags.UIElementies;
using System;

namespace Popcorn.GameObjects.Objects
{

    public class EndPoint : MonoBehaviour
    {

        [HideInInspector] public bool WasReachedTheEnd { get; private set; }
        
        void Awake()
        {
            WasReachedTheEnd = false;
            
        }

        void OnTriggerEnter2D(Collider2D otherCollider2D)
        {
            if (otherCollider2D.CompareTag(PersonsTags.Player.ToString()))
            {
                WasReachedTheEnd = true;
            }
        }

    }

}