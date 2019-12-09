using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GameManager.instance.kirbo.gameObject)
        GameManager.instance.kirbo.addHealth(-1);

        if (collision.gameObject.GetComponent<EnemyController>() == null && collision.gameObject.GetComponent<Thunder>())
        {
            Destroy(gameObject);
        }
        
    }

}
