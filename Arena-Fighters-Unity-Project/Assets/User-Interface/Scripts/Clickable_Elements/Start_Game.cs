using UnityEngine.SceneManagement;

public class Start_Game : Clickable_Text
{
    protected override void DoStuffOnClick()
    {
        SceneManager.LoadScene("Arena");
    }
}
