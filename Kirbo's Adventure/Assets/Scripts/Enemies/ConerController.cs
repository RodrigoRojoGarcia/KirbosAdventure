using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConerController : EnemyController
{
    

    [SerializeField] private float speed;


    private bool facingRight = false;



    [SerializeField] private GameObject inFrontCollider;
    [SerializeField] private GameObject downInFrontCollider;
    [SerializeField] private float inFrontCollidersRadius;

    private Collider2D[] collidersInFront = new Collider2D[8];
    private Collider2D[] collidersDownInFront = new Collider2D[8];



    [SerializeField] private LayerMask whatIsGround;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(downInFrontCollider.transform.position, inFrontCollidersRadius);
        Gizmos.DrawWireSphere(inFrontCollider.transform.position, inFrontCollidersRadius);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("hideInShell", kirboInSight);
    }

    private void FixedUpdate()
    {

        if (!kirboInSight)
        {
            tag = "absorbable";

            Vector2 vel = rigidBody.velocity;

            if (facingRight)
            {
                vel = speed * Vector2.right;
            }
            else
            {
                vel = speed * Vector2.left;
            }
            


            rigidBody.velocity = vel;


            Physics2D.OverlapCircleNonAlloc(inFrontCollider.transform.position, inFrontCollidersRadius, collidersInFront, whatIsGround);
            bool turn = false;
            foreach(Collider2D col in collidersInFront)
            {

                if (col != null)
                {
                    if (col.gameObject == this) continue;
                    turn = true;
                }
            }
            if (turn) {
                Flip();
                collidersInFront = new Collider2D[8];
            } else
            {
                Physics2D.OverlapCircleNonAlloc(downInFrontCollider.transform.position, inFrontCollidersRadius, collidersDownInFront, whatIsGround);
                turn = true;
                foreach (Collider2D col in collidersDownInFront)
                {
                    turn = turn && col == null;
                }
                if (turn)
                {
                    Flip();
                }

                collidersDownInFront = new Collider2D[8];

            }

        }
        else
        {
            tag = "Untagged";
        }

        
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public override void getHit()
    {
        Destroy(gameObject);

    }



}
