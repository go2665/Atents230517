using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Action onGoalIn;
    ParticleSystem[] goalInEffects;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        goalInEffects = child.GetComponentsInChildren<ParticleSystem>(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayGoalInEffects();
            onGoalIn?.Invoke();
        }
    }

    void PlayGoalInEffects()
    {
        if(goalInEffects != null)
        {
            foreach (ParticleSystem p in goalInEffects)
            {
                p.gameObject.SetActive(true);
                p.Play();
            }
            goalInEffects = null;
        }
    }
}
