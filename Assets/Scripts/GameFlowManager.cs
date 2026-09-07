using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public enum GameState
    {
        explanation,
        playing
    }

    [SerializeField] private GameObject m_explanation_panel;
    [SerializeField] private GameObject m_playing_panel;
    [SerializeField] private PlayerMove m_player_move;

    private const string k_result_scene_name = "Result";

    private GameState m_current_state;
    private float m_play_start_time;

    private void Start()
    {
        ChangeState(GameState.explanation);
    }

    // 操作説明画面の「わかった」ボタンから呼ぶ
    public void OnExplanationConfirmed()
    {
        ChangeState(GameState.playing);
    }

    // ゲームオーバー判定側(未実装)から、最終的な高さを渡して呼んでもらう想定のフック
    public void OnGameOver(float p_final_height)
    {
        float l_grow_time = Time.time - m_play_start_time;
        GameResultData.SetResult(l_grow_time, p_final_height);
        SceneManager.LoadScene(k_result_scene_name);
    }

    private void ChangeState(GameState p_next_state)
    {
        SetAllPanelsInactive();
        ActivatePanelFor(p_next_state);
        m_current_state = p_next_state;
    }

    private void ActivatePanelFor(GameState p_state)
    {
        switch (p_state)
        {
            case GameState.explanation:
                m_explanation_panel.SetActive(true);
                SetPlayerInputEnabled(false);
                break;

            case GameState.playing:
                m_playing_panel.SetActive(true);
                m_play_start_time = Time.time;
                SetPlayerInputEnabled(true);
                break;
        }
    }

    private void SetAllPanelsInactive()
    {
        m_explanation_panel.SetActive(false);
        m_playing_panel.SetActive(false);
    }

    private void SetPlayerInputEnabled(bool p_enabled)
    {
        if (m_player_move == null)
        {
            return;
        }

        m_player_move.enabled = p_enabled;
    }
}
