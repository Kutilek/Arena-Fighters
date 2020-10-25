﻿using UnityEngine;

public class Player_Combat : Character_Combat
{
    public override Character_Physics GetCombatCharacterComponents(float distance, out Health health)
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, distance, LayerMask.GetMask("Enemy"));
        foreach (Collider col in collider)
        {
            if (col.GetComponent<Character_Combat>() != null)
            {
                Debug.Log(col);
                transform.LookAt(col.transform);
                Health targetHealth;
                Character_Physics target = base.GetCombatCharacterComponents(distance, out targetHealth);
                health = targetHealth;
                return target;
            }
        }
        health = null;
        return null;
    }
}
