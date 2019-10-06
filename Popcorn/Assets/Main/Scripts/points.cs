using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class points : MonoBehaviour
{

    public int pointt;

    // Use this for initialization
    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D hit)
    {

        if (hit.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GameStatus.score += pointt;
        }
    }

}
