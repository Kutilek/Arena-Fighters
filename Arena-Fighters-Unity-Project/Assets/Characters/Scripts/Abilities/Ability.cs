using UnityEngine;

[RequireComponent(typeof(Physics_Character))]
[RequireComponent(typeof(Animator))]
public abstract class Ability : MonoBehaviour
{
    protected Physics_Character characterPhysics;
    protected Combat_Character characterCombat;
    protected Animator animator;
    
    [SerializeField] protected string animationCondition;
    
    protected virtual void Start()
    {
        if(GetComponent<Combat_Character>() != null)
            characterCombat = GetComponent<Combat_Character>();

        characterPhysics = GetComponent<Physics_Character>();
        animator = GetComponent<Animator>();
    }

    public abstract void Cast();
}
