using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DayData", menuName = "Game/Day Data")]
public class DayData : ScriptableObject
{
    [Header("NPC List")]
    public List<GameObject> npcPrefabs = new List<GameObject>();

    [Header("Dialogue")]
    [TextArea(3, 10)]
    public List<string> dialogues = new List<string>();
}