﻿using System.Collections;
using UnityEngine;
public class Jumpido : Ability
{
    [SerializeField] private float jumpForce;
    
    public override void Cast()
    {
        StartCoroutine(SetJumped());
        characterPhysics.AddForce(Vector3.up, jumpForce);
        StartCoroutine(characterPhysics.PauseGroundCheck());
        characterPhysics.currentGravityState = GravityState.Falling;
    }

    private IEnumerator SetJumped()
    {
        animator.SetBool(animationCondition, true);

        yield return new WaitForSeconds(0.1f);

        animator.SetBool(animationCondition, false);
    }
}
