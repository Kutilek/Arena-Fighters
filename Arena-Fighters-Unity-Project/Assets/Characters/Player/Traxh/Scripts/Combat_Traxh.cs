public class Combat_Traxh : Combat_Character
{
    public void CheckIfAttackign()
    {
        if (attacking)
        {
            animator.SetTrigger("exitSwordAttack");
            attacking = false;
        }
    }
}
