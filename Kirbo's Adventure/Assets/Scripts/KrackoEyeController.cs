using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrackoEyeController : MonoBehaviour
{

    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(11, 12);
    }


    private void FixedUpdate()
    {
        Vector2 dir = GameManager.instance.kirbo.transform.position - transform.position;
       
        if(dir.x < 0 && dir.y < 0)
        {
            animator.SetTrigger("lookDownLeft");
        }else
        if(dir.x > 0 && dir.y > 0)
        {
            animator.SetTrigger("lookUpRight");
        }else
        if(dir.x < 0 && dir.y > 0){
            animator.SetTrigger("lookUpLeft");
        }else
        if(dir.x > 0 && dir.y < 0)
        {
            animator.SetTrigger("lookDownRight");
        }
        
    }
}
