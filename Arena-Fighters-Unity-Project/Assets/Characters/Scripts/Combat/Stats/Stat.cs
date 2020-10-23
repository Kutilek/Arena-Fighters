using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    [SerializeField] protected float startingAmount = 1;
    protected float amount;

    protected virtual void Awake()
    {
        amount = startingAmount;
    }

    public float GetAmount()
    {
        return amount;
    }
}
