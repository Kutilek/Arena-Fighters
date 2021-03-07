using System.Collections;
using UnityEngine;

public class Physics_Enemy : Physics_Character
{
    protected float RandomX;
    protected float RandomZ;
    protected bool moving;

    public void SetInputDirection(Vector3 direction)
    {
        inputDirection = direction;
    }

    protected override void Update()
    {
        base.Update();
        SetAnimatorWalkX();
    }

    protected void SetAnimatorWalkX()
    {
        float xdir = inputDirection.x;
        if (xdir >= 0)
            animator.SetFloat("walkX", 1f);
        else
            animator.SetFloat("walkX", -1f);
    }
}
