using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// シーン遷移をまとめて管理する静的クラス
// シーン名の変更や遷移前後の共通処理(フェードなど)をここに集約する
public static class SceneTransition
{
    public const string k_title_scene_name = "Title";
    public const string k_tutorial_scene_name = "Tutorial";
    public const string k_game_scene_name = "Game";
    public const string k_result_scene_name = "Result";

    public static void LoadTitle()
    {
        Load(k_title_scene_name);
    }

    public static void LoadTutorial()
    {
        Load(k_tutorial_scene_name);
    }

    public static void LoadGame()
    {
        Load(k_game_scene_name);
    }

    public static void LoadResult()
    {
        Load(k_result_scene_name);
    }

    private static void Load(string p_scene_name)
    {
        SceneManager.LoadScene(p_scene_name);
    }

    // コントローラー接続時、指定のボタンを非表示にしてBボタンで同じ処理を呼び出す
    // 各シーンControllerのUpdate()から毎フレーム呼ぶ想定
    public static void HandleGamepadAdvance(GameObject p_button_object, Action p_on_advance)
    {
        Gamepad l_gamepad = Gamepad.current;
        bool l_gamepad_connected = l_gamepad != null;

        if (p_button_object != null && p_button_object.activeSelf == l_gamepad_connected)
        {
            p_button_object.SetActive(!l_gamepad_connected);
        }

        if (l_gamepad_connected && l_gamepad.buttonEast.wasPressedThisFrame)
        {
            p_on_advance?.Invoke();
        }
    }
}
