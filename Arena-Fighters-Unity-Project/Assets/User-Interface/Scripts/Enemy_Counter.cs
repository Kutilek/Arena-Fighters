using UnityEngine;
using UnityEngine.UI;

public class Enemy_Counter : MonoBehaviour
{
    public GameObject[] enemies;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        text.text = enemies.Length.ToString();
    }
}
