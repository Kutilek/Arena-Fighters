using UnityEngine;

public class Light_Attack : Combat_Ability
{
    [SerializeField] private float knocbackForce;

    protected override void Awake()
    {
        base.Awake();
        if (knocbackForce == 0f)
            knocbackForce = 10f;
    }

    public override void Cast()
    {
        Health targetHealth;
        Character_Physics target = characterCombat.GetCombatCharacterComponents(abilityDistance, out targetHealth);

        if (target != null)
        {
            StartCoroutine(characterCombat.KnocbackCharacter(target, transform.forward, knocbackForce, 0.3f));
            targetHealth.DecreaseHealth(damage.GetAmount());
        }
    }
}
