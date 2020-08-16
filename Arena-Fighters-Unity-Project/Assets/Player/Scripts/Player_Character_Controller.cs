using UnityEngine;

public abstract class Player_Character_Controller : MonoBehaviour
{
    public bool shiftHelperKey;
    public bool walkingForward;
    public bool walkingBackward;
    public bool walkingLeft;
    public bool walkingRight;

    public virtual void Update()
    {
        if (shiftHelperKey)
        {
            if (walkingForward)
            {
                WalkFastForward();
            }
        }
        else
        {
            if (walkingForward)
            {
                WalkForward();
            }
            else if (walkingBackward)
            {
                WalkBackward();
            }
            else if (walkingLeft)
            {
                WalkLeft();
            }
            else if (walkingRight)
            {
                WalkRight();
            }
        }
    }

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

    public virtual void WalkFastForward()
    {
        Debug.Log("I am walking fast forward!!!");
    }
}
