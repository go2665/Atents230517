using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    Transform fillPivot;

    private void Awake()
    {
        fillPivot = transform.GetChild(1);

        IHealth target = GetComponentInParent<IHealth>();
        target.onHealthChange += Refresh;
    }

    private void Refresh(float ratio)
    {
        fillPivot.localScale = new Vector3(ratio, 1, 1);
    }
}
