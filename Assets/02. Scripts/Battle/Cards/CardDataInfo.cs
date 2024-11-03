using System.Collections.Generic;
using UnityEngine;

public class CardDataInfo : MonoBehaviour
{
    public static CardDataInfo Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(Instance);
        CreateRewardCardListDictionary();
    }

    [Header("카드 리스트")]
    [SerializeField] private CardList cardList;
    // 코드에서 접근하는, listName을 받아 CardList를 반환하는 딕셔너리
    public Dictionary<string, CardData> cardListDict;

    // listName을 키로, CardList를 밸류로 갖는 Dictionary를 생성한다.
    private void CreateRewardCardListDictionary()
    {
        cardListDict = new Dictionary<string, CardData>();

        for (int i = 0; i < cardList.items.Length; i++)
        {
            cardListDict.Add(cardList.items[i].name, cardList.items[i]);
        }
    }

    public CardData GetCard(string cardName)
    {
        if (cardListDict.ContainsKey(cardName) == false)
        {
            Debug.LogError(cardName + "에 해당하는 이벤트가 없습니다.");
            return null;
        }

        return cardListDict[cardName];
    }
}
