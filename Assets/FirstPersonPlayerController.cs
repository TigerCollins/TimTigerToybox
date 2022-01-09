using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerController : BasePlayerController
{

    void Update()
    {
        base.CallUpdate();
    }

    private void FixedUpdate()
    {
        base.CallFixedUpdate();
    }
}
