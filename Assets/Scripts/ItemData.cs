using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public string itemName;
    public string itemTag;

    public List<string> ingredients = new List<string>();

    public float cookTime;
    public int refineCount;
    public float shapeProgress;

    public void CopyFrom(ItemData other)
    {
        itemName = other.itemName;
        itemTag = other.itemTag;

        ingredients = new List<string>(other.ingredients);

        cookTime = other.cookTime;
        refineCount = other.refineCount;
        shapeProgress = other.shapeProgress;
    }
}
