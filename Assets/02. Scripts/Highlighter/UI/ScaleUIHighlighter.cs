using DG.Tweening;
using UnityEngine;

// 호버링 시 크기를 변경해 강조하는 효과
public class ScaleUIHighlighter : HoverUIHighlighter
{
    private RectTransform myRect;
    [SerializeField] private float scaleTime = 0.4f;
    [SerializeField] private Vector3 originScale = Vector3.one;
    [SerializeField] private Vector3 highlightScale = new Vector3(1.1f, 1.1f, 1.1f);

    void Awake()
    {
        myRect = GetComponent<RectTransform>();
    }

    protected override void Highlight()
    {
        myRect.DOScale(highlightScale, scaleTime).SetEase(Ease.Linear);
    }

    protected override void Unhighlight()
    {
        myRect.DOScale(originScale, scaleTime).SetEase(Ease.Linear);
    }
}
