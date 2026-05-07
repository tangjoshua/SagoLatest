using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    public void SpawnNPC(GameObject npcPrefab)
    {
        Instantiate(
            npcPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        Debug.Log("👤 NPC生成");
    }
}