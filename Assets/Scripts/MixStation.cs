using System.Collections.Generic;
using UnityEngine;

public class MixStation : MonoBehaviour
{
    [Header("Settings")]
    public int requiredIngredientCount = 3;

    [Header("Prefab")]
    public GameObject foodPrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Dough Check")]
    public float checkRadius = 1f;
    public LayerMask doughLayer;

    [Header("Runtime")]
    public List<Ingredient> currentIngredients = new List<Ingredient>();

    private GameObject currentDough;

    public void AddIngredient(Ingredient ingredient)
    {
        if (ingredient == null) return;

        // ❗ 防止已有面团
        if (IsDoughInSpawnArea())
        {
            Debug.Log("已有面团，不能再加材料");
            return;
        }

        currentIngredients.Add(ingredient);

        Debug.Log("Added: " + ingredient.ingredientName);
        Debug.Log("Current Count: " + currentIngredients.Count);

        Destroy(ingredient.gameObject);

        CheckMixComplete();
    }

    void CheckMixComplete()
    {
        if (currentIngredients.Count >= requiredIngredientCount)
        {
            OnMixComplete();
        }
    }

    void OnMixComplete()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn point assigned!");
            return;
        }

        GameObject dough = Instantiate(
            foodPrefab,
            spawnPoints[0].position,
            spawnPoints[0].rotation
        );

        // ⭐⭐⭐ 核心：写入数据（你之前缺的）
        ItemData data = dough.GetComponent<ItemData>();

        if (data != null)
        {
            data.itemName = "Dough";
            data.itemTag = "Dough";

            data.ingredients.Clear();

            foreach (var ing in currentIngredients)
            {
                data.ingredients.Add(ing.ingredientName);
            }
        }
        else
        {
            Debug.LogWarning("Dough prefab 没有 ItemData！");
        }

        // ⭐ 确保可以拖
        if (dough.GetComponent<DragItem>() == null)
        {
            dough.AddComponent<DragItem>();
        }

        // ⭐ 设置Tag
        dough.tag = "Dough";

        currentDough = dough;

        Debug.Log("Mix Complete!（带数据）");

        ClearIngredients();
    }

    void ClearIngredients()
    {
        currentIngredients.Clear();
    }

    bool IsDoughInSpawnArea()
    {
        if (spawnPoints.Length == 0) return false;

        Collider[] hits = Physics.OverlapSphere(
            spawnPoints[0].position,
            checkRadius
        );

        foreach (var hit in hits)
        {
            ItemData data = hit.GetComponent<ItemData>();

            if (data != null && data.itemTag == "Dough")
            {
                return true;
            }
        }

        return false;
    }


    public void ClearDough()
    {
        currentDough = null;
    }

    void OnDrawGizmos()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnPoints[0].position, checkRadius);
    }
}
