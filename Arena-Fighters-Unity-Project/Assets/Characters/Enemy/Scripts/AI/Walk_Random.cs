using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk_Random : Ability_AI
{
    protected float RandomX;
    protected float RandomZ;
    protected Transform wall;
    
    protected override void Awake()
    {
        base.Awake();
        wall = GameObject.FindGameObjectWithTag("Wall").transform;
    }

    public override void Cast()
    {
        if (!casted)
        {
            if(RandomX == 0f || RandomZ == 0f)
            {
                RandomX = Random.Range(-1f, 1f);
                RandomZ = Random.Range(-1f, 1f);

                Vector3 direction = new Vector3(RandomX, 0f, RandomZ);

                if (Vector3.Distance(transform.position, wall.position) >= 30f)
                    direction -= (transform.position - wall.position).normalized;

                direction.y = 0f;
                
                characterPhysics.SetInputDirection(direction);
            }
            casted = true;
        }
    }

    protected override void DoStuffOnPeriodCall()
    {
        base.DoStuffOnPeriodCall();
        RandomX = 0f;
        RandomZ = 0f;
        casted = false;
        characterPhysics.SetInputDirection(Vector3.zero);
    }
}
