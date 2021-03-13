using UnityEngine;
using UnityEngine.UI;

public class Font_Loader : MonoBehaviour
{
    public Font font;

    private void Awake()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text text in texts)
            text.font = font;
    }
}
