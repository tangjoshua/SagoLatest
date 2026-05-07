using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // Player
    public Vector3 offset;     // 偏移

    void LateUpdate()
    {
      transform.position = Vector3.Lerp(
    transform.position,
    target.position + offset,
    Time.deltaTime * 5f);
    }
    

}
