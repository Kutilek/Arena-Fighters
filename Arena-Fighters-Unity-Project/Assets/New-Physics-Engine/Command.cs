using UnityEngine;

public class Command : MonoBehaviour
{
    public KeyCode keyCode;
    public Ability ability;

    public Command(KeyCode keyCode, Ability ability)
    {
        this.keyCode = keyCode;
        this.ability = ability;
    }
}
