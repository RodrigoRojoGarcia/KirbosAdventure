using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrackoController : EnemyController
{
    [SerializeField] private float health;
    [SerializeField] private float healthPhaseTwoThreshhold;
    [SerializeField] private float healthPhaseThreeThreshhold;


    [SerializeField] private KrackoPhaseOne phaseOne;
    [SerializeField] private KrackoPhaseTwo phaseTwo;
    [SerializeField] private KrackoPhaseThree phaseThree;

    private void FixedUpdate()
    {
        if(health > healthPhaseTwoThreshhold)
        {
            phaseOne.behaviourFixedUpdate(rigidBody, animator, Time.deltaTime);
        }else if(health > healthPhaseThreeThreshhold)
        {
            phaseTwo.behaviourFixedUpdate();
        }
        else
        {
            phaseThree.behaviourFixedUpdate();
        }
    }



}
