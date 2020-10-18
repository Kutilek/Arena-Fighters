using UnityEngine;
using UnityEngine.UI;

public class Display_Dash_Ready : MonoBehaviour
{
    private Text dashText;
    private Dash dash;
    private void Awake()
    {
        dashText = GetComponent<Text>();
        dash = GameObject.FindGameObjectWithTag("Player").GetComponent<Dash>();
    }

    void Update()
    {
        if (!dash.GetDashed())
        {
            dashText.color = Color.green;
            dashText.text = "Dash Is Ready";
        }      
        else
        {
            dashText.color = Color.red;
            dashText.text = "Dash Is Not Ready";
        }      
    }
}
