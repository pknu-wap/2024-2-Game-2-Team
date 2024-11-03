using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
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
    }

    // 데이터 파일 이름
    string dataFileName = "Data.json";

    // 데이터 파일 경로
    string filePath;

    // SaveData 변수
    public Data data = new Data();

    // load 여부
    public bool isLoaded = false;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/" + dataFileName;
    }

    // 로드
    public void LoadData()
    {
        // 세이브 데이터가 있으면
        if (IsFileExist())
        {
            // 저장된 파일을 읽고
            string json = File.ReadAllText(filePath);

            byte[] bytes = System.Convert.FromBase64String(json);

            string decodedJson = System.Text.Encoding.UTF8.GetString(bytes);

            // Json을 Data 형식으로 전환
            data = JsonUtility.FromJson<Data>(decodedJson);

            // DebugData(data);
        }
    }

    // 세이브
    // 세이브 타이밍: 전투 종료, 이벤트 종료, 사망, 아이템 획득, 카드 획득, 수동 저장
    public void SaveData()
    {
        // 옵션으로 생기는 세이브 방지
        if(data.currentEvent == null)
        {
            return;
        }

        string filePath = Application.persistentDataPath + "/" + dataFileName;

        // Data를 Json으로 변환 (true = 가독성 향상)
        string json = JsonUtility.ToJson(data, true);

        // json 파일을 8bit unsigned int로 변환
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);

        // 바이트 배열을 base-64 인코딩 문자열로 변환
        string encodedJson = System.Convert.ToBase64String(bytes);

        // 파일을 새로 생성하거나 덮어쓰기
        File.WriteAllText(filePath, encodedJson);
    }

    private void DebugData(Data data)
    {
        // Print Stat Info
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Data");
        sb.AppendFormat(" - Hp : {0}\n", data.Hp);
        sb.AppendFormat(" - Job : {0}\n", data.Job);
        for (int i = 0; i < data.deck.Count; i++)
        {
            sb.AppendFormat(" - deck[{0}] : {1}\n", i, data.deck[i]);
        }
        sb.AppendFormat(" - Current Event : {0}\n", data.CurrentEvent);

        for (int i = 0; i < data.processableMainEventList.Count; i++)
        {
            sb.AppendFormat(" - processableMainEventList[{0}] : {1}\n", i, data.processableMainEventList[i]);
        }

        for (int i = 0; i < data.processableSubEventList.Count; i++)
        {
            sb.AppendFormat(" - processableSubEventList[{0}] : {1}\n", i, data.processableSubEventList[i]);
        }

        for(int i = 0; i < data.delayDictionary.Count; ++i)
        {
            sb.AppendFormat(" - delayDictionary : {0}\n", data.delayDictionary[i].key);
        }

        for (int i = 0; i < data.items.Count; i++)
        {
            sb.AppendFormat(" - items[{0}] : {1}\n", i, data.items[i].name);
        }

        Debug.LogError(sb.ToString());
    }

    public void DeleteData()
    {
        if (IsFileExist())
        {
            File.Delete(filePath);
        }
    }
    
    public void StartLoadedGame()
    {
        LoadData();

        // 각자 자리에 삽입한다.
        Player.Instance.LoadHp();
        CardManager.Instance.LoadDeck();
        DialogueManager.Instance.LoadDialogueData();
        Items.Instance.LoadItems();
        SoundManager.Instance.LoadVolumeSettings();
        ResolutionManager.Instance.LoadResolutionSettings();

        StartCoroutine(DialogueManager.Instance.ProcessRandomEvent());
    }

    public List<DictionaryData> DictionaryToList(Dictionary<string, int> dict)
    {
        List<DictionaryData> list = new List<DictionaryData>();

        foreach (string key in dict.Keys)
        {
            DictionaryData data = new DictionaryData();
            data.key = key;
            data.value = dict[key];

            list.Add(data);
        }

        return list;
    }

    public Dictionary<string, int> ListToDictionary(List<DictionaryData> list)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();

        foreach (DictionaryData data in list)
        {
            dict[data.key] = data.value;
        }

        return dict;
    }

    public bool IsFileExist()
    {
        return File.Exists(Application.persistentDataPath + "/" + dataFileName);
    }
}
