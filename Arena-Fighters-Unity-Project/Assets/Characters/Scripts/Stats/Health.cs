public class Health : Stat
{
    public void DecreaseHealth(float decrease)
    {
        amount -= decrease;
    }

    public float GetStartingAmount()
    {
        return startingAmount;
    }
}
