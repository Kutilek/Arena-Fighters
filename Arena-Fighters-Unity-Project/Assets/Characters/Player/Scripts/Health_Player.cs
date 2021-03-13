using UnityEngine;
using UnityEngine.UI;

public class Health_Player : Health
{
    private Transform deathScreen;
    private Score_Counter scoreCounter;

    protected override void Awake()
    {
        base.Awake();
        deathScreen = GameObject.Find("Canvas").transform.Find("Death_Screen");
        scoreCounter = GameObject.FindGameObjectWithTag("ScoreCounter").GetComponent<Score_Counter>();
    }

    private void CheckForDeath()
    {
        if (amount <= 0f)
        {
            Time.timeScale = 0f;
            deathScreen.gameObject.SetActive(true);
            deathScreen.Find("Final_Score").GetComponent<Text>().text = scoreCounter.score.ToString();
            Cursor.lockState = CursorLockMode.None;    
        }
    }
}
