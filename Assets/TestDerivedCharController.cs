using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDerivedCharController : BasePlayerController
{

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        base.BaseInit();
    }

    void Update()
    {
        base.CallUpdate();
    }
}
