using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _offset;
    [SerializeField] private Transform player;
    private Vector3 _currentVelocity = Vector3.zero;
    float t = 0.1f;

    private void Awake()
    {
        _offset = transform.position - player.position;    
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = player.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, t);
    }
}
