using UnityEngine;

public class ExperimentZero_AI : MonoBehaviour
{
    // Abilities
    private Walk_Random walkRandom;

    private void Awake()
    {
        walkRandom = GetComponent<Walk_Random>();
    }

    void Update()
    {
        if (walkRandom.currentChance >= 10f)
            walkRandom.Cast();
    }
}
