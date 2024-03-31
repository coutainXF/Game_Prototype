using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedDetective : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    private Collider[] _colliders = new Collider[1];
    public bool isGround =>Physics.OverlapSphereNonAlloc(transform.position,detectionRadius,_colliders,groundLayer)!=0;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,detectionRadius);
    }
}
