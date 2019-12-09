using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirboProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyController>() != null)
        {
            collision.gameObject.GetComponent<EnemyController>().getHit();
        }
    }
}
