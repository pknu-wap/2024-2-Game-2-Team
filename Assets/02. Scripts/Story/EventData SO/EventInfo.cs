using System.Collections.Generic;
using UnityEngine;

public class EventInfo : MonoBehaviour
{
    public static EventInfo Instance { get; private set; }

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

    [Header("보상 카드 리스트")]
    [SerializeField] private EventDataList eventList;
    // 코드에서 접근하는, listName을 받아 CardList를 반환하는 딕셔너리
    public Dictionary<string, EventData> eventListDict;

    // listName을 키로, CardList를 밸류로 갖는 Dictionary를 생성한다.
    private void CreateRewardCardListDictionary()
    {
        eventListDict = new Dictionary<string, EventData>();

        for (int i = 0; i < eventList.list.Count; i++)
        {
            eventListDict.Add(eventList.list[i].eventName, eventList.list[i]);
        }
    }

    public EventData GetEvent(string eventName)
    {
        if(eventListDict.ContainsKey(eventName) == false)
        {
            Debug.LogError(eventName + "에 해당하는 이벤트가 없습니다.");
            return null;
        }

        return eventListDict[eventName];
    }
}
