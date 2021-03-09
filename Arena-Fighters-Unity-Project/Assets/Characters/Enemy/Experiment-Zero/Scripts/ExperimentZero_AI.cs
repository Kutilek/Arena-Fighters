using UnityEngine;

public class ExperimentZero_AI : MonoBehaviour
{
    protected Physics_Enemy characterPhysics;
    protected Combat_Enemy characterCombat;

    // Abilities
    private Walk_Random walkRandom;
    private Walk_Around_Player walkAroundPlayer;
    private Walk_To_Player walkToPlayer;
    private Punch punch;

    public bool lockedOnPlayer;
    
    private void Awake()
    {
        characterCombat = GetComponent<Combat_Enemy>();
        characterPhysics = GetComponent<Physics_Enemy>();

        walkRandom = GetComponent<Walk_Random>();
        walkAroundPlayer = GetComponent<Walk_Around_Player>();
        walkToPlayer = GetComponent<Walk_To_Player>();
        punch = GetComponent<Punch>();
    }

    void Update()
    {
        if (!lockedOnPlayer)
        {
            if (!walkRandom.casted && walkToPlayer.currentChance >= 50f)
            {   
                walkToPlayer.Cast();
                lockedOnPlayer = true;
            }
            else if (walkRandom.currentChance >= 80f)
                walkRandom.Cast();
        }
        else
        {
            if (!walkAroundPlayer.casted && punch.currentChance >= 30f)
                punch.Cast();
            else if (!punch.punching && characterPhysics.distanceToPlayer <= 20f)
            {
                if (walkAroundPlayer.currentChance >= 50f)
                    walkAroundPlayer.Cast();
                else if (walkRandom.currentChance >= 98f)
                    UnlockFromPlayer();
            }
            else if (characterPhysics.distanceToPlayer > 27f)
                UnlockFromPlayer();
        }   
    }

    private void UnlockFromPlayer()
    {
        lockedOnPlayer = false;
        characterCombat.LeaveAttackingState();
        walkRandom.Cast();
    }
}
