using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallDetector : MonoBehaviour
{
    [SerializeField] float detectionRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;

    Collider2D[] colliders = new Collider2D[1];

    public bool IsWall => Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, colliders, groundLayer) != 0;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
