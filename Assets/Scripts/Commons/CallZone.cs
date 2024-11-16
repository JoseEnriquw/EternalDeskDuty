using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallZone : MonoBehaviour
{
    public bool isPlayerInZone = false;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Player enter call zone");
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player leave call zone");
            isPlayerInZone = false;
        }
    }
}
