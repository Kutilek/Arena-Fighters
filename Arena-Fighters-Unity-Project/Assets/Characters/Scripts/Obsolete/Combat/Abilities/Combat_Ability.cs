using UnityEngine;

[RequireComponent(typeof(Character_Combat))]
public abstract class Combat_Ability : MonoBehaviour
{
    [SerializeField] protected float abilityDistance;
    protected Character_Combat characterCombat;
    protected Damage damage;

    protected virtual void Awake()
    {
        if (abilityDistance == 0f)
            abilityDistance = 3f;
        characterCombat = GetComponent<Character_Combat>();
        damage = GetComponent<Damage>();
    }

    public abstract void Cast();
}
