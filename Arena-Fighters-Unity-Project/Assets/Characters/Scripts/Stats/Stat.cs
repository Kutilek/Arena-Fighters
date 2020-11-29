using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    [SerializeField] protected float startingAmount;
    protected float amount;

    protected virtual void Awake()
    {
        if (startingAmount == 0f)
            startingAmount = 2f;
        amount = startingAmount;
    }

    public float GetAmount()
    {
        return amount;
    }
}
