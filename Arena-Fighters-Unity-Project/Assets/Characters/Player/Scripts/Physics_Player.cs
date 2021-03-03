using UnityEngine;

public class Physics_Player : Physics_Character
{
    public Vector3 inputDirectionRaw;
    private Transform cam;
    public ParticleSystem groundImpact;
    
    protected override void Awake()
    {
        base.Awake();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    protected override void Update()
    {
        base.Update();
        inputDirection = CalculateDirectionOnGround();
    }

    private Vector3 CalculateDirectionOnGround()
    {  
        if (inputDirectionRaw.magnitude >= 0.2f)
        {
            float targetAngle = Mathf.Atan2(inputDirectionRaw.x, inputDirectionRaw.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            return movementDirection;
        }
        return Vector3.zero;
    }

    private void PlayGroundImpactEffect()
    {     
        Instantiate(groundImpact, transform.Find("Ground_Impact_Position").position, transform.Find("Ground_Impact_Position").rotation);
    }
}
