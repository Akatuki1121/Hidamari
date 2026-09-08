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
    private float m_target_position_x;

    void Start()
    {
        SetupMoveRangeFromFloor();
        PickNewTargetPositionX();
    }

    void Update()
    {
        MoveTowardsTargetPositionX();

        if (HasArrivedAtTargetPositionX())
        {
            PickNewTargetPositionX();
        }
    }

    private void SetupMoveRangeFromFloor()
    {
        if (m_floor_renderer == null)
        {
            m_min_position_x = transform.position.x;
            m_max_position_x = transform.position.x;
            return;
        }

        Bounds floor_bounds = m_floor_renderer.bounds;
        m_min_position_x = floor_bounds.min.x;
        m_max_position_x = floor_bounds.max.x;
    }

    private void PickNewTargetPositionX()
    {
        m_target_position_x = Random.Range(m_min_position_x, m_max_position_x);
    }

    private void MoveTowardsTargetPositionX()
    {
        Vector3 current_position = transform.position;

        float new_position_x = Mathf.MoveTowards(
            current_position.x,
            m_target_position_x,
            m_move_speed * Time.deltaTime);

        transform.position = new Vector3(new_position_x, current_position.y, current_position.z);
    }

    private bool HasArrivedAtTargetPositionX()
    {
        float distance_to_target = Mathf.Abs(transform.position.x - m_target_position_x);
        return distance_to_target <= m_arrival_distance;
    }
}
