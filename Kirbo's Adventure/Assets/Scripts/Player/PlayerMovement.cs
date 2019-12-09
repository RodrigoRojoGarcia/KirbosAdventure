using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    public PlayerController controller;

    public float runSpeed = 40f;

    private float horizontalMove = 0f;

    private bool jump = false;

    private bool crouch = false;



    private bool oneJumpPress = false;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private bool absorb = false;


    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if ((Input.GetAxisRaw("Vertical") > 0.1) && !this.oneJumpPress)
        {

            this.oneJumpPress = true;
            this.jump = true;

        }
        else if(Input.GetAxisRaw("Vertical") < -0.1)
        {
            crouch = true;
        }
        else if(Input.GetAxisRaw("Vertical")>-0.1 && Input.GetAxisRaw("Vertical")<0.1)
        {
            this.oneJumpPress = false;
            crouch = false;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("release");
            GameManager.instance.kirbo.setObjectInMouth(null);
            if (controller.isFloating())
            {
                controller.changeAirControl();
            }
        }
        else
        {
            animator.ResetTrigger("release");
        }

        absorb = Input.GetKey(KeyCode.Space);
        animator.SetBool("jump", jump);
        animator.SetBool("crouch", crouch);
        animator.SetFloat("speed", horizontalMove);
        animator.SetBool("absorb", absorb);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    


}
