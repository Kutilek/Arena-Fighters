using UnityEngine;

public class Health_Enemy : Health
{
    protected Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected void Update()
    {
        animator.SetFloat("health", amount);
    }
}
