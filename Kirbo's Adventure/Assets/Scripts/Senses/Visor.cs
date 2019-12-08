using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visor : MonoBehaviour
{

    [SerializeField] private string tagWall;
    [SerializeField] private string tagPlayer;
    [SerializeField] private GameObject thisAgent;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag.Equals(tagPlayer))
        {
            if (thisAgent.GetComponent<EnemyController>() != null)
            {
                Debug.Log("uwu");
                thisAgent.GetComponent<EnemyController>().StopSeePlayer();
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (!tag.Equals(tagPlayer))
        {
            return;
        }

        GameObject player = collision.gameObject;
        Vector2 rayDirection = player.transform.position - thisAgent.transform.position;
        float length = rayDirection.magnitude;
        rayDirection.Normalize();

        RaycastHit2D[] raycastHits;
        raycastHits = Physics2D.RaycastAll(thisAgent.transform.position, rayDirection);
        bool playerseen = false;
        bool wallHit = false;
        int i = 0;


        while ((!playerseen && !wallHit) && i < raycastHits.Length)
        {

            GameObject hitObj;
            hitObj = raycastHits[i].collider.gameObject;
            tag = hitObj.tag;
            if (tag.Equals(tagWall))
            {
                wallHit = true;
            }
            if (tag.Equals(tagPlayer))
            {
                playerseen = true;
                if (thisAgent.GetComponent<EnemyController>() != null)
                {

                    thisAgent.GetComponent<EnemyController>().SeePlayer();
                }

            }

            i++;
        }
        
        if (!playerseen)
        {
            if(thisAgent.GetComponent<EnemyController>() != null)
            {
                
                thisAgent.GetComponent<EnemyController>().StopSeePlayer();
            }
        }

        
        
    }

}
