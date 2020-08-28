using UnityEngine;

public class Basic_Character_Behaviour : Character_Behaviour, IPlayerMovement
{ 
    public override void Walk(Vector3 direction)
    {
        Move(direction, walkSpeed);
    }

    public override void Run(Vector3 direction)
    {
        Move(direction, runSpeed);
    }
}
