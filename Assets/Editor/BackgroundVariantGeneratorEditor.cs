using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CustomEditor(typeof(BackgroundVariantGenerator))]
public class BackgroundVariantGeneratorEditor : Editor {
    // Properties for direct manipulation
    SerializedProperty _seed;
    SerializedProperty _sun;

    // Background type index
    private int _background = 1;
    public int background {
        get => Mathf.Clamp(_background, 1, script.rotations.Count);
        set => _background = Mathf.Clamp(value, 1, script.rotations.Count);
    }

    private int _lastEditedBackground = 1;
    public int lastEditedBackground {
        get => Mathf.Clamp(_lastEditedBackground, 1, script.rotations.Count);
        set => _lastEditedBackground = Mathf.Clamp(value, 1, script.rotations.Count);
    }

    public int level = 1;
    private BackgroundVariantGenerator script;

    private void OnEnable() {
        _seed = serializedObject.FindProperty("seed");
        _sun = serializedObject.FindProperty("sun");

        script = (BackgroundVariantGenerator)target;
    }

    public override void OnInspectorGUI() {
        int tinySpace = 10;
        int smallSpace = 20;
        int bigSpace = 30;

        // Update the serialized data
        serializedObject.Update();

        // Setup for the editor ------------------------------------------------
        EditorGUILayout.LabelField("Run the pre-render option!");
        EditorGUILayout.LabelField("If the Assets/Backgrounds folder doesn't exist, create one manually.");

        EditorGUILayout.Space(bigSpace);

        GUILayout.BeginVertical("Setup", "window");
        EditorGUILayout.PropertyField(_sun);
        EditorGUILayout.PropertyField(_seed);
        GUILayout.EndVertical();

        EditorGUILayout.Space(smallSpace);

        // Background editor ---------------------------------------------------
        GUILayout.BeginVertical("Backgrounds", "window");

        // - Slider for choosing the background
        EditorGUI.BeginChangeCheck();
        background = EditorGUILayout.IntSlider(background, 1, script.rotations.Count);
        if (EditorGUI.EndChangeCheck()) {
            script.setRotationType(background - 1);
        }

        EditorGUILayout.Space(tinySpace);

        // - Adding a new background
        if (GUILayout.Button("Add")) {
            Undo.RecordObject(target, "Add background");

            script.add();

            background = script.rotations.Count;

            script.save(background - 1);

            script.setRotationType(background - 1);
        }

        // - Generating random settings
        if (GUILayout.Button("Generate")) {
            Undo.RecordObject(target, "Generate background");

            script.generate();
            script.save(background - 1);

            lastEditedBackground = background;
        }

        EditorGUILayout.Space(tinySpace);

        // - Adjust the euler angles of the sun direction
        EditorGUI.BeginChangeCheck();
        script.euler = EditorGUILayout.Vector3Field("Adjust sun direction", script.euler);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(target, "Adjust sun direction");

            script.save(background - 1);

            lastEditedBackground = background;
        }

        EditorGUILayout.Space(tinySpace);

        // - Remove the chosen entry
        if (GUILayout.Button("Remove")) {
            if (EditorUtility.DisplayDialog("Remove this background?",
                        "Do you really want to remove this background?",
                        "Yes", "No")) {
                Undo.RecordObject(target, "Remove background");

                script.remove(background - 1);

                script.setRotationType(background - 1);
            }
            GUIUtility.ExitGUI();
        }

        GUILayout.EndVertical();

        EditorGUILayout.Space(smallSpace);

        // Rendering -----------------------------------------------------------
        GUILayout.BeginVertical("Rendering", "window");

        if (GUILayout.Button("Pre-render")) {
            EditorCoroutineUtility.StartCoroutine(saveBackgrounds(), this);
        }
        if (GUILayout.Button("Apply textures")) {
            findBackgroundTextures();
        }

        GUILayout.EndVertical();

        EditorGUILayout.Space(bigSpace);

        // Utility for checking the background order ---------------------------
        GUILayout.BeginVertical("Background order by level", "window");

        EditorGUI.BeginChangeCheck();
        level = Mathf.Max(EditorGUILayout.IntField("Level", level), 1);
        if (EditorGUI.EndChangeCheck()) {
            script.backgroundPerLevel.Clear();
            script.rotationAtLevel(level);
            script.setRotation(level);
        }

        GUILayout.EndVertical();

        // Sove the serialized data and update editor on save/command
        if (serializedObject.ApplyModifiedProperties() || undoOrRedoPerformed) {
            background = lastEditedBackground;
            script.setRotationType(background - 1);
        }
    }

    // Background serialization / rendering ------------------------------------
    public void findBackgroundTextures() {
        Undo.RecordObject(target, "Apply background textures");

        script.backgroundTextures = new List<Texture2D>();

        for (int i = 0; i < script.rotations.Count; ++i) {
            script.backgroundTextures.Add(
                    (Texture2D)AssetDatabase.LoadAssetAtPath(
                   "Assets/Backgrounds/background-" + (i + 1) + ".png",
                   typeof(Texture2D)));
        }
    }

    IEnumerator saveBackgrounds() {
        for (int i = 0; i < script.rotations.Count; ++i) {
            EditorUtility.DisplayProgressBar("Background pre-render",
                    "Rendering background " + (i + 1) + " of " + script.rotations.Count,
                    (float)i / script.rotations.Count);
            background = i + 1;
            script.setRotationType(background - 1);
            saveRenderTextureToPng("Assets/Backgrounds/background-" + (i + 1) + ".png");
            yield return null;
        }
        EditorUtility.ClearProgressBar();

        findBackgroundTextures();
    }

    public void saveRenderTextureToPng(string filename) {
        // Refresh post-processing (for AO problems)
        var postA = GameObject.Find("Background To Texture Camera A").GetComponent<PostProcessLayer>();
        var postB = GameObject.Find("Background To Texture Camera B").GetComponent<PostProcessLayer>();
        postA.enabled = false;
        postA.enabled = true;
        postB.enabled = false;
        postB.enabled = true;

        Camera camera = GameObject.Find("Background To Texture Camera A")
            .GetComponent<Camera>();
        RenderTexture rt = camera.targetTexture;

        RenderTexture tempRt = new RenderTexture(rt.width, rt.height, rt.depth,
                RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        tempRt.antiAliasing = rt.antiAliasing;

        var previousRenderTexture = RenderTexture.active;
        RenderTexture.active = tempRt;

        camera.targetTexture = tempRt;
        camera.Render();

        Texture2D tex = new Texture2D(tempRt.width, tempRt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, tempRt.width, tempRt.height), 0, 0);

        RenderTexture.active = previousRenderTexture;

        System.IO.File.WriteAllBytes(filename, tex.EncodeToPNG());
        AssetDatabase.ImportAsset(filename);

        Debug.Log("Saved render texture to " + filename);
    }

    // Utilities
    private bool undoOrRedoPerformed {
        get => Event.current.type == EventType.ExecuteCommand &&
            Event.current.commandName == "UndoRedoPerformed";
    }
}
