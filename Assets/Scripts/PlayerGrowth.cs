using UnityEngine;

public class PlayerGrowth : MonoBehaviour
{
    [Header("ライト")]
    [SerializeField] Light spotLight;

    [Header("プレイヤー成長速度")]
    [SerializeField] float growthSpeed = 0.1f;
    [SerializeField] float drakGrowthSpeed = 0.1f;
    [SerializeField] float growth = 1f;
    [SerializeField] float growthLimit = 10f;

    [Header("植物の大きさ")]
    [SerializeField] float plantX = 0.2f;
    [SerializeField] float plantZ = 0.2f;

    [Header("位置補正の基準の高さ")]
    [SerializeField] float bestHight = 0.8f;

    [Header("ゲームオーバー判定")]
    [SerializeField] private GameFlowManager m_game_flow_manager;
    [SerializeField] private float m_out_of_light_time_limit = 3.0f;

    private float m_out_of_light_timer = 0.0f;
    private bool m_is_game_over = false;

    private Renderer m_plant_renderer;

    //生存時間
    //float survivalTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_plant_renderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_is_game_over)
        {
            return;
        }

        if(IsPlantTipInSunLight())
        {
            //日に当たっている時
            growthSpeed += 0.1f * Time.deltaTime;
            //成長限界
            growthSpeed = Mathf.Min(growthSpeed, growthLimit);

            m_out_of_light_timer = 0.0f;
        }
        else
        {
            //日に当たっていない時
            growthSpeed -= drakGrowthSpeed * Time.deltaTime;

            growthSpeed = Mathf.Max(growthSpeed, 0f);

            m_out_of_light_timer += Time.deltaTime;

            if (m_out_of_light_timer >= m_out_of_light_time_limit)
            {
                TriggerGameOver();
                return;
            }
        }

        //現在の成長速度
        growth += growthSpeed * Time.deltaTime;
        float newHight = growth;
        //植物の成長
        transform.localScale = new Vector3(plantX, growth, plantZ);
        float offset = (newHight - bestHight) * 0.5f;
        transform.localPosition = new Vector3(transform.localPosition.x, offset, transform.localPosition.z);
    }

    private void TriggerGameOver()
    {
        m_is_game_over = true;

        if (m_game_flow_manager == null)
        {
            return;
        }

        m_game_flow_manager.OnGameOver(growth);
    }

    private bool IsPlantTipInSunLight()
    {
        if (spotLight == null)
        {
            return false;
        }

        if (m_plant_renderer == null)
        {
            return false;
        }


        //植物の先端
        Vector3 plantTip = m_plant_renderer.bounds.max;


        //ライトから植物の先端への方向
        Vector3 dir = plantTip - spotLight.transform.position;

        float dis = dir.magnitude;

        // スポットライトの範囲外
        if (dis > spotLight.range)
        {
            return false;
        }

        dir.Normalize();

        // スポットライトの照射角度
        float angle = Vector3.Angle(spotLight.transform.forward,dir);

        if (angle > spotLight.spotAngle / 2f)
        {
            return false;
        }

        return true;
    }

    //植物の高さ
    public float GetHeight()
    {
        return growth;
    }
    //生存時間
    //public float GetSurvivalTime()
    //{
    //    return survivalTime;
    //}

}
