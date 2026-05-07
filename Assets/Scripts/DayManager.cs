using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [Header("Days")]
    public List<DayData> days = new List<DayData>();

    [Header("Spawner")]
    public NPCSpawner npcSpawner;

    private int currentDayIndex = 0;
    private int currentNPCIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartDay();
    }

    // =========================
    // ⭐ 开始一天
    // =========================
    void StartDay()
    {
        currentNPCIndex = 0;

        Debug.Log("📅 Day " + (currentDayIndex + 1));

        ShowDayDialogue();

        Invoke(nameof(StartNPCFlow), 3f);
    }

    // =========================
    // ⭐ 显示剧情
    // =========================
    void ShowDayDialogue()
    {
        DayData day = days[currentDayIndex];

        DialogueUI.Instance.StartDialogue(day.dialogues);
    }

    // =========================
    // ⭐ DialogueUI 会调用这里
    // =========================
    public void StartNPCFlow()
    {
        Debug.Log("🚶‍♂️ 开始NPC流程");
        SpawnNextNPC();
    }

    // =========================
    // ⭐ 生成下一位NPC
    // =========================
    public void SpawnNextNPC()
    {
        DayData day = days[currentDayIndex];

        if (currentNPCIndex >= day.npcPrefabs.Count)
        {
            EndDay();
            return;
        }

        GameObject npcPrefab = day.npcPrefabs[currentNPCIndex];

        npcSpawner.SpawnNPC(npcPrefab);

        currentNPCIndex++;
    }

    // =========================
    // ⭐ 一天结束
    // =========================
    void EndDay()
    {
        Debug.Log("🌙 Day Complete");

        currentDayIndex++;

        if (currentDayIndex >= days.Count)
        {
            GameEnd();
            return;
        }

        StartDay();
    }

    // =========================
    void GameEnd()
    {
        Debug.Log("🎉 游戏结束");

        DialogueUI.Instance.StartDialogue(
            new List<string>()
            {
                "Thank you for playing!",
                "Game End"
            }
        );
    }
}