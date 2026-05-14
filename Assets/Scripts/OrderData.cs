using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderData", menuName = "Game/Order")]
public class OrderData : ScriptableObject
{
    public string requiredTag;

    public List<string> requiredIngredients = new List<string>();

    public float minCookTime = 0f;
    public int minRefine = 0;
    public int minShape = 0;


    [TextArea]
    public string dialogue;

    [Header("Complete Dialogue")]
    [TextArea]
    public List<string> completeDialogue = new List<string>();

}
