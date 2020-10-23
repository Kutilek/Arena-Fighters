using UnityEngine;

public class Light_Attack : Combat_Ability
{
    public override void Cast()
    {
        Health targetHealth;
        Character_Physics target = characterCombat.GetCombatCharacterComponents(3f, out targetHealth);

        if (target != null)
        {
            StartCoroutine(characterCombat.KnocbackCharacter(target, transform.forward, 25f, 0.3f));
            targetHealth.DecreaseHealth(damage.GetAmount());
        }
    }
}
