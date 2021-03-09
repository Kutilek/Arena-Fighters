using UnityEngine;

public class Punch : Ability_AI
{
    private Damage damage;
    [SerializeField] private float knockbackForce;

    public bool punching;

    private void SetPunchingTrue()
    {
        characterPhysics.LookAtPlayer();
        characterPhysics.AddForce(transform.rotation * Vector3.forward, characterPhysics.distanceToPlayer * 16f);
        punching = true;
    }

    private void SetPunchingFalse()
    {
        characterPhysics.AddForce(transform.rotation * Vector3.back, 50f);
        punching = false;
    }

    protected override void Awake()
    {
        base.Awake();
        damage = GetComponent<Damage>();
    }

    protected override void DoStuffOnPeriodCall()
    {
        lastCall = Time.time;
        currentChance = Random.Range(1f, 100f);
        casted = false;
    }

    public void CheckHit(Collider hit)
    {
        if (punching)
        {
            float dealDamage = damage.GetAmount() * Random.Range(0.2f, 1.1f);
            if (hit.tag == "Player")
            {
                Physics_Character phyChar = hit.GetComponent<Physics_Character>();
                phyChar.AddOutsideForce(transform.forward, knockbackForce);
                hit.transform.GetComponent<Combat_Character>().GetHit();
                hit.transform.GetComponent<Health>().DecreaseHealth(dealDamage);
            }
        }
    }
    
    public override void Cast()
    {
        if (!casted)
        {
            animator.SetTrigger("punch");  
            casted = true;
        }
    }
}
