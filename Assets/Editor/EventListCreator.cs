using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventListCreator : Editor
{
    public static string listPath = "Assets/02. Scripts/Story/EventData SO/Events/Data List/EventList.asset";

    [MenuItem("Assets/Create Story/Create Event List")]
    public static void CreateEventList()
    {
        ClearEventList(listPath);

        CreateEventList(listPath, "Assets/02. Scripts/Story/EventData SO/Events/Main");
        CreateEventList(listPath, "Assets/02. Scripts/Story/EventData SO/Events/Main Incarnage");
        CreateEventList(listPath, "Assets/02. Scripts/Story/EventData SO/Events/Sub");
        CreateEventList(listPath, "Assets/02. Scripts/Story/EventData SO/Events/Sub Incarnage");
        CreateEventList(listPath, "Assets/02. Scripts/Story/EventData SO/Events/Relation");
        CreateEventList(listPath, "Assets/02. Scripts/Story/EventData SO/Events/Relation Incarnage");
    }

    // listPath의 리스트에 folderPath 내 데이터를 추가합니다.
    private static void CreateEventList(string listPath, string folderPath)
    {
        EventDataList eventDataList = AssetDatabase.LoadAssetAtPath<EventDataList>(listPath);

        // 폴더 내의 모든 에셋 파일 경로 가져오기
        string[] assetGuids = AssetDatabase.FindAssets("", new[] { folderPath });
        // 각 에셋을 로드하고 ScriptableObject로 캐스팅
        foreach (string guid in assetGuids)
        {
            // 에셋 경로 가져오기
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            // 에셋 로드
            EventData asset = AssetDatabase.LoadAssetAtPath<EventData>(assetPath);

            eventDataList.list.Add(asset);
        }

        Debug.Log(folderPath + " 경로의 이벤트를 " + listPath + "에 모두 할당했습니다.");
    }

    private static void ClearEventList(string listPath)
    {
        EventDataList eventDataList = AssetDatabase.LoadAssetAtPath<EventDataList>(listPath);

        if (eventDataList != null)
        {
            Debug.Log(listPath + "가 올바르지 않습니다.");
            return;
        }

        eventDataList.list = new List<EventData>();
        Debug.Log(listPath + "를 비웠습니다.");
    }
}
