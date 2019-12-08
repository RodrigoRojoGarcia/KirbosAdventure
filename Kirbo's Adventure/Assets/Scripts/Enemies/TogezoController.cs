using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogezoController : EnemyController
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationThreshold;
    [SerializeField] private GameObject[] rotationWaypoints;
    [SerializeField] private int nextWaypoint = 1;

    private float distToPoint;

    private void Start()
    {
        Vector3 scale = transform.localScale;
        scale.z *= -1;
        transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        distToPoint = Vector2.Distance(transform.position, rotationWaypoints[nextWaypoint].transform.position);

        transform.position = Vector2.MoveTowards(transform.position, rotationWaypoints[nextWaypoint].transform.position,
            movementSpeed * Time.deltaTime);

        if(distToPoint < rotationThreshold)
        {
            TakeTurn();
        }
    }

    private void TakeTurn()
    {
        Vector3 currRot = transform.eulerAngles;
        currRot.z += rotationWaypoints[nextWaypoint].transform.eulerAngles.z;
        transform.eulerAngles = currRot;
        ChooseNextWaypoint();
    }

    private void ChooseNextWaypoint()
    {
        nextWaypoint++;

        if(nextWaypoint >= rotationWaypoints.Length)
        {
            nextWaypoint = 0;
        }
    }

    public override void SeePlayer()
    {
        
    }

    public override void StopSeePlayer()
    {

    }
}
