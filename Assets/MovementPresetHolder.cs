using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPresetHolder : MonoBehaviour
{
    public static MovementPresetHolder instance;

    public PlayerMovementPresets firstPerson;
    public PlayerMovementPresets thirdPerson;
    public PlayerMovementPresets sideScroller;
    public PlayerMovementPresets topdown;


    private void Awake()
    {
        instance = this;
    }
}
