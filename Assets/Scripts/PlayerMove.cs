using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float m_move_speed = 5.0f;
    [SerializeField]
    float minX = -10f;
    [SerializeField]
    float maxX = 10f;

    [SerializeField]
    private bool m_use_camera_relative_direction = true;

    private Transform m_camera_transform;
    private CharacterController m_character_controller;
    private Rigidbody m_rigidbody;

    // 念のための対策
    // 接地中にわずかな下向きの力を与え続け、坂道や段差で浮いてしまうのを防ぐための値
    private const float k_grounded_stick_velocity_y = -2.0f;

    // 入力ベクトルの大きさがこの値未満なら「入力なし」とみなすしきい値(浮動小数の誤差対策)
    private const float k_input_deadzone_sqr_magnitude = 0.0001f;

    // 移動方向への向き直りの速さ(値が大きいほど素早く目標方向を向く)
    private const float k_rotation_speed = 10.0f;

    private Vector3 m_vertical_velocity;

    void Start()
    {
        if (Camera.main != null)
        {
            m_camera_transform = Camera.main.transform;
        }

        m_character_controller = GetComponent<CharacterController>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 move_direction = GetInputDirection();

        if (m_character_controller != null)
        {
            MoveWithCharacterController(move_direction);
        }
        else if (m_rigidbody == null)
        {
            MoveWithTransform(move_direction);
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }

    void FixedUpdate()
    {
        if (m_rigidbody == null)
        {
            return;
        }

        Vector3 move_direction = GetInputDirection();
        MoveWithRigidbody(move_direction);
    }

    private Vector2 GetKeyboardInput()
    {
        if (Keyboard.current == null)
        {
            return Vector2.zero;
        }

        float horizontal_input = 0.0f;
        float vertical_input = 0.0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal_input -= 1.0f;
        }

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal_input += 1.0f;
        }

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            vertical_input -= 1.0f;
        }

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            vertical_input += 1.0f;
        }

        return new Vector2(horizontal_input, vertical_input);
    }

    private Vector3 GetInputDirection()
    {
        Vector2 keyboard_input = GetKeyboardInput();
        float horizontal_input = keyboard_input.x;
        float vertical_input = keyboard_input.y;

        Vector3 raw_input = new Vector3(horizontal_input, 0.0f, vertical_input);

        if (raw_input.sqrMagnitude < k_input_deadzone_sqr_magnitude)
        {
            return Vector3.zero;
        }

        if (!m_use_camera_relative_direction || m_camera_transform == null)
        {
            return raw_input.normalized;
        }

        return GetCameraRelativeDirection(horizontal_input, vertical_input);
    }

    private Vector3 GetCameraRelativeDirection(float horizontal_input, float vertical_input)
    {
        Vector3 camera_forward = m_camera_transform.forward;
        Vector3 camera_right = m_camera_transform.right;

        camera_forward.y = 0.0f;
        camera_right.y = 0.0f;
        camera_forward.Normalize();
        camera_right.Normalize();

        Vector3 direction = (camera_forward * vertical_input) + (camera_right * horizontal_input);

        return direction.normalized;
    }

    private void MoveWithTransform(Vector3 move_direction)
    {
        if (move_direction.sqrMagnitude < k_input_deadzone_sqr_magnitude)
        {
            return;
        }

        transform.position += move_direction * m_move_speed * Time.deltaTime;
        RotateTowardsMoveDirection(move_direction);
    }

    private void MoveWithCharacterController(Vector3 move_direction)
    {
        if (m_character_controller.isGrounded && m_vertical_velocity.y < 0.0f)
        {
            m_vertical_velocity.y = k_grounded_stick_velocity_y;
        }

        // Physics.gravity はProject Settings > Physicsで設定された重力加速度(通常は下向きで負の値)
        float gravity_acceleration_y = Physics.gravity.y;
        m_vertical_velocity.y += gravity_acceleration_y * Time.deltaTime;

        Vector3 horizontal_motion = move_direction * m_move_speed;
        Vector3 total_motion = horizontal_motion + m_vertical_velocity;

        m_character_controller.Move(total_motion * Time.deltaTime);

        if (move_direction.sqrMagnitude >= k_input_deadzone_sqr_magnitude)
        {
            RotateTowardsMoveDirection(move_direction);
        }
    }

    private void MoveWithRigidbody(Vector3 move_direction)
    {
        Vector3 target_velocity = move_direction * m_move_speed;
        target_velocity.y = m_rigidbody.linearVelocity.y;

        m_rigidbody.linearVelocity = target_velocity;

        if (move_direction.sqrMagnitude >= k_input_deadzone_sqr_magnitude)
        {
            RotateTowardsMoveDirection(move_direction);
        }
    }

    private void RotateTowardsMoveDirection(Vector3 move_direction)
    {
        Quaternion target_rotation = Quaternion.LookRotation(move_direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, target_rotation, Time.deltaTime * k_rotation_speed);
    }
}
