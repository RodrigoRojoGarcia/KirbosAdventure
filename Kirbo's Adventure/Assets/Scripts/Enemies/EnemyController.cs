using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{

    [SerializeField] private GameObject thisPrefab;


    public GameObject getThis() { return thisPrefab; }


    public abstract void SeePlayer();
    public abstract void StopSeePlayer();
}
