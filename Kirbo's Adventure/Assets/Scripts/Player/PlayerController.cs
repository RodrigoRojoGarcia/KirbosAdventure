using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    #region Movement Not Mine
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

    private float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    public float k_CeilingRadius = .1f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;
    #endregion

    [SerializeField] private float jumpDuration = 0f;
    [SerializeField] private bool floating = false;
    private Animator myAnimator;

    public AbsorbingZone absorbingArea;

    [SerializeField] private GameObject objectInMouth;

    [SerializeField] private GameObject kirboProjectile;
    [SerializeField] private GameObject kirboProjectileSpawn;
    [SerializeField] private float projectileSpeed;

    [SerializeField] private int health;


    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();

        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        myAnimator.SetFloat("jump_speed", m_Rigidbody2D.velocity.y);
        myAnimator.SetBool("grounded", m_Grounded);
        myAnimator.SetFloat("jump_duration", jumpDuration);
        myAnimator.SetBool("float", floating);

        if(objectInMouth != null)
        {
            myAnimator.SetBool("mouthfull", true);
        }
        else
        {
            myAnimator.SetBool("mouthfull", false);
        }
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        if (m_FacingRight)
        {
            absorbingArea.setWhatKirboIsFacing(-1);
        }
        else
        {
            absorbingArea.setWhatKirboIsFacing(1);
        }

    }


    public void Move(float move, bool crouch, bool jump)
    {
        
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (true)
        {

            // If crouching
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (jump)
        {
            Jump();
        }
        if (!m_Grounded)
        {
            jumpDuration += Time.deltaTime;
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    private void Jump()
    {
        if (!m_Grounded)
        {
            floating = true;
        }
        if (m_AirControl)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            //m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            Vector2 vel = m_Rigidbody2D.velocity;
            vel.y = m_JumpForce;
            m_Rigidbody2D.velocity = vel;
            jumpDuration = 0f;
        }
        
    }


    public void onLandTouch()
    {
        jumpDuration = 0f;
        floating = false;
        m_AirControl = true;
    }

    public void absorb()
    {
        absorbingArea.setAbsorbing(true);
    }


    public void setObjectInMouth(GameObject go) { this.objectInMouth = go; }
    public GameObject getObjectInMouth() { return this.objectInMouth; }

    public bool isGrounded() { return this.m_Grounded; }



    public void shoot()
    {
        GameObject b = Instantiate(kirboProjectile, kirboProjectileSpawn.transform.position, kirboProjectileSpawn.transform.rotation);

        Vector2 projSpeed;
        if (m_FacingRight)
        {
            projSpeed = new Vector2(projectileSpeed, 0);
        }
        else
        {
            projSpeed = new Vector2(-projectileSpeed, 0);
        }

        b.GetComponent<Rigidbody2D>().velocity = projSpeed;

        objectInMouth = null;
    }

    public bool isFloating() { return this.floating; }
    public void changeAirControl()
    {
        floating = false;
        m_AirControl = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (absorbingArea.getAbsorbing() && collision.gameObject.tag.Equals("absorbable"))
        {
            return;
        }

        if (collision.gameObject.GetComponent<EnemyController>() != null)
        {

            health--;
        }
    }

    public void addHealth(int h) { this.health += h; }

    public int getHealth() { return this.health; }

}