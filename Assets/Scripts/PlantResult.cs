using UnityEngine;
using TMPro;

public class PlantResult : MonoBehaviour
{
    public TMP_Text hightText;
    public TMP_Text survivalTimeText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowResult(float hight,float survivalTime)
    {
        hightText.text = "成長した高さ:" + hight.ToString("F2") + " m";

        int minutes = Mathf.FloorToInt(survivalTime / 60f);
        int seconds = Mathf.FloorToInt(survivalTime % 60f);

        survivalTimeText.text = "生存時間:" + minutes + "分" + seconds + "秒";
    }
}
