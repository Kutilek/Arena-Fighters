using System.Collections;
using UnityEngine;

public class Physics_Enemy : Physics_Character
{
    protected float RandomX;
    protected float RandomZ;
    protected bool moving;
    protected Transform player;
    public Vector3 directionToPlayer;
    public float distanceToPlayer;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SetInputDirection(Vector3 direction)
    {
        inputDirection = direction;
    }

    protected override void Update()
    {
        base.Update();
        SetAnimatorWalkX();
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        directionToPlayer = (transform.position - player.transform.position).normalized;
    }

    float xdir;
    protected void SetAnimatorWalkX()
    {
        xdir = inputDirection.x;
        if (xdir > 0f)
            animator.SetFloat("walkX", -1f);
        else if (xdir < 0f)
            animator.SetFloat("walkX", 1f);
        else
            animator.SetFloat("walkX", 0f);
    }

    public void LookAtPlayer()
    {
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }
}
