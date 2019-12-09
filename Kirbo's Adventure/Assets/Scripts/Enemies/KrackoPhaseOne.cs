using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrackoPhaseOne : MonoBehaviour
{

    [SerializeField] private float height;
    [SerializeField] private float speed;

    [SerializeField] private float chargeTime;
    private float timeSinceLastThunder = 0;

    [SerializeField] private float detectionThreshhold;

    [SerializeField] private GameObject thunder;
    [SerializeField] private GameObject thunderSpawn;
    [SerializeField] private float thunderSpeed;

    private bool start = false;

    public void behaviourFixedUpdate(Rigidbody2D rigidBody, Animator animator, float delta)
    {
        if (!start)
        {
            start = true;
            transform.position = new Vector2(transform.position.x, height);
        }
        
        if(timeSinceLastThunder < chargeTime)
        {
            Vector2 dir = GameManager.instance.kirbo.gameObject.transform.position - transform.position;
            if(Mathf.Abs(transform.position.x - GameManager.instance.kirbo.gameObject.transform.position.x) < detectionThreshhold)
            {
                rigidBody.velocity = new Vector2();
            }else
            if (dir.x > 0)
            {
                rigidBody.velocity = new Vector2(speed, 0);
            }
            else if(dir.x < 0)
            {
                rigidBody.velocity = new Vector2(-speed, 0);
            }

            timeSinceLastThunder += delta;
        }
        else
        {
            animator.SetTrigger("thunder");
            
        }


    }


    public void onAttackByKirbo()
    {

    }

    public void launchThunder()
    {
        GameObject t = Instantiate(thunder, thunderSpawn.transform.position, thunderSpawn.transform.rotation);

        Vector2 dir = new Vector2(0,-1);
        

        t.GetComponent<Rigidbody2D>().velocity = dir * thunderSpeed;
    }


    public void resetTimeSinceLastThunder()
    {
        timeSinceLastThunder = 0;
    }
}
