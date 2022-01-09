using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerRotation : MonoBehaviour
{

    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float cameraHeight;
    [SerializeField] float rotationSpeed;
    [SerializeField] float rotationSmooth;

    [SerializeField] public Camera Camera;

    float smoothing;

    Transform lookAtTarget;

    internal Transform player;

    internal void Init()
    {
        lookAtTarget = new GameObject("lookAtTarget").transform;
    }

    Vector3 previousPosition;

    private void Update()
    {
        Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
        cameraOffset = turnAngle * cameraOffset;

        Vector3 newPos = player.transform.position + cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, rotationSmooth);

        //Vector3 velocity = player.Movement.rb.transform.position - previousPosition;
        //previousPosition = player.Movement.rb.transform.position;

        //transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, rotationSmooth);

        lookAtTarget.position = player.transform.position + Vector3.up * cameraHeight;
        transform.LookAt(lookAtTarget);
    }
}
