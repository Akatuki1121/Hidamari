using UnityEngine;

public static class RankingData
{
    private const string k_best_height_key = "Hidamari_BestHeight";

    // 自己ベストを更新した場合のみ保存する
    public static void Register(float p_height)
    {
        float l_current_best = GetBestHeight();

        if (p_height <= l_current_best)
        {
            return;
        }

        PlayerPrefs.SetFloat(k_best_height_key, p_height);
        PlayerPrefs.Save();
    }

    public static float GetBestHeight()
    {
        return PlayerPrefs.GetFloat(k_best_height_key, 0.0f);
    }
}
