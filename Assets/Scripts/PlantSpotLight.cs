using UnityEngine;

public class PlantSpotLight : MonoBehaviour
{
    [Header("追いかける植物")]
    [SerializeField] PlayerGrowth plant;

    [Header("スポットライトの設定")]
    [SerializeField] float followSpeed = 3f;
    [SerializeField] float spotLightOffset = 2f;

    private Renderer plantRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (plant != null)
        {
            plantRenderer = plant.GetComponentInChildren<Renderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        if (plant == null || plantRenderer == null)
        {
            return;
        }

        //先端のY座標
        float plantTopY = plantRenderer.bounds.max.y;
        //先端を追いかける
        float targetY = plantTopY + spotLightOffset;
        //追従
        float newY = Mathf.Lerp(transform.position.y, targetY, followSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
