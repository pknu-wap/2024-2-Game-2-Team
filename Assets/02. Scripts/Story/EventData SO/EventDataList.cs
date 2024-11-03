using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventDataList", menuName = "Scriptable Object/Event Data List", order = 11)]
public class EventDataList : ScriptableObject
{
    public List<EventData> list;
}
