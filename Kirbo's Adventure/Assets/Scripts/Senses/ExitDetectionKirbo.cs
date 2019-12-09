using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDetectionKirbo : MonoBehaviour
{
    [SerializeField] private GameObject thisAgent;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == GameManager.instance.kirbo.gameObject)
        {
            thisAgent.GetComponent<EnemyController>().StopSeePlayer();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
