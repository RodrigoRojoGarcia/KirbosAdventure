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

    protected virtual void Awake()
    {
        base.Awake();
        
    }

    private void FixedUpdate()
    {

        if (kirboInSight)
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
                phaseThree.behaviourFixedUpdate(Time.deltaTime);
            }
        }
        
    }

    public void setVulnerable(bool vul) { this.vulnerable = vul; }
    public bool isVulnerable() { return this.vulnerable; }


    public override void getHit()
    {
        health -= 10;
    }

    public void launchThunder()
    {
        if(health > healthPhaseTwoThreshhold)
        {
            phaseOne.launchThunder();
        }else if(health < healthPhaseThreeThreshhold)
        {
            phaseThree.launchThunder();
        }
    }

    
    public int getHealth() { return this.health; }
}
