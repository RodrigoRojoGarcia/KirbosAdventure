using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirboProjectile : MonoBehaviour
{

    [SerializeField] private float lifeTime;
    private float life;


    private void Update()
    {
        if(life > lifeTime)
        {
            Destroy(gameObject);
        }
        else
        {
            life += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyController>() != null)
        {
            collision.gameObject.GetComponent<EnemyController>().getHit();
            Destroy(gameObject);
        }
        
            
    }
}
