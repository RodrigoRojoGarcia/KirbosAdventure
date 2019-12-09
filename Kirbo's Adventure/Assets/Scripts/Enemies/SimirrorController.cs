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
    [SerializeField] private GameObject projectileSpawner;


    private bool kirboCloseEnough = false;
    private bool firing = false;
    private bool facingRight = true;



    [SerializeField] private float tpRadius;
    [SerializeField] private Vector2[] tpDir;
    [SerializeField] private string tagWalls;
    [SerializeField] private float tpTimeDelay;
    private float timeSinceLastTp = 0;


    


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(jumpCheck.transform.position, colliderInFrontRadius);
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundDetectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tpRadius);

        Gizmos.color = Color.cyan;
        foreach(Vector2 v in tpDir)
        {
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x+v.x, transform.position.y+v.y));
        }
        

        foreach(Vector2 v in getPosibleViablePoints())
        {
            Gizmos.DrawWireSphere(v, 0.01f);
        }

    }


    private void Awake()
    {
        base.Awake();
        rigidBody.velocity = new Vector2(speed, 0);
        xCenterPatrol = transform.position.x;
        
    }

    private void Update()
    {
        animator.SetBool("kirboInSight", kirboInSight);
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
            animator.SetBool("StartFiring", false);
            patrol();
        }
        else
        {
            
            if (timeSinceLastTp <= tpTimeDelay)
            {
                
                animator.SetBool("StartFiring", true);

                timeSinceLastTp += Time.deltaTime;
            }
            else
            {
                animator.SetBool("StartFiring", false);
                animator.SetTrigger("Teleport");
                
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

            rigidBody.velocity = (new Vector2(rigidBody.velocity.x, jumpForce));
        }
        collidersInFront = new Collider2D[8];
    }

    public void teleport()
    {
        List<Vector2> viablePoints = getPosibleViablePoints();
        if(viablePoints.Count != 0)
        {
            int tpPoint = Random.Range(0, viablePoints.Count - 1);
            transform.position = viablePoints[tpPoint] + new Vector2(0, 0.5f);
        }
        


    }


    public void Fire()
    {
        GameObject b = Instantiate(bullet, projectileSpawner.transform.position, projectileSpawner.transform.rotation);

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



    public List<Vector2> getPosibleViablePoints()
    {
        List<Vector2> viablePoints = new List<Vector2>();


        foreach(Vector2 dir in tpDir)
        {
            RaycastHit2D[] raycastHits = Physics2D.RaycastAll(transform.position, dir, tpRadius);

            //Si la Y es negativa nos podremos teletransportar en el primer hit de cada raycast
            if(dir.y <= 0){
                bool wallHit = false;
                int i = 0;
                while(!wallHit && i < raycastHits.Length)
                {
                    GameObject hitObj = raycastHits[i].collider.gameObject;
                    if (hitObj.tag.Equals(tagWalls))
                    {
                        wallHit = true;
                    }
                    else
                    {
                        i++;
                    }
                    
                }
                if (wallHit)
                {
                    if(Vector2.Distance(raycastHits[i].point, transform.position) > 0.5f)
                    viablePoints.Add(raycastHits[i].point);

                    RaycastHit2D[] auxHits = Physics2D.RaycastAll(raycastHits[i].point + dir, dir, tpRadius - Vector2.Distance(transform.position, raycastHits[i].point));
                    bool auxWallHit = false;
                    int j = 0;
                    while(!auxWallHit && j < auxHits.Length)
                    {
                        GameObject hitObj = auxHits[j].collider.gameObject;
                        if (hitObj.tag.Equals(tagWalls))
                        {
                            auxWallHit = true;
                        }
                        else
                        {
                            j++;
                        }
                    }
                    if (auxWallHit)
                    {

                        Collider2D[] collidersInPoint = new Collider2D[2];
                        Physics2D.OverlapCircleNonAlloc(auxHits[j].point + new Vector2(0, 0.5f), 0.26f, collidersInPoint, whatIsGround);

                        bool noTp = false;
                        foreach(Collider2D col in collidersInPoint)
                        {
                            if(col != null)
                            {
                                noTp = true;
                            }
                        }
                        if(!noTp)
                        viablePoints.Add(auxHits[j].point);
                    }


                }



            }
            else if(dir.y > 0)
            {
                bool wallHit = false;
                int i = 0;
                while(!wallHit && i < raycastHits.Length)
                {
                    
                    
                    GameObject hitObj = raycastHits[i].collider.gameObject;
                    if (hitObj.tag.Equals(tagWalls))
                    {
                        RaycastHit2D[] auxHits = Physics2D.RaycastAll(
                            new Vector2(transform.position.x + (dir.x)*tpRadius, transform.position.y + (dir.y) * tpRadius), 
                            -dir, tpRadius - 0.01f);
                        bool auxWallHit = false;
                        int j = 0;
                        while(!auxWallHit && j < auxHits.Length)
                        {
                            GameObject auxHitObj = auxHits[j].collider.gameObject;
                            if (auxHitObj.tag.Equals(tagWalls))
                            {
                                viablePoints.Add(auxHits[j].point);
                                auxWallHit = true;
                            }
                            j++;
                        }
                        wallHit = true;
                    }
                    else
                    {
                        i++;
                    }
                    
                }

            }

            



        }






        return viablePoints;
    }

    public void setFiring(bool fire) { this.firing = fire; }
    public void resetTimeSinceLastTp() { this.timeSinceLastTp = 0; }

    public override void getHit()
    {
        Destroy(gameObject);

    }

    public new void StopSeePlayer()
    {
        kirboInSight = false;
        xCenterPatrol = transform.position.x;
    }
}
