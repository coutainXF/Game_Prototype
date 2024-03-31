using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    
    [SerializeField] private VoidEventChannel gateEventChannel;
    
    void Open()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        gateEventChannel.AddListener(Open);
    }

    private void OnDisable()
    {
        gateEventChannel.RemoveListener(Open);
    }
}
