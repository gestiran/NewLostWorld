﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New part", menuName = "Игровые обьекты/Новая глава/Глава боя", order = 2)]
public class BattlePart : GamePart
{
    /// <summary> Текст первой кнопки </summary>
    public string buttonText_1;

    /// <summary> Текст второй кнопки </summary>
    public string buttonText_2;

    /// <summary> Текст третей кнопки </summary>
    public string buttonText_3;
}

#if UNITY_EDITOR

namespace GUIInspector
{

    [CustomEditor(typeof(BattlePart))]
    public class BattlePartGUI_Inspector : Editor
    {
        private BattlePart _battlePart;

        private void OnEnable() => _battlePart = (BattlePart)target;

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Текстовые поля");

            GUILayout.BeginVertical("Box");

            _battlePart.mainText = EditorGUILayout.TextArea(_battlePart.mainText, GUILayout.Height(100));
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            _battlePart.buttonText_1 = EditorGUILayout.TextArea(_battlePart.buttonText_1, GUILayout.Height(40));
            _battlePart.movePart_1 = (GamePart)EditorGUILayout.ObjectField(_battlePart.movePart_1, typeof(GamePart), true, GUILayout.Width(80));
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            _battlePart.buttonText_2 = EditorGUILayout.TextArea(_battlePart.buttonText_2, GUILayout.Height(40));
            _battlePart.movePart_2 = (GamePart)EditorGUILayout.ObjectField(_battlePart.movePart_2, typeof(GamePart), true, GUILayout.Width(80));
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            _battlePart.buttonText_3 = EditorGUILayout.TextArea(_battlePart.buttonText_3, GUILayout.Height(40));
            _battlePart.movePart_3 = (GamePart)EditorGUILayout.ObjectField(_battlePart.movePart_3, typeof(GamePart), true, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.Label("Параметры");

            if (_battlePart.mainEvents != null) GlobalHelperGUI_Inspector.ShowPartEventList(_battlePart.mainEvents);
            else _battlePart.mainEvents = new System.Collections.Generic.List<GameEvent>();
        }
    }
}

#endif