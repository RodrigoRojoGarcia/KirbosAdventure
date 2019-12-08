using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbingZone : MonoBehaviour
{
    private bool absorbing = false;

    private int whatKirbosFacing = 1;

    [SerializeField] private float absorbingSpeed;

    [SerializeField] private float mouthCollisionRadius;


    private Collider2D[] colliders = new Collider2D[8];

    public void setAbsorbing(bool abs) { this.absorbing = abs; }
    public void setWhatKirboIsFacing(int i) { this.whatKirbosFacing = i; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        
        Gizmos.DrawWireSphere(transform.position, mouthCollisionRadius);
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("absorbable"))
        {

            if (absorbing)
            {
                Vector2 vel = collision.gameObject.GetComponent<Rigidbody2D>().velocity;

                vel.x += whatKirbosFacing * absorbingSpeed;

                collision.gameObject.GetComponent<Rigidbody2D>().velocity = vel;
            }

        }
    }



    private void FixedUpdate()
    {
        if (absorbing)
        {
            Physics2D.OverlapCircleNonAlloc(transform.position, mouthCollisionRadius, colliders);
            foreach(Collider2D col in colliders)
            {

                if (col == null) continue;
                if (!col.gameObject.tag.Equals("absorbable")) continue;


                EnemyController enemyController = col.GetComponent<EnemyController>();
                if(enemyController != null)
                {
                    
                    GameManager.instance.kirbo.setObjectInMouth(enemyController.getThis());

                    GameObject.Destroy(col.gameObject);
                }
                

                return;
            }
        }
    }
}
