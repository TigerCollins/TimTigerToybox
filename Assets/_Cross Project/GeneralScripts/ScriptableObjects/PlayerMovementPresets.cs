using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Movement Info", menuName = "Toybox/Player/Movement", order = 1)]
public class PlayerMovementPresets : ScriptableObject
{
    public enum MoveAxis
    {
        x,
        y,
        z,
        cameraRelativeHorizontal,
        cameraRelativeForward,
        nothing
    }

    [SerializeField]
    MoveAxis horizontalAxis;
    [SerializeField]
    MoveAxis verticalAxis;

    public MoveAxis GetHorizontalAxis()
    {
        return horizontalAxis;
    }
    public MoveAxis GetVerticalAxis()
    {
        return verticalAxis;
    }
}
