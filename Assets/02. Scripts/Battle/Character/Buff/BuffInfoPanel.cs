using TMPro;

public class BuffInfoPanel : Poolable
{
    public new TMP_Text name;
    public TMP_Text description;

    public void SetContent(string name, string description)
    {
        this.name.text = name;
        this.description.text = description;
    }

}