using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationReceiver : MonoBehaviour
{
    [SerializeField] private float vibrationThreshold;

    public float getVibrationThreshold() { return this.vibrationThreshold; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Receive(float intensity, Vector2 position)
    {

    }
}
