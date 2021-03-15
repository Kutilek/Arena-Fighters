using UnityEngine;

public class Sword_Check_For_Hit : MonoBehaviour
{
    protected Sword_Slash swordSlash;
    
    protected void Awake()
    {
        swordSlash = GetComponentInParent<Sword_Slash>();
    }

    public void OnTriggerEnter(Collider hit)
    {
        swordSlash.CheckHit(hit);
    }
}
