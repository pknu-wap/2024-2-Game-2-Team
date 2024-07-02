using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Items : MonoBehaviour
{
    public static Items Instance { get; private set; }
    public Transform slotsParent;
    public List<TMP_Text> slots = new List<TMP_Text>();
    // items를 아이템과 그 수량을 관리할 Dictionary로 제작
    public Dictionary<string, int> items = new Dictionary<string, int>(); 

    private void Awake() => Instance = this;

    private void Start()
    {
        foreach (TMP_Text slot in slotsParent.GetComponentsInChildren<TMP_Text>())
        {
            slots.Add(slot);
            // 게임 시작 시 모든 슬롯을 비활성화
            slot.gameObject.SetActive(false);
        }
        UpdateToSlot();

        #if UNITY_EDITOR
            TestItem();
        #endif
    }

    void TestItem()
    {
        items.Add("관찰력", 1);
        items.Add("빵", 1);

        UpdateToSlot();
    }

    public void AddItem(string itemName)
    {
        // 아이템이 있는지 검색
        if (items.ContainsKey(itemName))
        {
            // 아이템이 이미 있다면 아이템 수량을 증가
            items[itemName]++;
        }
        else
        {
            // 아이템이 없다면 인벤토리에 해당 아이템을 추가
            items[itemName] = 1;
        }
        UpdateToSlot();
    }

    public void RemoveItem(string itemName)
    {
        // 아이템이 있는지 검색
        if (items.ContainsKey(itemName))
        {
            // 아이템이 한개 이상 있다면 수량을 감소
            items[itemName]--;
            // 아이템이 1개만 있다면 인벤토리에서 아이템 삭제
            if (items[itemName] <= 0)
            {
                items.Remove(itemName);
            }
            UpdateToSlot();
        }
    }


    private void UpdateToSlot()
    {
        // 모든 슬롯 초기화 및 비활성화
        foreach (TMP_Text slot in slots)
        {
            slot.gameObject.SetActive(false);
        }

        int i = 0;
        // items 딕셔너리에 있는 아이템과 수량을 인벤토리 슬롯에 추가
        foreach (var item in items)
        {
            if (i >= slots.Count) break;

            slots[i].text = item.Value > 1 ? $"{item.Key} x {item.Value}" : item.Key;
            slots[i].gameObject.SetActive(true); // 해당 슬롯 활성화
            i++;
        }
    }
}