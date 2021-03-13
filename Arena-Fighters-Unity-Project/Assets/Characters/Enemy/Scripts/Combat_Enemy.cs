using UnityEngine;

public class Combat_Enemy : Combat_Character
{
    protected ParticleSystem[] attackingEffects;
    protected Score_Counter scoreCounter;

    protected override void Awake()
    {
        base.Awake();
        attackingEffects = transform.Find("Armature").GetComponentsInChildren<ParticleSystem>();
        scoreCounter = GameObject.FindGameObjectWithTag("ScoreCounter").GetComponent<Score_Counter>();
    }

    public void StartAttackingState()
    {
        animator.SetBool("attacking", true);
        foreach (var effect in attackingEffects)
        {
            if (effect.isStopped)
                effect.Play(false);
        }
    }

    public void LeaveAttackingState()
    {
        animator.SetBool("attacking", false);
        foreach (var effect in attackingEffects)
        {
            if (!effect.isStopped)
                effect.Stop();
        }
    }

    protected virtual void OnDeath()
    {
        scoreCounter.score += 3 + Random.Range(-2, 5);
        Destroy(gameObject);
    }
}
