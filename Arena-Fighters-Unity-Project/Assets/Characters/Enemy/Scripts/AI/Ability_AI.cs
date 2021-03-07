using UnityEngine;

public abstract class Ability_AI : Ability
{
    public float currentChance;
    [SerializeField] protected float callPeriod;
    private float lastCall = 0f;
    protected new Physics_Enemy characterPhysics;
    protected new Combat_Enemy characterCombat;
    public bool casted;

    protected override void Start()
    {
        characterCombat = GetComponent<Combat_Enemy>();
        characterPhysics = GetComponent<Physics_Enemy>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Awake()
    {
        if (callPeriod == 0f)
            callPeriod = 10f;
    }

    protected virtual void Update()
    {
        CalculateChance();
    }

    public void CalculateChance()
    {
        if (lastCall + callPeriod <= Time.time)
        {
            DoStuffOnPeriodCall();
        }
    }

    protected virtual void DoStuffOnPeriodCall()
    {
        lastCall = Time.time;
        currentChance = Random.Range(1f, 100f);
    }
}
