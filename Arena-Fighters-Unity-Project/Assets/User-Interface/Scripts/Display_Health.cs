using UnityEngine;
using UnityEngine.UI;

public class Display_Health : MonoBehaviour
{
    private Slider slider;
    private Health health;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        slider.maxValue = health.GetStartingAmount();
    }

    void Update()
    {
        slider.value = health.GetAmount();

        if (Input.GetKeyDown(KeyCode.G))
            health.DecreaseHealth(5f);
    }
}
