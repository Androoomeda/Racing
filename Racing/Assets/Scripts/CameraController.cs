using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 offset;

    private void Update()
    {
        transform.position = followTarget.position + offset;
    }
}
