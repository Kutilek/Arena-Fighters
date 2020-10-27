using UnityEngine;
using UnityEngine.UI;

public class Display_Dash_Ready : MonoBehaviour
{
    private Slider slider;
    private Dash dash;
    private Image image;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        dash = GameObject.FindGameObjectWithTag("Player").GetComponent<Dash>();
        slider.maxValue = dash.GetDashCooldown();
        slider.value = slider.maxValue;
    }

    private float timer;

    void Update()
    {
        slider.maxValue = dash.GetDashCooldown();

        if (dash.GetDashed())
        {
            if (slider.value == slider.maxValue)
                slider.value = 0f;
                
            slider.value += Time.deltaTime;        
        }
    }
}
