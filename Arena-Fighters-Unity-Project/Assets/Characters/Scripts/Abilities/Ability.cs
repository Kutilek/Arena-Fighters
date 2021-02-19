using UnityEngine;

[RequireComponent(typeof(Physics_Character))]
[RequireComponent(typeof(Animator))]
public abstract class Ability : MonoBehaviour
{
    protected Physics_Character characterPhysics;
    protected Animator animator;
    
    [SerializeField] protected string animationCondition;
    
    protected virtual void Start()
    {
        characterPhysics = GetComponent<Physics_Character>();
        animator = GetComponent<Animator>();
    }

    public abstract void Cast();
}
