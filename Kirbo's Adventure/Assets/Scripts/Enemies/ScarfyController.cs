using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarfyController : EnemyController
{

    [SerializeField] private float groundDetectionRadius;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Vector2 jumpForce;

    [SerializeField] private float explosionRadius;

    

    private bool grounded;


    private Collider2D[] collidersInExplosionArea = new Collider2D[8];

    private bool exploding;

    private bool facingRight = false;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(groundCheck.transform.position, groundDetectionRadius);
    }


    private void Update()
    {
        animator.SetBool("pursue", kirboInSight);
    }

    private void FixedUpdate()
    {

        if (kirboInSight && !exploding)
        {
            
            grounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundDetectionRadius, whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    grounded = true;
                }
            }

            if (grounded)
            {
                
                Vector2 dirToPlayer = GameManager.instance.kirbo.gameObject.transform.position - this.transform.position;
                if(dirToPlayer.x < 0)
                {
                    rigidBody.velocity = new Vector2(-1*jumpForce.x, jumpForce.y);
                    if (facingRight) Flip();
                }else if(dirToPlayer.x > 0)
                {
                    rigidBody.velocity = new Vector2(1 * jumpForce.x, jumpForce.y);
                    if (!facingRight) Flip();
                }

            }

            Physics2D.OverlapCircleNonAlloc(transform.position, explosionRadius, collidersInExplosionArea);
            foreach(Collider2D col in collidersInExplosionArea)
            {
                if(col != null)
                {
                    
                    if (col.gameObject == GameManager.instance.kirbo.gameObject)
                    {
                        
                        explode();
                    }
                }
            }


        }
    }


    public void explode()
    {
        exploding = true;
        rigidBody.velocity = new Vector2();
        rigidBody.isKinematic = true;
        animator.SetTrigger("explode");
        
    }

    public void explosionDmg()
    {
        Physics2D.OverlapCircleNonAlloc(transform.position, explosionRadius, collidersInExplosionArea);
        foreach (Collider2D col in collidersInExplosionArea)
        {
            if (col != null)
            {
                if (col.gameObject == GameManager.instance.kirbo.gameObject)
                {
                    GameManager.instance.kirbo.GetComponent<PlayerMovement>().addHealth(-1);
                    
                }
            }
        }

        GameObject.Destroy(this);
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


}
