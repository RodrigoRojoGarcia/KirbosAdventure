using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float timeOfLife = 0;

    [SerializeField] private float maxTimeOfLiving;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.instance.kirbo.gameObject)
        {
            GameManager.instance.kirbo.addHealth(-1);
            Destroy(gameObject);
        }
            
    }

    private void Update()
    {
        timeOfLife += Time.deltaTime;

        if(timeOfLife > maxTimeOfLiving)
        {
            Destroy(gameObject);
        }
    }
}
