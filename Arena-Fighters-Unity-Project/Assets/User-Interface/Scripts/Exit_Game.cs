using UnityEngine;

public class Exit_Game : Clickable_Text
{
    protected override void DoStuffOnClick()
    {
        Application.Quit();
    }
}
