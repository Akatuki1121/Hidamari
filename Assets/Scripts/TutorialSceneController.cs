using UnityEngine;

public class TutorialSceneController : MonoBehaviour
{
    [SerializeField] private GameObject m_next_button_object;

    private void Update()
    {
        SceneTransition.HandleGamepadAdvance(m_next_button_object, OnNextButtonPressed);
    }

    // チュートリアル画面の「次へ」ボタンから呼ぶ
    public void OnNextButtonPressed()
    {
        SceneTransition.LoadGame();
    }
}
