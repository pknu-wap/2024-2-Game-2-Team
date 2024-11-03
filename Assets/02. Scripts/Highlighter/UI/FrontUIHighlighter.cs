public class FrontUIHighlighter : HoverUIHighlighter
{
    private int originSiblingIndex;

    protected override void Highlight()
    {
        transform.SetAsLastSibling();
    }

    protected override void Unhighlight()
    {
        transform.SetSiblingIndex(originSiblingIndex);
    }
}
