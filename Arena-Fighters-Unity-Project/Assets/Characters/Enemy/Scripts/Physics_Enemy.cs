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

    float xdir;
    protected void SetAnimatorWalkX()
    {
        xdir = inputDirection.x;
        if (xdir > 0f)
            animator.SetFloat("walkX", 1f);
        else if (xdir < 0f)
            animator.SetFloat("walkX", -1f);
        else
            animator.SetFloat("walkX", 0f);
    }
}
