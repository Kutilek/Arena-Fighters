using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch_Check_Hit : MonoBehaviour
{
    protected Punch punch;
    
    protected void Awake()
    {
        punch = GetComponentInParent<Punch>();
    }

    public void OnTriggerEnter(Collider hit)
    {
        punch.CheckHit(hit);
    }
}
