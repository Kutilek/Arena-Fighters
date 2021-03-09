using UnityEngine;

public class Walk_Around_Player : Ability_AI
{
    protected float RandomX;
    protected float RandomZ;

    public override void Cast()
    {
        characterPhysics.LookAtPlayer();

        if (!casted)
        {
            if(RandomX == 0f || RandomZ == 0f)
            {
                RandomX = Random.Range(-1f, 1f);
                RandomZ = Random.Range(-1f, 1f);

                Vector3 direction = new Vector3(RandomX, 0f, RandomZ);

                direction -= characterPhysics.directionToPlayer;

                direction.y = 0f;
                
                characterPhysics.SetInputDirection(direction);
            }
            casted = true;
        }
    }

    protected override void DoStuffOnPeriodCall()
    {
        base.DoStuffOnPeriodCall();
        if (casted)
            characterPhysics.SetInputDirection(Vector3.zero);
        RandomX = 0f;
        RandomZ = 0f;
        casted = false;
    }
}
