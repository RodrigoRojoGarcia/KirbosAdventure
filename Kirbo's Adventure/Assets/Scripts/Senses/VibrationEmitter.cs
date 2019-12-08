using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationEmitter : MonoBehaviour
{
    [SerializeField] private float vibrationIntensity;
    [SerializeField] private float vibrationAttenuation;
    [SerializeField] private GameObject emitterObject;
    private Dictionary<int, VibrationReceiver> vibrationReceivers;

    // Start is called before the first frame update
    void Start()
    {
        vibrationReceivers = new Dictionary<int, VibrationReceiver>();
        if(emitterObject == null)
        {
            emitterObject = this.gameObject;
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        VibrationReceiver vibReceiver;
        vibReceiver = collision.gameObject.GetComponent<VibrationReceiver>();
        if (vibReceiver == null) return;
        int objId = collision.gameObject.GetInstanceID();

        vibrationReceivers.Add(objId, vibReceiver);
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        VibrationReceiver vibReceiver;
        vibReceiver = collision.gameObject.GetComponent<VibrationReceiver>();
        if (vibReceiver == null) return;
        int objId = collision.gameObject.GetInstanceID();

        vibrationReceivers.Remove(objId);
    }

    public void EmitVibration()
    {
        GameObject vibrationReceiverObject;
        Vector3 vibrationReceiverPosition;
        float intensity;
        float distance;
        Vector2 emitterPos = emitterObject.transform.position;

        foreach(VibrationReceiver vr in vibrationReceivers.Values)
        {
            vibrationReceiverObject = vr.gameObject;
            vibrationReceiverPosition = vibrationReceiverObject.transform.position;
            distance = Vector2.Distance(vibrationReceiverPosition, emitterPos);
            intensity = vibrationIntensity;
            intensity -= vibrationAttenuation * distance;

            if (intensity < vr.getVibrationThreshold()) continue;

            vr.Receive(intensity, emitterPos);

        }
    }
}
