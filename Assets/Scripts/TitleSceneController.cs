using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_best_height_text;

    private const string k_game_scene_name = "Game";
    private const string k_height_format = "F2";

    private static readonly Color32 k_best_height_text_color = new Color32(0xAE, 0x56, 0x0C, 0xFF);
    private static readonly Color32 k_best_height_outline_color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    private const float k_best_height_outline_width = 0.2f;

    private void Start()
    {
        DisplayBestHeight();
    }

    // スタート画面の「はじめる」ボタンから呼ぶ
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene(k_game_scene_name);
    }

    private void DisplayBestHeight()
    {
        if (m_best_height_text == null)
        {
            return;
        }

        float l_best_height = RankingData.GetBestHeight();
        m_best_height_text.text = $"Best: {l_best_height.ToString(k_height_format)}m";
        m_best_height_text.color = k_best_height_text_color;
        m_best_height_text.outlineWidth = k_best_height_outline_width;
        m_best_height_text.outlineColor = k_best_height_outline_color;
    }
}
