using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BasePlayerController : MonoBehaviour
{
    [SerializeField] MiniGameStats currentMinigameStats;
    [SerializeField] PlayerMovementPresets movementPreset;
    public float movementSpeed;
    [SerializeField] Interactable raycastInteractable;
    [SerializeField] Interactable physicsInteractable;
    [SerializeField] RaycastInformation raycastInformation;
    [SerializeField] Movement movementInformation;
    [SerializeField] PhysicsEvents physicsEvents;


    public void BaseInit()
    {
        movementInformation.perspective = movementPreset.PresetPerspective;
        movementInformation.events.onPerspectiveChange.AddListener(delegate { SetMovementPreset(movementInformation.perspective); });
    }

    public void AttemptInteraction()
    {
        //Checks if the game is paused before running
        if (GlobalHelper.IsPaused() != true)
        {
            //If the player is looking at an object with an interactable, do this.
            if (raycastInteractable != null)
            {
                raycastInteractable.Interact();
            }

            else if(physicsInteractable != null)
            {
                physicsInteractable.Interact();
            }
        }
    }

    void Movement()
    {
        Vector3 input = ConvertMovementDirection(movementInformation.currentMovementVector.x, movementInformation.currentMovementVector.y);
        movementInformation.rb.MovePosition(transform.position + input * Time.deltaTime * movementSpeed);
    }

    void Raycast()
    {
        //Checks if the game is paused before running to save performance
        if (GlobalHelper.IsPaused() != true && raycastInformation.raycastPoint != null)
        {
            // The raycast
            RaycastHit hit;
            Debug.DrawRay(raycastInformation.raycastPoint.position, raycastInformation.raycastPoint.forward * raycastInformation.raycastDistance, raycastInformation.debugColour, Time.deltaTime);

            //Checks a raycast for scripts with the If statement
            if (Physics.Raycast(raycastInformation.raycastPoint.position, raycastInformation.raycastPoint.forward, out hit, raycastInformation.raycastDistance))
            {
                //If the raycast hits an object with a dialogue trigger
                if (hit.collider.TryGetComponent(out Interactable interactableObject))
                {
                    raycastInteractable = interactableObject;
                }

                //If the ray cast isn't hitting an object with a dialogue trigger, the variable is null
                else
                {
                    raycastInteractable = null;
                }
            }

            //If raycast hits nothing
            else
            {
                raycastInteractable = null;
            }
        }
    }

    public void CallUpdate()
    {
        Raycast();
        movementInformation.CheckEvents();
    }

    public void CallFixedUpdate()
    {
        Movement();
    }

    internal Interactable GetRaycastInteractable()
    {
        return raycastInteractable;
    }

    internal Interactable GetPhysicsInteractable()
    {
        return physicsInteractable;
    }

    public void LoadUpMiniGameStats(MiniGameStats stats)
    {
        currentMinigameStats = stats;
    }

    public void UpdateMovementInput(InputAction.CallbackContext context)
    {
        movementInformation.currentMovementVector = context.ReadValue<Vector2>();
    }

    Vector3 ConvertMovementDirection(float xValue,  float yValue, Transform swag = null)
    {
        float x = 0;
        float y = 0;
        float z = 0;
        Vector3 newVector = new Vector3(x, y, z);
        switch (movementPreset.GetHorizontalAxis())
        {
            case PlayerMovementPresets.MoveAxis.x:
                x = xValue;
                break;
            case PlayerMovementPresets.MoveAxis.y:
                y = xValue;
                break;
            case PlayerMovementPresets.MoveAxis.z:
                z = xValue;
                break;
            case PlayerMovementPresets.MoveAxis.cameraRelativeHorizontal:
                newVector = new Vector3();
               

                break;
            case PlayerMovementPresets.MoveAxis.cameraRelativeForward:
                break;
            case PlayerMovementPresets.MoveAxis.nothing:

                break;
            default:
                break;
        }

        switch (movementPreset.GetVerticalAxis())
        {
            case PlayerMovementPresets.MoveAxis.x:
                x = yValue;
                break;
            case PlayerMovementPresets.MoveAxis.y:
                y = yValue;
                break;
            case PlayerMovementPresets.MoveAxis.z:
                z = yValue;
                break;
            case PlayerMovementPresets.MoveAxis.cameraRelativeHorizontal:

                break;
            case PlayerMovementPresets.MoveAxis.cameraRelativeForward:
                break;
            case PlayerMovementPresets.MoveAxis.nothing:
                break;
            default:
                break;
        }
        return new Vector3(x,y,z);
    }


    void AdjustRotation()
    {

    }
    public void SetMovementPreset(GlobalHelper.Perspective perspective)
    {
        switch (perspective)
        {
            case GlobalHelper.Perspective.firstPerson:
                movementPreset = MovementPresetHolder.instance.firstPerson;
                break;
            case GlobalHelper.Perspective.thirdPerson:
                movementPreset = MovementPresetHolder.instance.thirdPerson;
                break;
            case GlobalHelper.Perspective.sideScroller:
                movementPreset = MovementPresetHolder.instance.sideScroller;
                break;
            case GlobalHelper.Perspective.topDown:
                movementPreset = MovementPresetHolder.instance.topdown;
                break;
            default:
                break;
        }
    }

    public void ChangeMoveVectors()
    {

    }

    //PHYSICS EVENTS
    private void OnCollisionEnter(Collision collision)
    {
        physicsEvents.defaultEvents.onColliderEnter.Invoke();

        //Triggers Event + sets physics interactable
        if(collision.transform.TryGetComponent(out Interactable interactable))
        {
            physicsInteractable = interactable;
            physicsEvents.customEvents.onInteractableHit.Invoke();
        }

        //Sets physics interactable to null
        else
        {
            physicsInteractable = null;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        physicsEvents.defaultEvents.onColliderStay.Invoke();

        //Triggers Event + sets physics interactable
        if (collision.transform.TryGetComponent(out Interactable interactable))
        {
            physicsInteractable = interactable;
            physicsEvents.customEvents.onInteractableStay.Invoke();
        }

        //Sets physics interactable to null
        else
        {
            physicsInteractable = null;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        physicsEvents.defaultEvents.onColliderExit.Invoke();

        //Triggers Event + sets physics interactable
        if (collision.transform.TryGetComponent(out Interactable interactable))
        {
            physicsInteractable = interactable;
            physicsEvents.customEvents.onInteractableExit.Invoke();
        }

        //Sets physics interactable to null
        else
        {
            physicsInteractable = null;
        }
    }
}

[System.Serializable]
public class RaycastInformation
{
    [Header("Variables")]
    public Transform raycastPoint;
    [Range(0,100)]
    public float raycastDistance = 25;

    [Header("Debug")]
    public Color debugColour;

    
}

[System.Serializable]
public class Movement
{
    [Header("Variables")]
    public GlobalHelper.Perspective perspective;
    GlobalHelper.Perspective prevPerspective;

    [Space(10)]

    public Vector2 currentMovementVector;
    public Rigidbody rb;

    [Space(10)]
    public MovementEvents events;

    public void CheckEvents()
    {
        if(perspective != prevPerspective)
        {
            events.onPerspectiveChange.Invoke();
            prevPerspective = perspective;
            switch (perspective)
            {
                case GlobalHelper.Perspective.firstPerson:
                    events.onFirstPerson.Invoke();
                    break;
                case GlobalHelper.Perspective.thirdPerson:
                    events.onThirdPerson.Invoke();
                    break;
                case GlobalHelper.Perspective.sideScroller:
                    events.onSideScroller.Invoke();
                    break;
                case GlobalHelper.Perspective.topDown:
                    events.onTopDown.Invoke();
                    break;
                default:
                    break;
            }
        }
    }

  

}

[System.Serializable]
public class MovementEvents
{
    public UnityEvent onPerspectiveChange;
    public UnityEvent onFirstPerson;
    public UnityEvent onThirdPerson;
    public UnityEvent onSideScroller;
    public UnityEvent onTopDown;
}


[System.Serializable]
public class PhysicsEvents
{
    public CustomInteractions customEvents;
    public UnityInteractions defaultEvents;
}

[System.Serializable]
public class UnityInteractions
{
    public UnityEvent onColliderEnter;
    public UnityEvent onColliderExit;
    public UnityEvent onColliderStay;
}

[System.Serializable]
public class CustomInteractions
{
    public UnityEvent onInteractableHit;
    public UnityEvent onInteractableExit;
    public UnityEvent onInteractableStay;
}


