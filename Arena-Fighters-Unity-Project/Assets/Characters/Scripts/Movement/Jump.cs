using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character_Physics))]
public class Jump : MonoBehaviour
{
    private Character_Physics characterPhysics;
    [SerializeField] private float jumpForce;

    public void CastJump()
    {
        if (characterPhysics.currentGravityState == GravityState.Grounded)
            StartCoroutine(CalculateJump());
        else if (characterPhysics.currentGravityState == GravityState.FallingOffWall)
            characterPhysics.AddForce(Vector3.down, jumpForce); /* Push Character Down If jumped of wall */
    }

    private void Awake()
    {
        if (jumpForce == 0)
            jumpForce = 35f;

        characterPhysics = GetComponent<Character_Physics>();
    }

    private IEnumerator CalculateJump()
    {
        characterPhysics.AddForce(Vector3.up, jumpForce);
        characterPhysics.currentGravityState = GravityState.Falling;

        // Needed to pause Ground Checking because then player never got off the ground
        StartCoroutine(characterPhysics.PauseGroundCheck());

        yield return new WaitUntil(() => characterPhysics.currentGravityState == GravityState.Grounded);

        characterPhysics.ResetImpactY();
    }
}
