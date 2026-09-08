using TMPro;
using UnityEngine;

public class ResultScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_height_text;

    private const string k_height_format = "F2";

    public void DisplayResult(float p_grow_time, float p_height)
    {
        m_height_text.text = $"{p_height.ToString(k_height_format)}";
    }
}
