using UnityEngine;
using UnityEngine.UI;

public class Display_Health : MonoBehaviour
{
    private Slider slider;
    private Health_Player health;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health_Player>();
        slider.maxValue = health.GetStartingAmount();
        slider.value = slider.maxValue;
    }

    void Update()
    {
        slider.value = health.GetAmount();
    }
}
