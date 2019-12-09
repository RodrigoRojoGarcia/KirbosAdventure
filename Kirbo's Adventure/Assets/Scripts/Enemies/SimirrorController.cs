using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimirrorController : EnemyController
{
    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject jumpCheck;
    [SerializeField] private float colliderInFrontRadius;
    private Collider2D[] collidersInFront = new Collider2D[8];


    [SerializeField] private float groundDetectionRadius;
    [SerializeField] private GameObject groundCheck;
    private bool grounded = false;

    [SerializeField] private LayerMask whatIsGround;
    

    [SerializeField] private float xDistancePatrolThreshhold;
    private float xCenterPatrol;



    [SerializeField] private GameObject bullet;
    [SerializeField] private float shotDelay;
    [SerializeField] private float bulletSpeed;


    private bool kirboInSight = false;
    private bool kirboCloseEnough = false;
    private bool firing = false;
    private bool facingRight = true;

    private Rigidbody2D rigidBody;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(jumpCheck.transform.position, colliderInFrontRadius);
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundDetectionRadius);
    }


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(speed, 0);
        xCenterPatrol = transform.position.x;
    }

    private void FixedUpdate()
    {
        if (rigidBody.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rigidBody.velocity.x < 0 && facingRight)
        {
            Flip();
        }

        if (!kirboInSight)
        {
            patrol();
        }
        else
        {
            if (kirboCloseEnough)
            {
                StartFiring();
            }
            else
            {
                teleport();
            }
        }
    }

    public void patrol()
    {
        #region Grounded
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundDetectionRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
            }
        }
        #endregion



        Vector2 vel = rigidBody.velocity;
        if (transform.position.x > xCenterPatrol + xDistancePatrolThreshhold)
        {
            speed *= -1;
        }
        else if(transform.position.x < xCenterPatrol - xDistancePatrolThreshhold)
        {
            speed *= -1;
        }

        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

        Physics2D.OverlapCircleNonAlloc(jumpCheck.transform.position, colliderInFrontRadius, collidersInFront, whatIsGround);

        bool jump = false;
        foreach(Collider2D col in collidersInFront)
        {
            jump = jump || col != null;
        }
        if (jump && grounded)
        {
            Debug.Log("jump");
            rigidBody.velocity = (new Vector2(rigidBody.velocity.x, jumpForce));
        }
        collidersInFront = new Collider2D[8];
    }

    public void teleport()
    {

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

    public override void SeePlayer()
    {
        kirboInSight = true;
    }

    public override void StopSeePlayer()
    {
        kirboInSight = false;
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
