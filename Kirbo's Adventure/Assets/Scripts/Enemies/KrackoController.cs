using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrackoController : EnemyController
{
    [SerializeField] private int health;
    [SerializeField] private float healthPhaseTwoThreshhold;
    [SerializeField] private float healthPhaseThreeThreshhold;


    [SerializeField] private KrackoPhaseOne phaseOne;
    [SerializeField] private KrackoPhaseTwo phaseTwo;
    [SerializeField] private KrackoPhaseThree phaseThree;

    [SerializeField] private int lifeLostFromHit = 10;


    private bool vulnerable = true;

    private void FixedUpdate()
    {
        if(health > healthPhaseTwoThreshhold)
        {
            phaseOne.behaviourFixedUpdate(Time.deltaTime);
        }else if(health > healthPhaseThreeThreshhold)
        {
            phaseTwo.behaviourFixedUpdate(Time.deltaTime);
        }
        else
        {
            phaseThree.behaviourFixedUpdate();
        }
    }

    public void setVulnerable(bool vul) { this.vulnerable = vul; }
    public bool isVulnerable() { return this.vulnerable; }


    public override void getHit()
    {
        health -= 10;
    }

}
