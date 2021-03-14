using UnityEngine;
using UnityEngine.UI;

public class Time_Display : MonoBehaviour
{
    private Text text;
    
    private void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        float minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);  
        float seconds = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60);

        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
