using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character_Physics))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
public abstract class Character_Combat : MonoBehaviour
{
    public virtual Character_Physics GetCombatCharacterComponents(float distance, out Health health)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
        {
            if (hit.collider.GetComponent<Character_Combat>() != null)
            {
                health = hit.collider.GetComponent<Health>();
                return hit.collider.GetComponent<Character_Physics>();
            }   
        }
        health = null;
        return null;
    }

    public IEnumerator KnocbackCharacter(Character_Physics character, Vector3 direction, float knocbackForce, float knocbackLength)
    {
        character.AddOutsideForce(direction, knocbackForce);

        yield return new WaitForSeconds(knocbackLength);

        character.ResetOutsideImpact();
    }

    public IEnumerator SetCharacterMovementImpairingEffect(Character_Physics character, MovementImpairingEffect effect, float length)
    {
        character.SetMovementImpairingEffect(effect);

        yield return new WaitForSeconds(length);

        character.SetMovementImpairingEffect(MovementImpairingEffect.None);
    }
}
