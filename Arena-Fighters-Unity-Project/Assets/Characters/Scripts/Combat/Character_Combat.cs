using System.Collections;
using UnityEngine;

public class Character_Combat : MonoBehaviour
{
    [SerializeField] protected float attackDistance;

    protected virtual void Awake()
    {
        
    }

    public void Test()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackDistance))
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pressAttackCommand.Equals(actionTwoStartCommand))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
            {
                StartCoroutine(MovementImpairEnemy(hit, MovementImpairingEffects.Stun, 3f));
            }
        }

        if (pressAttackCommand.Equals(lightAttackCommand))
        {
           // StartCoroutine(KnocbackMe());

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                if (hit.collider.tag == "Enemy")
                {
                    Debug.Log("HIT");
                    hit.collider.GetComponent<Enemy_Controller>().Knocback(transform.forward);
                }
            }
        }
    }

    public void KnocbackTarget()
    {

    }

    public void Knocback(Vector3 direction)
    {
        StartCoroutine(GetKnocback(direction));
    }

    public IEnumerator GetKnocback(Vector3 direction)
    {
        AddOutsideImpact(direction, 20f);

        yield return new WaitForSeconds(0.4f);

        ResetOutsideImpact();
    }

    protected IEnumerator MovementImpairEnemy(RaycastHit hit, MovementImpairingEffects effect, float duration)
    {
        if (hit.collider.tag == "Enemy")
        {
            hit.collider.GetComponent<Enemy_Controller>().currentMovementImpairingEffect = effect;
            Debug.Log("Impaired!!!");
        }

        yield return new WaitForSeconds(duration);

        hit.collider.GetComponent<Enemy_Controller>().currentMovementImpairingEffect = MovementImpairingEffects.None;
    }

    
}
