using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStandard : TurretBase
{
    private void Start()
    {
        StartCoroutine(fireCoroutine);
    }
}
