using UnityEngine;

public class Check_For_Pause_Menu : MonoBehaviour
{
    private Transform pauseMenu;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu = transform.Find("Pause_Menu");
    }

    void Update()
    {
        // Check For Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
