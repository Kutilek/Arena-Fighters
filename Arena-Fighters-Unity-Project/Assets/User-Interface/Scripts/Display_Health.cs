using UnityEngine;
using UnityEngine.UI;

public class Display_Health : MonoBehaviour
{
    private Text healthText;
    private Health health;
    private void Awake()
    {
        healthText = GetComponent<Text>();
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void Update()
    {
        healthText.text = health.GetAmount().ToString();
    }
}
