using UnityEngine;

public class ResultSceneController : MonoBehaviour
{
    [SerializeField] private ResultScreenController m_result_screen_controller;
    [SerializeField] private GameObject m_back_to_title_button_object;

    private void Start()
    {
        DisplayResult();
    }

    private void Update()
    {
        SceneTransition.HandleGamepadAdvance(m_back_to_title_button_object, OnRetryButtonPressed);
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
        SceneTransition.LoadTitle();
    }
}
