using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEffects : MonoBehaviour
{
    [SerializeField] Animator animator;
    private void Awake()
    {
    }

    internal void OnLand()
    {
        animator.SetTrigger("Land");
    }
}
