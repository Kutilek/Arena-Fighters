using System.Collections;
using UnityEngine;

public class Jump : Ability
{
    [SerializeField] private float jumpForce;
    
    public override void Cast()
    {
        if (characterPhysics.currentGravityState == GravityState.Grounded)
        {
            Debug.Log("I am still Grounded");
            StartCoroutine(characterPhysics.PauseCheckForGround(1f));
            characterPhysics.currentGravityState = GravityState.Falling;
            animator.SetBool("grounded", false);
            animator.SetTrigger(animationCondition);
            StartCoroutine(Ahoj());
            StartCoroutine(Blabla());
        } 
    }

    private IEnumerator Blabla()
    {
        yield return new WaitUntil(() => characterPhysics.currentGravityState == GravityState.Grounded);

        StartCoroutine(characterPhysics.SetMovementImpairingEffect(MovementImpairingEffect.Immobilization, animator.GetCurrentAnimatorStateInfo(0).length));
    }

    private IEnumerator Tada()
    {
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(WaitForAnimationToEnd(animator.GetCurrentAnimatorStateInfo(0).length - 0.2f));
    }

    private IEnumerator WaitForAnimationToEnd(float length)
    {
        yield return new WaitForSeconds(length);

        characterPhysics.AddForce(Vector3.up, jumpForce);
    }

    private IEnumerator Ahoj()
    {
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(WaitForAnimationToEnd(animator.GetCurrentAnimatorStateInfo(0).length - 0.2f));
    }

    private IEnumerator SetJumped()
    {
        animator.SetTrigger(animationCondition);

        yield return new WaitForSeconds(0.1f);

        animator.SetTrigger(animationCondition);
    }
}
