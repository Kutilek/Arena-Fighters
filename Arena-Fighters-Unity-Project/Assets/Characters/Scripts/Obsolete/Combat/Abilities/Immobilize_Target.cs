using UnityEngine;

public class Immobilize_Target : Combat_Ability
{
    [SerializeField] private float immobilizeLength;

    protected override void Awake()
    {
        base.Awake();
        if (immobilizeLength == 0f)
            immobilizeLength = 1f;
    }

    public override void Cast()
    {
        Health targetHealth;
        Character_Physics target = characterCombat.GetCombatCharacterComponents(abilityDistance, out targetHealth);
        if (target != null)
            StartCoroutine(characterCombat.SetCharacterMovementImpairingEffect(target, MovementImpairingEffect.Immobilization, immobilizeLength)); 
    }
}
