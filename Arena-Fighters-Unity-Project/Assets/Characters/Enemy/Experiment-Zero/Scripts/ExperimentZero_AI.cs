using UnityEngine;

public class ExperimentZero_AI : MonoBehaviour
{
    // Abilities
    private Walk_Random walkRandom;
    private Walk_Around_Player walkAroundPlayer;
    private Walk_To_Player walkToPlayer;


    private void Awake()
    {
        walkRandom = GetComponent<Walk_Random>();
        walkAroundPlayer = GetComponent<Walk_Around_Player>();
        walkToPlayer = GetComponent<Walk_To_Player>();
    }

    void Update()
    {
      /*  if (walkRandom.currentChance >= 10f)
            walkRandom.Cast();*/

        if (walkToPlayer.currentChance >= 90f)
            walkToPlayer.Cast();
    }
}
