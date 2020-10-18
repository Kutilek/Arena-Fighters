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
        else if (characterPhysics.currentGravityState == GravityState.JumpedOffWall)
            characterPhysics.AddForce(Vector3.down, jumpForce);
    }

    private void Awake()
    {
        characterPhysics = GetComponent<Character_Physics>();
    }

    private IEnumerator CalculateJump()
    {
        characterPhysics.AddForce(Vector3.up, jumpForce);   

        characterPhysics.currentGravityState = GravityState.Falling;
        StartCoroutine(characterPhysics.PauseGroundCheck());

        yield return new WaitUntil(() => characterPhysics.currentGravityState == GravityState.Grounded);

        characterPhysics.ResetImpactY();
    }
}
