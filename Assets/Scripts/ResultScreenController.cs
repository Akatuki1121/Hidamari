using TMPro;
using UnityEngine;

public class ResultScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_grow_time_text;
    [SerializeField] private TMP_Text m_height_text;

    private const string k_time_format = "F1";
    private const string k_height_format = "F2";

    public void DisplayResult(float p_grow_time, float p_height)
    {
        m_grow_time_text.text = $"生存時間: {p_grow_time.ToString(k_time_format)}秒";
        m_height_text.text = $"高さ: {p_height.ToString(k_height_format)}m";
    }
}
