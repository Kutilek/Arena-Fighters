using UnityEngine;

public class Basic_Character_Behaviour : Character_Behaviour, IPlayerMovement
{ 
    public override void WalkForward()
    {
        Move(Vector3.forward, walkSpeed, cam);
    }

    public override void WalkLeft()
    {
        Move(Vector3.left, walkSpeed, cam);
    }

    public override void WalkBack()
    {
        Move(Vector3.back, walkSpeed, cam);
    }

    public override void WalkRight()
    {
        Move(Vector3.right, walkSpeed, cam);
    }
}
