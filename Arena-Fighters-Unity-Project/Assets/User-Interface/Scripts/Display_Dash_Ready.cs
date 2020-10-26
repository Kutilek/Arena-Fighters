using System.Collections;
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
        image = slider.fillRect.GetComponent<Image>();
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

        if (slider.value == slider.maxValue)
            image.color = Color.green;
        else
            image.color = Color.gray;
    }
}
