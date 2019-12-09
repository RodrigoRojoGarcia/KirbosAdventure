using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrackoPhaseThree : MonoBehaviour
{

    [SerializeField] private float dashSpeed;
    [SerializeField] private Vector2 speed;


    [SerializeField] private float restingTime;
    private float timeSinceDash = 0;

    private bool midDash = false;

    private Rigidbody2D rigidBody;
    private Animator animator;

    [SerializeField] private float distanceOfShooting;

    [SerializeField] private int maxDash;
    private int dashCount = 0;

    [SerializeField] private GameObject thunder;
    [SerializeField] private GameObject thunderSpawn;
    [SerializeField] private float thunderSpeed;

    [SerializeField] private float timeBetweenTwoDashes;
    private float timeSinceLastDash = 0;

    private bool firing = false;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanceOfShooting);
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void behaviourFixedUpdate(float delta)
    {
        animator.SetBool("midDash", midDash);
        
        if (midDash) return;
        
        if(dashCount > 0 && dashCount <= maxDash)
        {
            if(timeSinceLastDash >= timeBetweenTwoDashes)
            {
                dash();
            }
            else
            {
                gainAltitude();
                timeSinceLastDash += delta;
            }
            
            
        }
        else
        {
            dashCount = 0;
            if (timeSinceDash < restingTime)
            {
                if(Vector2.Distance(GameManager.instance.kirbo.gameObject.transform.position, transform.position) > distanceOfShooting)
                {
                    if (!firing)
                    {
                        animator.SetTrigger("thunder");
                        firing = true;
                    }   
                }
                else
                {
                    evade();
                }
                timeSinceDash += delta;
            }
            else
            {
                dash();
                timeSinceDash = 0;

            }
        }
        
    }


    public void dash()
    {
        midDash = true;
        gameObject.GetComponent<KrackoController>().setVulnerable(false);
        Vector2 dir = GameManager.instance.kirbo.gameObject.transform.position - transform.position;
        dir.Normalize();

        rigidBody.velocity = dir * dashSpeed;
        dashCount++;
        timeSinceLastDash = 0;
    }

    public void gainAltitude()
    {
        Vector2 vel = rigidBody.velocity;
        vel = new Vector2(0, speed.y);
        rigidBody.velocity = vel;
    }



    public void evade()
    {
        Vector2 dir = transform.position - GameManager.instance.kirbo.gameObject.transform.position;
        dir.Normalize();
        Vector2 vel = new Vector2();
        if (dir.x > 0)
        {
            vel = new Vector2(speed.x, speed.y);
        }
        else
        {
            vel = new Vector2(-speed.x, speed.y);
        }
        rigidBody.velocity = vel;
    }

    public void launchThunder()
    {
        GameObject t = Instantiate(thunder, thunderSpawn.transform.position, thunderSpawn.transform.rotation);

        Vector2 dir = GameManager.instance.kirbo.gameObject.transform.position - transform.position;


        t.GetComponent<Rigidbody2D>().velocity = dir * thunderSpeed;

        Vector3 relativePos = GameManager.instance.kirbo.gameObject.transform.position - t.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        rotation.x = t.transform.rotation.x;
        rotation.y = t.transform.rotation.y;
        t.transform.rotation = rotation;
        firing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        midDash = false;
        gameObject.GetComponent<KrackoController>().setVulnerable(true);
    }
}
