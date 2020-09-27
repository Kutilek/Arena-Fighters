using System;
using UnityEngine;

[Serializable]
public struct EnemyCommand
{
    public bool isAble;
    public float cooldown;
    public float duration;
    public float randomChance;
    private float timer;  

    public EnemyCommand(float cooldown, float duration, float randomChance)
    {
        this.cooldown = cooldown;
        this.duration = duration;
        this.randomChance = randomChance;
        timer = 0f;
        isAble = false;
    }

    public void CalculateChance()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {  
            float random = UnityEngine.Random.Range(0f, 100f);
            
            if (random <= randomChance)
                isAble =  true;
            timer = 0f;
        }
    }
}