using UnityEngine;

public class Stun_Target : Combat_Ability
{
    [SerializeField] private float stunLength;

    protected override void Awake()
    {
        base.Awake();
        if (stunLength == 0f)
            stunLength = 1f;
    }

    public override void Cast()
    {
        Health targetHealth;
        Character_Physics target = characterCombat.GetCombatCharacterComponents(abilityDistance, out targetHealth);
        if (target != null)
            StartCoroutine(characterCombat.SetCharacterMovementImpairingEffect(target, MovementImpairingEffect.Stun, stunLength)); 
    }
}
