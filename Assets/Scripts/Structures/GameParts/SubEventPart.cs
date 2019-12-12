﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SubEventPart : ScriptableObject
{
    /// <summary> Базовый текст </summary>
    public string mainText;

    /// <summary> Победа в событии </summary>
    public bool isFinal;

    /// <summary> Провал события </summary>
    public bool isFail;

    /// <summary> Глава при переходе влево </summary>
    public SubEventPart moveLeft;

    /// <summary> Глава при переходе вправо </summary>
    public SubEventPart moveRight;

#if UNITY_EDITOR

    public bool windowSizeStady = false;
    public Rect windowRect;
    public float openedHeight = 120f;
    public string windowTitle;
    public string comment;

#endif

}

#if UNITY_EDITOR

namespace GUIInspector
{
    [CustomEditor(typeof(SubEventPart))]
    public class SubEventPartGUI_Inspector : Editor
    {
        private SubEventPart _subEventPart;

        private void OnEnable() => _subEventPart = (SubEventPart)target;

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Текстовое поле");

            GUILayout.BeginVertical("Box");

            _subEventPart.mainText = EditorGUILayout.TextArea(_subEventPart.mainText, GUILayout.Height(100));

            GUILayout.BeginHorizontal();

            if(!_subEventPart.isFail && !_subEventPart.isFinal)
            {
                if (_subEventPart.moveLeft != null)
                {
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Отключить", GUILayout.Height(20))) _subEventPart.moveLeft = null;
                    GUI.backgroundColor = Color.white;
                }
                else GUILayout.Label("Не подключено", "Button", GUILayout.Height(20));

                if (_subEventPart.moveRight != null)
                {
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Отключить", GUILayout.Height(20))) _subEventPart.moveRight = null;
                    GUI.backgroundColor = Color.white;
                }
                else GUILayout.Label("Не подключено", "Button", GUILayout.Height(20));
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
    }
}

#endif