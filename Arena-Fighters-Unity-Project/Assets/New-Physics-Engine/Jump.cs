using System.Collections;
using UnityEngine;
public class Jump : Ability
{
    [SerializeField] private float jumpForce;
    
    public override void Cast()
    {
        StartCoroutine(SetJumped());
        characterPhysics.AddForce(Vector3.up, jumpForce);
    }

    private IEnumerator SetJumped()
    {
        animator.SetBool(animationCondition, true);

        yield return new WaitForSeconds(0.1f);

        animator.SetBool(animationCondition, false);
    }
}
