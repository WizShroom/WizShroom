using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.3f;
    public Vector3 offset = Vector3.zero;
    Vector3 hiddenOffset = Vector3.zero;

    private Vector3 velocity = Vector3.zero;
    Vector3 targetPosition;

    private void Awake()
    {
        hiddenOffset = offset;
    }

    void LateUpdate()
    {
        targetPosition = player.position + hiddenOffset;
        Vector3 directionNormalized = (targetPosition - transform.position).normalized;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
