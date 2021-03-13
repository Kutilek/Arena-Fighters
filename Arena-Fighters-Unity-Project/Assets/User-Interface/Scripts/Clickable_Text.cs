using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class Clickable_Text : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    protected Text text;
    protected int startFontSize;
    protected Color startColor;
    protected Color hoverColor;

    protected virtual void Awake()
    {
        text = transform.GetComponentInChildren<Text>();
        startFontSize = text.fontSize;
        startColor = text.color;
        hoverColor = Color.yellow;
    }

    protected abstract void DoStuffOnClick();

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = startColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        text.fontSize = startFontSize - 4;
        DoStuffOnClick();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        text.fontSize = startFontSize;
    }
}
