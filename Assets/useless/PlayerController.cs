using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;

    public FloatingJoystick joystick; // UI摇杆

    void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 move = new Vector3(horizontal, 0, vertical);

        if (move.magnitude > 0.1f)
        {
            controller.Move(move * speed * Time.deltaTime);

            // 角色朝向移动方向
            transform.forward = move;
        }
    }
}
