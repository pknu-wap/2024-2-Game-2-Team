using UnityEngine;
using UnityEngine.EventSystems;

public abstract class HoverUIHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected abstract void Highlight();
    protected abstract void Unhighlight();

    public void OnPointerEnter(PointerEventData eventData)
    {
        Highlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Unhighlight();
    }
}
