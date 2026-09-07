using UnityEngine;

public class SpotLightMove : MonoBehaviour
{
    [Header("床として扱うオブジェクト(この面積の範囲内で移動する)")]
    [SerializeField]
    private Renderer m_floor_renderer;

    [Header("移動速度")]
    [SerializeField]
    private float m_move_speed = 2.0f;

    [Header("目標地点に到達したとみなす距離")]
    [SerializeField]
    private float m_arrival_distance = 0.1f;

    private float m_min_position_x;
    private float m_max_position_x;
    private float m_min_position_z;
    private float m_max_position_z;
    private Vector2 m_target_position_xz;

    void Start()
    {
        SetupMoveRangeFromFloor();
        PickNewTargetPositionXZ();
    }

    void Update()
    {
        MoveTowardsTargetPositionXZ();

        if (HasArrivedAtTargetPositionXZ())
        {
            PickNewTargetPositionXZ();
        }
    }

    private void SetupMoveRangeFromFloor()
    {
        if (m_floor_renderer == null)
        {
            m_min_position_x = transform.position.x;
            m_max_position_x = transform.position.x;
            m_min_position_z = transform.position.z;
            m_max_position_z = transform.position.z;
            return;
        }

        Bounds floor_bounds = m_floor_renderer.bounds;
        m_min_position_x = floor_bounds.min.x;
        m_max_position_x = floor_bounds.max.x;
        m_min_position_z = floor_bounds.min.z;
        m_max_position_z = floor_bounds.max.z;
    }

    private void PickNewTargetPositionXZ()
    {
        float target_x = Random.Range(m_min_position_x, m_max_position_x);
        float target_z = Random.Range(m_min_position_z, m_max_position_z);
        m_target_position_xz = new Vector2(target_x, target_z);
    }

    private void MoveTowardsTargetPositionXZ()
    {
        Vector3 current_position = transform.position;
        Vector2 current_position_xz = new Vector2(current_position.x, current_position.z);

        Vector2 new_position_xz = Vector2.MoveTowards(
            current_position_xz,
            m_target_position_xz,
            m_move_speed * Time.deltaTime);

        transform.position = new Vector3(new_position_xz.x, current_position.y, new_position_xz.y);
    }

    private bool HasArrivedAtTargetPositionXZ()
    {
        Vector2 current_position_xz = new Vector2(transform.position.x, transform.position.z);
        float distance_to_target = Vector2.Distance(current_position_xz, m_target_position_xz);
        return distance_to_target <= m_arrival_distance;
    }
}
