using UnityEngine;

public abstract class Player_Character_Controller : MonoBehaviour
{
    public virtual void WalkForward()
    {
        Debug.Log("I am walking forward!!!");
    }

    public virtual void WalkBackward()
    {
        Debug.Log("I am walking backward!!!");
    }

    public virtual void WalkLeft()
    {
        Debug.Log("I am walking left!!!");
    }

    public virtual void WalkRight()
    {
        Debug.Log("I am walking right!!!");
    }
}
