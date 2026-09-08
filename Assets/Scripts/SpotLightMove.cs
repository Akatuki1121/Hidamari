using UnityEngine;

public class SpotLightMove : MonoBehaviour
{
    [Header("床として扱うオブジェクト(この面積の範囲内で移動する)")]
    [SerializeField]
    private Renderer m_floor_renderer;

    [Header("移動速度")]
    [SerializeField]
    private float m_move_speed = 2.0f;

    [Header("移動範囲")]
    [SerializeField]
    float minX = -10f;
    [SerializeField]
    float maxX = 10f;

    [Header("目標地点に到達したとみなす距離")]
    [SerializeField]
    private float m_arrival_distance = 0.1f;

    [Header("植物")]
    [SerializeField]
    private PlayerGrowth m_plant;

    [Header("点滅")]
    [SerializeField]
    private float m_initial_blink_interval = 0.5f;
    [SerializeField]
    private float m_min_blink_interval = 0.05f;
    [SerializeField]
    private float m_blink_speed_up = 0.1f;

    private float m_min_position_x;
    private float m_max_position_x;
    private float m_target_position_x;

    private float m_outside_time = 0f;
    private float m_blink_timer = 0f;
    private bool m_light_on = true;

    private Light m_light;

    private Renderer m_plant_renderer;

    void Start()
    {
        m_light = GetComponent<Light>();

        if (m_plant != null)
        {
            m_plant_renderer = m_plant.GetComponentInChildren<Renderer>();
        }

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.position = position;

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

        if (IsPlantInSpotLight())
        {
            m_outside_time = 0f;
            m_blink_timer = 0f;

            m_light_on = true;

            if (m_light != null)
            {
                m_light.enabled = true;
            }
        }
        else
        {
            m_outside_time += Time.deltaTime;

            BlinkLight();
        }
    }

    private void SetupMoveRangeFromFloor()
    {
        if (m_floor_renderer == null)
        {
            m_min_position_x = minX;
            m_max_position_x = maxX;
            return;
        }

        Bounds floor_bounds = m_floor_renderer.bounds;
        m_min_position_x = Mathf.Max(floor_bounds.min.x, minX);
        m_max_position_x = Mathf.Min(floor_bounds.max.x, maxX);
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

    private bool IsPlantInSpotLight()
    {
        if (m_plant == null ||
            m_plant_renderer == null ||
            m_light == null)
        {
            return false;
        }

        Bounds plant_bounds =
            m_plant_renderer.bounds;

        Vector3[] check_points =
        {
        plant_bounds.center,

        new Vector3(
            plant_bounds.min.x,
            plant_bounds.min.y,
            plant_bounds.min.z
        ),

        new Vector3(
            plant_bounds.min.x,
            plant_bounds.min.y,
            plant_bounds.max.z
        ),

        new Vector3(
            plant_bounds.min.x,
            plant_bounds.max.y,
            plant_bounds.min.z
        ),

        new Vector3(
            plant_bounds.min.x,
            plant_bounds.max.y,
            plant_bounds.max.z
        ),

        new Vector3(
            plant_bounds.max.x,
            plant_bounds.min.y,
            plant_bounds.min.z
        ),

        new Vector3(
            plant_bounds.max.x,
            plant_bounds.min.y,
            plant_bounds.max.z
        ),

        new Vector3(
            plant_bounds.max.x,
            plant_bounds.max.y,
            plant_bounds.min.z
        ),

        new Vector3(
            plant_bounds.max.x,
            plant_bounds.max.y,
            plant_bounds.max.z
        )
    };

        foreach (Vector3 point in check_points)
        {
            if (IsPointInSpotLight(point))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPointInSpotLight(Vector3 point)
    {
        Vector3 dir =
            point - transform.position;

        float distance =
            dir.magnitude;

        if (distance > m_light.range)
        {
            return false;
        }

        dir.Normalize();

        float angle =
            Vector3.Angle(
                transform.forward,
                dir
            );

        if (angle > m_light.spotAngle / 2f)
        {
            return false;
        }

        return true;
    }

    private void BlinkLight()
    {
        float blink_interval = m_initial_blink_interval - m_outside_time * m_blink_speed_up;

        blink_interval = Mathf.Max(blink_interval, m_min_blink_interval);

        m_blink_timer += Time.deltaTime;

        if (m_blink_timer >= blink_interval)
        {
            m_blink_timer = 0f;

            m_light_on = !m_light_on;

            m_light.enabled = m_light_on;
        }
    }
}
