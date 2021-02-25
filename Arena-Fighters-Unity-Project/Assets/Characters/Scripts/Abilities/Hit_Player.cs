using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Player : Ability
{
    public override void Cast()
    {
        characterCombat.GetHit();
    }
}
