using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private PlayerMove m_player_move;

    private float m_play_start_time;

    private void Start()
    {
        m_play_start_time = Time.time;
        SetPlayerInputEnabled(true);
    }

    // ゲームオーバー判定側から、最終的な高さを渡して呼んでもらう想定のフック
    public void OnGameOver(float p_final_height)
    {
        float l_grow_time = Time.time - m_play_start_time;
        GameResultData.SetResult(l_grow_time, p_final_height);
        RankingData.Register(p_final_height);
        SceneTransition.LoadResult();
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
