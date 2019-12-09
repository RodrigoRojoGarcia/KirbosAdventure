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
        if (midDash) return;
        if(timeSinceDash < restingTime)
        {

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
        Vector2 dir = GameManager.instance.kirbo.gameObject.transform.position - transform.position;
        dir.Normalize();

        rigidBody.velocity = dir * dashSpeed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        midDash = false;

    }

}
