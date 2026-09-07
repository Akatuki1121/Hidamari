using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneController : MonoBehaviour
{
    [SerializeField] private ResultScreenController m_result_screen_controller;

    private const string k_title_scene_name = "Title";

    private void Start()
    {
        DisplayResult();
    }

    private void DisplayResult()
    {
        if (m_result_screen_controller == null)
        {
            return;
        }

        m_result_screen_controller.DisplayResult(GameResultData.last_grow_time, GameResultData.last_height);
    }

    // リザルト画面の「スタートへ戻る」ボタンから呼ぶ
    public void OnRetryButtonPressed()
    {
        SceneManager.LoadScene(k_title_scene_name);
    }
}
