using UnityEngine;

[RequireComponent(typeof(Character_Combat))]
public abstract class Combat_Ability : MonoBehaviour
{
    protected Character_Combat characterCombat;
    protected Damage damage;

    protected virtual void Awake()
    {
        characterCombat = GetComponent<Character_Combat>();
        damage = GetComponent<Damage>();
    }

    public abstract void Cast();
}
