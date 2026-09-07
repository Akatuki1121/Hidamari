using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    private const string k_game_scene_name = "Game";

    // スタート画面の「はじめる」ボタンから呼ぶ
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene(k_game_scene_name);
    }
}
