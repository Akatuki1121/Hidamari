using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_height_text;

    [Header("高さに応じて切り替える立ち絵")]
    [SerializeField] private Image m_illustration_1; // 0m 以上 〜 80m 未満
    [SerializeField] private Image m_illustration_2; // 80m 以上 〜 160m 未満
    [SerializeField] private Image m_illustration_3; // 160m 以上

    private const string k_height_format = "F2";

    // 立ち絵切り替えの境界値(m)
    private const float k_illustration_2_threshold = 80.0f;
    private const float k_illustration_3_threshold = 160.0f;

    public void DisplayResult(float p_grow_time, float p_height)
    {
        m_height_text.text = $"{p_height.ToString(k_height_format)}";

        DisplayIllustration(p_height);
    }

    private void DisplayIllustration(float p_height)
    {
        bool l_is_stage_1 = p_height < k_illustration_2_threshold;
        bool l_is_stage_2 = !l_is_stage_1 && p_height < k_illustration_3_threshold;
        bool l_is_stage_3 = !l_is_stage_1 && !l_is_stage_2;

        SetActiveIfAssigned(m_illustration_1, l_is_stage_1);
        SetActiveIfAssigned(m_illustration_2, l_is_stage_2);
        SetActiveIfAssigned(m_illustration_3, l_is_stage_3);
    }

    private void SetActiveIfAssigned(Image p_image, bool p_active)
    {
        if (p_image == null)
        {
            return;
        }

        p_image.gameObject.SetActive(p_active);
    }
}
