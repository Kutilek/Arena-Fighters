using UnityEngine;

public class Jump : Ability
{
    [SerializeField] private float jumpForce;
    protected new Combat_Traxh characterCombat;

    protected void Awake()
    {
        characterCombat = GetComponent<Combat_Traxh>();
    }

    public override void Cast()
    {
        if (characterCombat.ableToCast)
        {
            characterCombat.CheckIfAttackign();
            characterCombat.ableToCast = false;
            animator.SetBool("grounded", false);
            animator.SetTrigger(animationCondition);
            characterCombat.attacking = false;
        }
    }

    // Called from Animation Event
    private void JumpAbility()
    {
        characterPhysics.AddForce(Vector3.up, jumpForce);
    }
}
