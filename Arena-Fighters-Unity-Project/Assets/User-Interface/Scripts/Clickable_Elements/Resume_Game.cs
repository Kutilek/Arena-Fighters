using UnityEngine;

public class Resume_Game : Clickable_Text
{
    private Transform pauseMenu;

    protected override void Awake()
    {
        base.Awake();
        pauseMenu = transform.parent;
    }

    protected override void DoStuffOnClick()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
