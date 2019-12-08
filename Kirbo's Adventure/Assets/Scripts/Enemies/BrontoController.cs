using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrontoController : EnemyController
{
    [SerializeField] private float detectingRadius;
    [SerializeField] private float speed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shotDelay;
    [SerializeField] private float bulletSpeed;

    private bool patrol;
    private bool pursue;


    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }


    private void Fire()
    {
        GameObject b = Instantiate(bullet, transform.position, transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
    }

    public void StopFiring()
    {
        CancelInvoke("Fire");
    }


    public void StartFiring()
    {
        InvokeRepeating("Fire", shotDelay, shotDelay);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("UwU");
    }

    public override void SeePlayer()
    {
        
    }

    public override void StopSeePlayer()
    {
        
    }
}
