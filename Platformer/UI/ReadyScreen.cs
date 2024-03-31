using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyScreen : MonoBehaviour
{
    [SerializeField] private VoidEventChannel readyEventChannel;

    // anim event func
    void LevelStart()
    {
        readyEventChannel.Broadcast();
        GetComponent<Canvas>().enabled = false;
        GetComponent<Animator>().enabled = false;
    }
    
}
