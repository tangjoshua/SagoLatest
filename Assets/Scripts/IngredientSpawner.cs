using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject ingredientPrefab;
    public int maxCount = 3;
    public float spawnInterval = 2f;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    private List<GameObject> currentIngredients = new List<GameObject>();
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            TrySpawn();
            timer = 0f;
        }
    }

    void TrySpawn()
    {
        // 清理已经被拿走的
        currentIngredients.RemoveAll(item => item == null);

        if (currentIngredients.Count >= maxCount)
            return;

        foreach (Transform point in spawnPoints)
        {
            bool occupied = false;

            foreach (GameObject item in currentIngredients)
            {
                if (item != null && Vector3.Distance(item.transform.position, point.position) < 0.1f)
                {
                    occupied = true;
                    break;
                }
            }

            if (!occupied)
            {
                GameObject obj = Instantiate(ingredientPrefab, point.position, Quaternion.identity);
                currentIngredients.Add(obj);
                break;
            }
        }
    }
}
