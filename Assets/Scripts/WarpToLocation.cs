using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarpToLocation : MonoBehaviour
{
    public RoomConnection ourConnection;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NavMeshAgent otherAgent = other.GetComponent<NavMeshAgent>();
            otherAgent.Warp(ourConnection.connectedRoom.roomLanding.position);
            otherAgent.SetDestination(otherAgent.transform.position);
        }
    }
}
