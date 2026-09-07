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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsInSunLight())
        {
            //日に当たっている時
            growthSpeed += 0.1f * Time.deltaTime;
            //成長限界
            growthSpeed = Mathf.Max(growthSpeed, growthLimit);
        }
        else
        {
            //日に当たっていない時
            growthSpeed -= drakGrowthSpeed * Time.deltaTime;
            growthSpeed = Mathf.Min(growthSpeed, 0f);
        }

        //現在の成長速度
        growth += growthSpeed * Time.deltaTime;
        //植物の成長
        transform.localScale = new Vector3(1f, growth, 1f);
    }

    bool IsInSunLight()
    {
        if(spotLight == null)
            return false;

        Vector3 dir = transform.position - spotLight.transform.position;

        float dis = dir.magnitude;

        //スポットライトの範囲外
        if(dis > spotLight.range)
        {
            return false;
        }

        dir.Normalize();

        //スポットライトの照射角度
        float angle = Vector3.Angle(spotLight.transform.forward, dir);

        if(angle > spotLight.spotAngle/2f)
        {
            return false;
        }

        //光と障害物の判定確認
        //if(Physics.Raycast(spotLight.transform.position,dir,out RaycastHit hit,dis))
        //{
        //    if(hit.transform!=transform)
        //    {
        //        return false;
        //    }
        //}

        return true;
    }

}
