using UnityEngine;

public class Player_Combat : Character_Combat
{
    public override Character_Physics GetCombatCharacterComponents(float distance, out Health health)
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, distance, LayerMask.GetMask("Enemy"));
        foreach (Collider col in collider)
        {
            if (col.GetComponent<Character_Combat>() != null)
            {
                transform.LookAt(col.transform);
                transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
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
