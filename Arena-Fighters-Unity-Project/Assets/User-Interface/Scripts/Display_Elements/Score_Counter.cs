using UnityEngine;
using UnityEngine.UI;

public class Score_Counter : MonoBehaviour
{
    public int score;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = score.ToString();
    }
}
