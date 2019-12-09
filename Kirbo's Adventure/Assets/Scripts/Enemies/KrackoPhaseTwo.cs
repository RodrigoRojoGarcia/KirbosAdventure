using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrackoPhaseTwo : MonoBehaviour
{
    [SerializeField] private float dashSpeed;
    [SerializeField] private Vector2 speed;


    [SerializeField] private float restingTime;
    private float timeSinceDash = 0;

    private bool midDash = false;

    private Rigidbody2D rigidBody;
    private Animator animator;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void behaviourFixedUpdate(float delta)
    {
        animator.SetBool("midDash", midDash);

        if (midDash) return;
        if(timeSinceDash < restingTime)
        {
            evade();
            timeSinceDash += delta;
        }
        else
        {
            dash();
            timeSinceDash = 0;
    
        }
    }

    public void dash()
    {
        midDash = true;
        gameObject.GetComponent<KrackoController>().setVulnerable(false);
        Vector2 dir = GameManager.instance.kirbo.gameObject.transform.position - transform.position;
        dir.Normalize();

        rigidBody.velocity = dir * dashSpeed;
    }


    public void evade()
    {
        Vector2 dir =   transform.position - GameManager.instance.kirbo.gameObject.transform.position;
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



    private void OnCollisionEnter2D(Collision2D collision)
    {
        midDash = false;
        gameObject.GetComponent<KrackoController>().setVulnerable(true);
    }

}
