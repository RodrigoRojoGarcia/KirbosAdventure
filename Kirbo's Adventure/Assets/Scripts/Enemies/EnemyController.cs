using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{

    [SerializeField] private GameObject thisPrefab;


    protected bool kirboInSight = false;
    protected Rigidbody2D rigidBody;
    protected Animator animator;

    protected virtual void Awake()
    {
        Physics2D.IgnoreLayerCollision(9, 9);
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public GameObject getThis() { return thisPrefab; }


    public void SeePlayer()
    {
        kirboInSight = true;
    }
    public void StopSeePlayer()
    {
        kirboInSight = false;
    }
}
