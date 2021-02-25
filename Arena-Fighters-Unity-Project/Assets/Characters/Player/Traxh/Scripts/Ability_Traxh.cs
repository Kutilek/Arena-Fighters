using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability_Traxh : Ability
{
    protected new Combat_Traxh characterCombat;
    
    protected override void Start()
    {
        base.Start();
        characterCombat = GetComponent<Combat_Traxh>();
    }
}
