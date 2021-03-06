﻿using Helpers;
using UnityEditor;
using UnityEngine;

namespace Data.GameParts
{
    public class FinalPart : GamePart
    {
        /// <summary> Получаемое достижение, если нужно </summary>
        public Achivemants newAchive;

        /// <summary> Текст кнопки возврата в меню </summary>
        public string backButtonText;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(FinalPart))]
    public class FinalPartGUInspector : Editor
    {
        private FinalPart _finalPart;
        private static int id = 0;

        private void OnEnable() => _finalPart = (FinalPart)target;

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Текстовое поле");

            GUILayout.BeginVertical("Box");

            _finalPart.mainText = EditorGUILayout.TextArea(_finalPart.mainText, GUILayout.Height(100));
            EditorGUILayout.Space();

            _finalPart.backButtonText = EditorGUILayout.TextArea(_finalPart.backButtonText, GUILayout.Height(40));

            GUILayout.EndVertical();

            EditorGUILayout.BeginVertical();

            object[] allAchives = Resources.LoadAll("Achivemants/", typeof(Achivemants));

            string[] names = new string[allAchives.Length];

            Achivemants nameConvert;

            for (int i = 0; i < names.Length; i++)
            {
                nameConvert = (Achivemants)allAchives[i];

                names[i] = (nameConvert.achiveName == "") ? nameConvert.name : nameConvert.achiveName;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));

            if (allAchives.Length > 0)
            {
                id = EditorGUILayout.Popup(id, names);
                _finalPart.newAchive = (Achivemants)allAchives[id];
            }
            else GUILayout.Label("Нет достижений");

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Создать", GUILayout.Width(70)))
                AssetDatabase.CreateAsset(CreateInstance(typeof(Achivemants)), "Assets/Resources/Achivemants/" + allAchives.Length + "_Achive.asset");

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            if (_finalPart.newAchive != null) AchivemantsGUInspector.ShowAchiveGUI(_finalPart.newAchive);

            EditorGUILayout.EndVertical();
        }
    }

#endif
}