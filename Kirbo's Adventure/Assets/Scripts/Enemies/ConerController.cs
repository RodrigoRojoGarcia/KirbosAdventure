using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConerController : EnemyController
{
    

    [SerializeField] private float speed;

    private bool hidedInShell = false;

    private bool facingRight = false;

    private Rigidbody2D rigidBody;

    [SerializeField] private GameObject inFrontCollider;
    [SerializeField] private GameObject downInFrontCollider;
    [SerializeField] private float inFrontCollidersRadius;

    private Collider2D[] collidersInFront = new Collider2D[8];
    private Collider2D[] collidersDownInFront = new Collider2D[8];

    private Animator animator;


    [SerializeField] private LayerMask whatIsGround;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(downInFrontCollider.transform.position, inFrontCollidersRadius);
        Gizmos.DrawWireSphere(inFrontCollider.transform.position, inFrontCollidersRadius);
    }


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("hideInShell", hidedInShell);
    }

    private void FixedUpdate()
    {

        if (!hidedInShell)
        {
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
                Debug.Log("UwU");
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
                    Debug.Log("OwO");
                    Flip();
                }

                collidersDownInFront = new Collider2D[8];

            }

        }

        
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    public override void SeePlayer()
    {
        this.hidedInShell = true;
    }
    public override void StopSeePlayer()
    {
        this.hidedInShell = false;
    }
}
