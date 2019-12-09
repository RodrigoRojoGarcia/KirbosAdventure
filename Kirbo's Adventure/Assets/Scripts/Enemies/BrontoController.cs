using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrontoController : EnemyController
{
    [SerializeField] private float height;
    [SerializeField] private float heightTreshhold;
    [SerializeField] private string tagWalls;

    [SerializeField] private float detectingRadius;

    [SerializeField] private float evadeDistance;

    [SerializeField] private Vector2 speed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shotDelay;
    [SerializeField] private float bulletSpeed;


    [SerializeField] private GameObject inFrontCollider;
    [SerializeField] private float inFrontColliderRadius;
    [SerializeField] private GameObject topCollider;

    private Collider2D[] collidersInFront = new Collider2D[8];
    private Collider2D[] colliderOnTop = new Collider2D[8];

    [SerializeField] private LayerMask whatIsGround;

    private bool patrol;
    private bool pursue;

    private bool facingRight = false;


    private bool midDash = false;

    private bool firing = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(inFrontCollider.transform.position, inFrontColliderRadius);
        Gizmos.DrawWireSphere(topCollider.transform.position, inFrontColliderRadius);
    }


    private void FixedUpdate()
    {



        if(rigidBody.velocity.x > 0 && !facingRight)
        {
            Flip();
        }else if(rigidBody.velocity.x < 0 && facingRight)
        {
            Flip();
        }

        if (midDash) return;

        #region HeightController

        float actualHeight = 0;


        Vector2 rayDir = Vector2.down;

        RaycastHit2D[] raycastHits;
        raycastHits = Physics2D.RaycastAll(transform.position, rayDir);

        int i = 0;
        bool wallHit = false;


        while (!wallHit && i < raycastHits.Length)
        {
            GameObject hitObj = raycastHits[i].collider.gameObject;
            string tag = hitObj.tag;
            if (tag.Equals(tagWalls))
            {
                actualHeight = raycastHits[i].point.y + this.height;
                wallHit = true;
            }
            i++;
        }

        #endregion

        Physics2D.OverlapCircleNonAlloc(inFrontCollider.transform.position, inFrontColliderRadius, collidersInFront, whatIsGround);
        bool turn = false;
        foreach (Collider2D col in collidersInFront)
        {

            if (col != null)
            {
                if (col.gameObject == this) continue;
                turn = true;
            }
        }
        if (turn)
        {
            Flip();
            collidersInFront = new Collider2D[8];
        }

        if (!kirboInSight)
        {
            patrolMovement(actualHeight);
        }
        else
        {

            if (GameManager.instance.kirbo.isGrounded())
            {
                if(transform.position.y < actualHeight - heightTreshhold)
                {
                    gainAltitude();
                }
                else
                {
                    dashMovement();
                }
                
            }
            else
            {
                float distance = Vector2.Distance(GameManager.instance.kirbo.gameObject.transform.position, transform.position);
                
                if (distance < evadeDistance)
                {

                    evadeMovement(actualHeight);
                }
                else
                {
                    
                    
                    if(!firing)
                    StartFiring();
                }


                
            }
        }
        
        

    }


    private void Fire()
    {
        

        GameObject b = Instantiate(bullet, transform.position, transform.rotation);

        Vector2 dir = GameManager.instance.kirbo.gameObject.transform.position - transform.position;
        dir.Normalize();

        b.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
        
    }

    public void StopFiring()
    {
        firing = false;
       
        CancelInvoke("Fire");
    }


    public void StartFiring()
    {
        firing = true;
        
        rigidBody.velocity = new Vector2();
        InvokeRepeating("Fire", 0, shotDelay);
    }




    private void Flip()
    {
        facingRight = !facingRight;

        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    public void patrolMovement(float actualHeight)
    {
        StopFiring();
        Vector2 vel = rigidBody.velocity;
        if (transform.position.y > actualHeight + heightTreshhold)
        {

            vel = new Vector2(speed.x, -speed.y);

        }
        else if (transform.position.y < actualHeight - heightTreshhold)
        {
            vel = new Vector2(speed.x, speed.y);
        }

        if ((facingRight && vel.x < 0) || (!facingRight && vel.x > 0))
        {
            vel.x *= -1;
        }


        Physics2D.OverlapCircleNonAlloc(topCollider.transform.position, inFrontColliderRadius, colliderOnTop, whatIsGround);
        bool turn = false;
        foreach (Collider2D col in colliderOnTop)
        {

            if (col != null)
            {
                if (col.gameObject == this) continue;
                turn = true;
            }
        }
        if (turn)
        {
            if(vel.y > 0)
            {
                vel.y *= -1;
            }
            colliderOnTop = new Collider2D[8];
        }


        rigidBody.velocity = vel;
    }


    public void dashMovement()
    {
        StopFiring();
        midDash = true;

        Vector2 dir = GameManager.instance.kirbo.gameObject.transform.position - transform.position;
        dir.Normalize();

        rigidBody.velocity = dir * dashSpeed;
    }

    public void evadeMovement(float actualHeight)
    {
        StopFiring();
        Vector2 dir =  GameManager.instance.kirbo.gameObject.transform.position - transform.position;
        dir.Normalize();
        Vector2 vel = new Vector2();
        if(dir.x > 0)
        {
            vel = new Vector2(speed.x, speed.y);
        }
        else
        {
            vel = new Vector2(-speed.x, speed.y);
        }
        rigidBody.velocity = vel;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        midDash = false;
        
    }

    public void gainAltitude()
    {
        Vector2 vel = rigidBody.velocity;
        vel = new Vector2(speed.x, speed.y);
        rigidBody.velocity = vel;
    }


    public override void getHit()
    {
        Destroy(gameObject);

    }
}
