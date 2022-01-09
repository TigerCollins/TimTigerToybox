﻿using UnityEngine;

public class FPSRotation : MonoBehaviour
{
    Vector2 _mouseAbsolute;
    Vector2 _smoothMouse;

    public bool lockCursor, invertY;
    [SerializeField] float focusSpeed;
    public float sensitivity = 2f;
    [Range(1, 3)]
    public float smoothing = 1f;
    public Vector2 clampInDegrees = new Vector2(360, 180);
    public Vector2 targetDirection;
    public Vector2 targetCharacterDirection;

    // Assign this if there's a parent object controlling motion, such as a Character Controller.
    // Yaw rotation will affect this object instead of the camera if set.
    public GameObject characterBody;

    Transform focusSubject;
    bool frozen;

    public bool IsFocusing => focusSubject != null;

    internal Rigidbody player;

    internal void Init()
    {
        // Set target direction to the camera's initial orientation.
        targetDirection = transform.localRotation.eulerAngles;

        // Set target direction for the character body to its inital state.
        if (characterBody)
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
    }

    void Update()
    {
        if (!frozen)
        {
            PerformRotation();
        }
        else
        {
            if (focusSubject != null)
            {
                Vector3 focusDirection = focusSubject.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(focusDirection, transform.up);
                Vector3 finalRotation = new Vector3(lookRotation.eulerAngles.x, lookRotation.eulerAngles.y, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(finalRotation), focusSpeed * Time.deltaTime);
            } 
            else
            {
                frozen = false;
            }
        }
    }

    internal void SetFrozen(bool freeze)
    {
        if (!IsFocusing)
            frozen = freeze;
    }

    /*
    public void FocusOn(Transform subject)
    {
        frozen = true;
        focusSubject = subject;
        player.Movement.SetFrozen(true);
    }

    public void StopFocusing()
    {
        focusSubject = null;
        SetFrozen(false);
        player.Movement.SetFrozen(false);
    }
    */

    private void PerformRotation()
    {
        // Allow the script to clamp based on a desired target value.
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // Get raw mouse input for a cleaner reading on more sensitive mice.
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), invertY ? -Input.GetAxisRaw("Mouse Y") : Input.GetAxisRaw("Mouse Y"));

        // Scale input against the sensitivity setting and multiply that against the smoothing value.
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

        // Interpolate mouse movement over time to apply smoothing delta.
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing);

        // Find the absolute mouse movement value from point zero.
        _mouseAbsolute += _smoothMouse;

        // Clamp and apply the local x value first, so as not to be affected by world transforms.
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        // Then clamp and apply the global y value.
        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        // If there's a character body that acts as a parent to the camera
        if (characterBody)
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
            player.MoveRotation(yRotation * targetCharacterOrientation);
        }
        else
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }
    }
}