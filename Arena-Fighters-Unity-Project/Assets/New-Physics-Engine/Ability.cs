using UnityEngine;

[RequireComponent(typeof(Physics_Character))]
public abstract class Ability : MonoBehaviour
{
    protected Physics_Character characterPhysics;
    
    protected virtual void Start()
    {
        characterPhysics = GetComponent<Physics_Character>();
    }

    public abstract void Cast();
}
