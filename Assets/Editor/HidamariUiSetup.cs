using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class HidamariUiSetup
{
    private const string k_title_scene_path = "Assets/Scenes/Title.unity";
    private const string k_game_scene_path = "Assets/Scenes/Game.unity";
    private const string k_result_scene_path = "Assets/Scenes/Result.unity";

    private const float k_overlay_alpha = 0.7f;
    private const float k_button_width = 320.0f;
    private const float k_button_height = 80.0f;

    [MenuItem("Hidamari/UIを仮配置 (Title, Game, Result)")]
    public static void SetupAllScenes()
    {
        SetupTitleScene();
        SetupGameScene();
        SetupResultScene();

        EditorUtility.DisplayDialog("Hidamari UI仮配置", "Title / Game / Result のUIを仮配置しました。", "OK");
    }

    private static void SetupTitleScene()
    {
        Scene l_scene = OpenScene(k_title_scene_path);

        GameObject l_canvas_object = CreateCanvas("Canvas");
        TitleSceneController l_controller = FindOrAddComponent<TitleSceneController>();

        CreateTitleLabel(l_canvas_object.transform, "Hidamari");

        Button l_start_button = CreateButton(l_canvas_object.transform, "StartButton", "Start", new Vector2(0.0f, -40.0f));
        AddButtonListener(l_start_button, l_controller, nameof(TitleSceneController.OnStartButtonPressed));

        TMP_Text l_best_height_text = CreateLabel(l_canvas_object.transform, "BestHeightText", "Best: -- m", new Vector2(0.0f, -160.0f), 32);
        AssignSerializedField(l_controller, "m_best_height_text", l_best_height_text);

        SaveScene(l_scene);
    }

    private static void SetupGameScene()
    {
        Scene l_scene = OpenScene(k_game_scene_path);

        GameObject l_canvas_object = CreateCanvas("Canvas");
        GameFlowManager l_flow_manager = FindOrAddComponent<GameFlowManager>();

        GameObject l_explanation_panel = CreateExplanationPanel(l_canvas_object.transform);
        AssignSerializedField(l_flow_manager, "m_explanation_panel", l_explanation_panel);

        PlayerMove l_player_move = Object.FindAnyObjectByType<PlayerMove>();
        if (l_player_move != null)
        {
            AssignSerializedField(l_flow_manager, "m_player_move", l_player_move);
        }

        SaveScene(l_scene);
    }

    private static void SetupResultScene()
    {
        Scene l_scene = OpenScene(k_result_scene_path);

        GameObject l_canvas_object = CreateCanvas("Canvas");
        ResultSceneController l_scene_controller = FindOrAddComponent<ResultSceneController>();
        ResultScreenController l_screen_controller = FindOrAddComponent<ResultScreenController>();

        TMP_Text l_grow_time_text = CreateLabel(l_canvas_object.transform, "GrowTimeText", "Time: -- s", new Vector2(0.0f, 100.0f), 32);
        TMP_Text l_height_text = CreateLabel(l_canvas_object.transform, "HeightText", "Height: -- m", new Vector2(0.0f, 30.0f), 32);

        AssignSerializedField(l_screen_controller, "m_grow_time_text", l_grow_time_text);
        AssignSerializedField(l_screen_controller, "m_height_text", l_height_text);
        AssignSerializedField(l_scene_controller, "m_result_screen_controller", l_screen_controller);

        Button l_retry_button = CreateButton(l_canvas_object.transform, "BackToTitleButton", "Back to title", new Vector2(0.0f, -120.0f));
        AddButtonListener(l_retry_button, l_scene_controller, nameof(ResultSceneController.OnRetryButtonPressed));

        SaveScene(l_scene);
    }

    private static Scene OpenScene(string p_scene_path)
    {
        return EditorSceneManager.OpenScene(p_scene_path, OpenSceneMode.Single);
    }

    private static void SaveScene(Scene p_scene)
    {
        EditorSceneManager.MarkSceneDirty(p_scene);
        EditorSceneManager.SaveScene(p_scene);
    }

    private static T FindOrAddComponent<T>() where T : Component
    {
        T l_existing = Object.FindAnyObjectByType<T>();
        if (l_existing != null)
        {
            return l_existing;
        }

        GameObject l_holder = new GameObject(typeof(T).Name);
        return l_holder.AddComponent<T>();
    }

    private static GameObject CreateCanvas(string p_name)
    {
        GameObject l_existing_canvas = GameObject.Find(p_name);
        if (l_existing_canvas != null)
        {
            return l_existing_canvas;
        }

        GameObject l_canvas_object = new GameObject(p_name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas l_canvas = l_canvas_object.GetComponent<Canvas>();
        l_canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler l_scaler = l_canvas_object.GetComponent<CanvasScaler>();
        l_scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        l_scaler.referenceResolution = new Vector2(1920.0f, 1080.0f);

        EnsureEventSystemExists();

        return l_canvas_object;
    }

    private static void EnsureEventSystemExists()
    {
        UnityEngine.EventSystems.EventSystem l_existing = Object.FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>();
        if (l_existing != null)
        {
            return;
        }

        GameObject l_event_system_object = new GameObject(
            "EventSystem",
            typeof(UnityEngine.EventSystems.EventSystem),
            typeof(UnityEngine.EventSystems.StandaloneInputModule));
    }

    private static TMP_Text CreateLabel(Transform p_parent, string p_name, string p_text, Vector2 p_anchored_position, float p_font_size)
    {
        GameObject l_label_object = new GameObject(p_name, typeof(RectTransform));
        l_label_object.transform.SetParent(p_parent, false);

        RectTransform l_rect_transform = l_label_object.GetComponent<RectTransform>();
        SetAnchorCenter(l_rect_transform, p_anchored_position, new Vector2(800.0f, 100.0f));

        TextMeshProUGUI l_text = l_label_object.AddComponent<TextMeshProUGUI>();
        l_text.text = p_text;
        l_text.fontSize = p_font_size;
        l_text.alignment = TextAlignmentOptions.Center;
        l_text.color = Color.white;

        return l_text;
    }

    private static TMP_Text CreateTitleLabel(Transform p_parent, string p_title_text)
    {
        return CreateLabel(p_parent, "TitleLabel", p_title_text, new Vector2(0.0f, 250.0f), 72);
    }

    private static Button CreateButton(Transform p_parent, string p_name, string p_label_text, Vector2 p_anchored_position)
    {
        GameObject l_button_object = new GameObject(p_name, typeof(RectTransform), typeof(Image), typeof(Button));
        l_button_object.transform.SetParent(p_parent, false);

        RectTransform l_rect_transform = l_button_object.GetComponent<RectTransform>();
        SetAnchorCenter(l_rect_transform, p_anchored_position, new Vector2(k_button_width, k_button_height));

        Image l_background_image = l_button_object.GetComponent<Image>();
        l_background_image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        TMP_Text l_label = CreateLabel(l_button_object.transform, "Label", p_label_text, Vector2.zero, 36);
        l_label.color = Color.black;
        RectTransform l_label_rect = l_label.GetComponent<RectTransform>();
        StretchToFillParent(l_label_rect);

        return l_button_object.GetComponent<Button>();
    }

    private static GameObject CreateExplanationPanel(Transform p_parent)
    {
        GameObject l_panel_object = new GameObject("ExplanationPanel", typeof(RectTransform));
        l_panel_object.transform.SetParent(p_parent, false);
        RectTransform l_panel_rect = l_panel_object.GetComponent<RectTransform>();
        StretchToFillParent(l_panel_rect);

        GameObject l_overlay_object = new GameObject("DarkOverlay", typeof(RectTransform), typeof(Image));
        l_overlay_object.transform.SetParent(l_panel_object.transform, false);
        StretchToFillParent(l_overlay_object.GetComponent<RectTransform>());
        l_overlay_object.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, k_overlay_alpha);

        GameObject l_illustration_object = new GameObject("TutorialIllustration", typeof(RectTransform), typeof(Image));
        l_illustration_object.transform.SetParent(l_panel_object.transform, false);
        RectTransform l_illustration_rect = l_illustration_object.GetComponent<RectTransform>();
        SetAnchorCenter(l_illustration_rect, new Vector2(0.0f, 40.0f), new Vector2(900.0f, 600.0f));
        l_illustration_object.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        Button l_confirm_button = CreateButton(l_panel_object.transform, "ConfirmButton", "Got it", new Vector2(0.0f, -400.0f));
        GameFlowManager l_flow_manager = FindOrAddComponent<GameFlowManager>();
        AddButtonListener(l_confirm_button, l_flow_manager, nameof(GameFlowManager.OnExplanationConfirmed));

        return l_panel_object;
    }

    private static void SetAnchorCenter(RectTransform p_rect_transform, Vector2 p_anchored_position, Vector2 p_size_delta)
    {
        p_rect_transform.anchorMin = new Vector2(0.5f, 0.5f);
        p_rect_transform.anchorMax = new Vector2(0.5f, 0.5f);
        p_rect_transform.pivot = new Vector2(0.5f, 0.5f);
        p_rect_transform.anchoredPosition = p_anchored_position;
        p_rect_transform.sizeDelta = p_size_delta;
    }

    private static void StretchToFillParent(RectTransform p_rect_transform)
    {
        p_rect_transform.anchorMin = Vector2.zero;
        p_rect_transform.anchorMax = Vector2.one;
        p_rect_transform.offsetMin = Vector2.zero;
        p_rect_transform.offsetMax = Vector2.zero;
    }

    private static void AddButtonListener(Button p_button, Object p_target, string p_method_name)
    {
        UnityEditor.Events.UnityEventTools.AddPersistentListener(p_button.onClick, System.Delegate.CreateDelegate(
            typeof(UnityEngine.Events.UnityAction),
            p_target,
            p_method_name) as UnityEngine.Events.UnityAction);
    }

    private static void AssignSerializedField(Object p_target, string p_field_name, Object p_value)
    {
        SerializedObject l_serialized_object = new SerializedObject(p_target);
        SerializedProperty l_property = l_serialized_object.FindProperty(p_field_name);

        if (l_property == null)
        {
            Debug.LogWarning($"フィールドが見つかりません: {p_target.GetType().Name}.{p_field_name}");
            return;
        }

        l_property.objectReferenceValue = p_value;
        l_serialized_object.ApplyModifiedProperties();
    }
}
