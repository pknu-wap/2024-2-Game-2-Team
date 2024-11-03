using System;
using System.Collections.Generic;

[Serializable]
public class DictionaryData
{
    public string key;
    public int value;
}

[Serializable]
public class Data
{
    // 체력
    public int hp;
    // 덱
    public List<CardData> deck;
    // 직업
    public string job;
    // 현재 이벤트
    public string currentEvent;
    // 이벤트 리스트
    public List<string> processableMainEventList;
    public List<string> processableSubEventList;
    // 딜레이 딕셔너리
    public List<DictionaryData> delayDictionary;
    // 아이템 인벤토리
    public List<Item> items = new List<Item>();
    //옵션
    public int selectedResolutionIndex;
    public int selectedFrameRateIndex;
    public bool fullscreen;
    public bool isMasterVolumeMuted;
    public bool isBgmVolumeMuted;
    public bool isSfxVolumeMuted;
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;


    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    public List<CardData> Deck
    {
        get { return deck; }
        set { deck = value; }
    }

    public string Job
    {
        get { return job; }
        set { job = value; }
    }

    public string CurrentEvent
    {
        get { return currentEvent; }
        set { currentEvent = value; }
    }

    public List<string> ProcessableMainEventList
    {
        get { return processableMainEventList; }
        set { processableMainEventList = value; }
    }

    public List<string> ProcessableSubEventList
    {
        get { return processableSubEventList; }
        set { processableSubEventList = value; }
    }

    public List<DictionaryData> DelayDictionary
    {
        get { return delayDictionary; }
        set { delayDictionary = value; }
    }

    public List<Item> Items
    {
        get { return items; }
        set { items = value; }
    }

    public int SelectedResolutionIndex
    {
        get { return selectedResolutionIndex; }
        set { selectedResolutionIndex = value; }
    }

    public int SelectedFrameRateIndex
    {
        get { return selectedFrameRateIndex; }
        set { selectedFrameRateIndex = value; }
    }

    public bool Fullscreen
    {
        get { return fullscreen; }
        set {  fullscreen = value; }
    }

    public bool IsMasterVolumeMuted
    {
        get { return isMasterVolumeMuted; }
        set { isMasterVolumeMuted = value; }
    }
    public bool IsBgmVolumeMuted
    {
        get { return isBgmVolumeMuted; }
        set { isBgmVolumeMuted = value; }
    }
    public bool IsSfxVolumeMuted
    {
        get { return isSfxVolumeMuted; }
        set { isSfxVolumeMuted = value; }
    }

    public float MasterVolume
    {
        get { return masterVolume; }
        set {  masterVolume = value; }
    }

    public float BgmVolume
    {
        get { return bgmVolume; }
        set { bgmVolume = value; }
    }
    public float SfxVolume
    {
        get { return sfxVolume; }
        set { sfxVolume = value; }
    }
}
