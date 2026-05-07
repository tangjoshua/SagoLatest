using UnityEngine;

public class RefineTarget : MonoBehaviour
{
    private RefineStation station;

    public void SetStation(RefineStation rs)
    {
        station = rs;
    }

    void OnMouseDown()
    {
        // ⭐ 点击销毁
        if (station != null)
        {
            station.OnTargetDestroyed(gameObject);
        }

        Destroy(gameObject);
    }
}
