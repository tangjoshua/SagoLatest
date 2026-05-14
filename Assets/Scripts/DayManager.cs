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
    private bool waitingNextDay = false;

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

        if (DayUI.Instance != null)
        {
            DayUI.Instance.UpdateDay(currentDayIndex + 1);
        }

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
            waitingNextDay = true;

            DialogueUI.Instance.OnDialogueEnd += HandleDialogueEnd;

            return;
        }

        GameObject npcPrefab = day.npcPrefabs[currentNPCIndex];

        npcSpawner.SpawnNPC(npcPrefab);

        currentNPCIndex++;
    }

    // =========================
    // ⭐ 一天结束
    // =========================

    void NextDay()
    {
        Debug.Log("➡ 进入下一天");

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

    void HandleDialogueEnd()
    {
        DialogueUI.Instance.OnDialogueEnd -= HandleDialogueEnd;

        if (waitingNextDay)
        {
            waitingNextDay = false;

            NextDay();
        }
    }
}