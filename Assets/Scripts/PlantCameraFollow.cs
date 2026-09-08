using UnityEngine;

public class PlantCameraFollow : MonoBehaviour
{
    [Header("追いかける植物")]
    [SerializeField] Transform plant;

    [Header("カメラの設定")]
    [SerializeField] float followSpeed = 3f;
    [SerializeField] float cameraHightOffset = 0f;

    private float FixedX;
    private float FixedZ;

    private Renderer plantPenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FixedX = transform.position.x;
        FixedZ = transform.position.z;

        plantPenderer = plant.GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        if (plant == null || plantPenderer == null)
        {
            return;
        }
        //先端のY座標
        float plantTopY = plantPenderer.bounds.max.y;
        //先端を追いかける
        float targetY = plantTopY + cameraHightOffset;
        //追従
        float newY = Mathf.Lerp(transform.position.y, targetY, followSpeed * Time.deltaTime);

        transform.position = new Vector3(FixedX, newY, FixedZ);
    }
}
