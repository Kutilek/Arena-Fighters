using UnityEngine;

public class Combat_Enemy : Combat_Character
{
    public override void GetHit()
    {
        animator.SetTrigger("gotHit");
        getHitEffect.Play();
        ableToCast = false;
    }
}
