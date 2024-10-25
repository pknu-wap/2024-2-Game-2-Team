using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
    public CardData cardData;
    [SerializeField] Image card;
    [SerializeField] Image illust;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;
    [SerializeField] TMP_Text descriptionTMP;
    public void Setup(CardData item)
    {
        cardData = item;

        illust.sprite = cardData.sprite;
        nameTMP.text = cardData.name;
        costTMP.text = cardData.cost.ToString();
        descriptionTMP.text = SetDamageDescription(cardData.description);
    }

    private string SetDamageDescription(string originText)
    {
        string result = originText;

        for (int i = 0; i < cardData.skills.Length; i++)
        {
            // "damage + 해당하는 스킬의 인덱스"인 부분을 대체
            result = result.Replace("damage" + i, cardData.skills[i].amount.ToString());
        }

        return result;
    }
}
