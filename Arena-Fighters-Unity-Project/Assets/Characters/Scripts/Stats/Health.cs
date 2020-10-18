using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Stat
{
    public void DecreaseHealth(float decrease)
    {
        amount -= decrease;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            amount -= 10f;
    }
}
