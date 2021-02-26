using UnityEngine;

public class Jump : Ability
{
    [SerializeField] private float jumpForce;

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
