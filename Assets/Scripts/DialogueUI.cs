using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI")]
    public GameObject panel;
    public TextMeshProUGUI dialogueText;

    private List<string> lines = new List<string>();

    private int currentLine = 0;

    private bool isDialogueActive = false;
    public System.Action OnDialogueEnd;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isDialogueActive) return;

        // PC
        if (Input.GetMouseButtonDown(0))
        {
            NextLine();
        }

        // Mobile
        if (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began)
        {
            NextLine();
        }
    }

    // =========================
    // ⭐ 开始对话
    // =========================
    public void StartDialogue(List<string> newLines)
    {
        if (newLines == null || newLines.Count == 0)
        {
            Debug.LogWarning("Dialogue为空");

            EndDialogue();
            return;
        }

        lines = newLines;

        currentLine = 0;

        panel.SetActive(true);

        isDialogueActive = true;

        ShowLine();
    }

    // =========================
    // ⭐ 显示当前行
    // =========================
    void ShowLine()
    {
        if (currentLine >= lines.Count)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = lines[currentLine];

        Debug.Log("显示对白: " + lines[currentLine]);
    }

    // =========================
    // ⭐ 下一句
    // =========================
    void NextLine()
    {
        currentLine++;

        if (currentLine >= lines.Count)
        {
            EndDialogue();
            return;
        }

        ShowLine();
    }

    // =========================
    // ⭐ 对话结束
    // =========================
    void EndDialogue()
    {
        panel.SetActive(false);

        dialogueText.text = "";

        isDialogueActive = false;

        Debug.Log("对白结束");

        // ⭐ 通知外部
        OnDialogueEnd?.Invoke();
    }
    
}