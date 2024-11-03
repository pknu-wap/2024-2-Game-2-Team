using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

// 호버링 시 이미지가 Fade되는 하이라이트 효과
public class FadeUIHighlighter : HoverUIHighlighter
{
    [SerializeField] private Image hoverImage;
    [SerializeField] private float fadeTime = 0.4f;
    [Tooltip("0 ~ 255 값을 정합니다.")]
    [SerializeField] private int fadeMinValue = 255;
    [Tooltip("0 ~ 255 값을 정합니다.")]
    [SerializeField] private int fadeMaxValue = 0;

    private void Awake()
    {
        hoverImage.color = new Color(1, 1, 1, 0);
    }

    protected override void Highlight()
    {
        hoverImage.DOFade(fadeMinValue / 255f, fadeTime).SetEase(Ease.Linear);
    }

    protected override void Unhighlight()
    {
        hoverImage.DOFade(fadeMaxValue, fadeTime).SetEase(Ease.Linear);
    }
}
