using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_best_height_text;

    private const string k_game_scene_name = "Game";
    private const string k_height_format = "F2";

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
    }
}
