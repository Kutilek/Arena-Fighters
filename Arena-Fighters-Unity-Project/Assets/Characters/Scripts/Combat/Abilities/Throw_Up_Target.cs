using UnityEngine;

public class Throw_Up_Target : Combat_Ability
{
    private Character_Physics characterPhysics;

    public override void Cast()
    {
        Health targetHealth;
        Character_Physics target = characterCombat.GetCombatCharacterComponents(abilityDistance, out targetHealth);
        if (target != null)
        {
            StartCoroutine(characterPhysics.PauseGroundCheck());
            StartCoroutine(target.PauseGroundCheck());
            target.SetMovementImpairingEffect(MovementImpairingEffect.Immobilization);
            target.currentGravityState = GravityState.InAir;
            characterPhysics.currentGravityState = GravityState.InAir;
            
            characterPhysics.AddForce(Vector3.up, 80f);
            target.AddOutsideForce(Vector3.up, 80f);
            targetHealth.DecreaseHealth(damage.GetAmount() * 2f);
        }   
    }

    protected override void Awake()
    {
        base.Awake();
        characterPhysics = GetComponent<Character_Physics>();
    }
}
