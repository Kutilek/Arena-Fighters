using UnityEngine;
using UnityEngine.UI;

public class Display_Physics_State : MonoBehaviour
{
    private Character_Physics characterPhysics;
    private Text text;

    private void Awake()
    {
        characterPhysics = GameObject.FindGameObjectWithTag("Player").GetComponent<Character_Physics>();
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = characterPhysics.currentGravityState.ToString() + "\n" + characterPhysics.GetMovementImpairingEffect().ToString();
    }
}
