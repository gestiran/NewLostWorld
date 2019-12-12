﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GUIInspector.NodeEditor
{
    public class EventEditor : EditorWindow
    {
        public static EventEditor eventEditor;
        public static Event eventThis;

        private Vector2 _drag;
        private SubEventPart _selectedNode;
        private SubEventPart _sellectedToConnect;
        private int _sellectionId;
        Texture _emptyTexture;
        public int tempConnect = 0;

        /// <summary> Данные графа Эвента </summary>
        public static EventPart eventGraph;

        public enum InputEnum
        {
            ADD_NEW_EVENT_SUB_PART,
            ADD_NEW_EVENT_FINAL,
            ADD_NEW_EVENT_FAIL
        }

        public static void ShowWindow()
        {
            eventEditor.UserEventInput(eventThis);
            if(eventEditor._emptyTexture == null)
                eventEditor._emptyTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeEditor/Images/Connect.png", typeof(Texture));

            eventEditor.DrawEventWindows();
            GUI.backgroundColor = Color.white;
            eventEditor.DrawEventConnectors();
        }

        /// <summary> Отрисовка окон в редакторе Евентов </summary>
        private void DrawEventWindows()
        {
            if (eventGraph.eventParts == null) eventGraph.eventParts = new List<SubEventPart>();

            if (_sellectedToConnect != null)
            {
                CreateCurve(ConnectPosition(_sellectedToConnect, tempConnect),
                        new Rect(BehaviorEditor._mousePosition, new Vector2(0, 0)), Color.blue);
                BehaviorEditor.trBehaviorEditor.Repaint();
            }

            BeginWindows();

            BehaviorEditor.trBehaviorEditor.Repaint();

            if (eventGraph.eventParts.Count > 0)
            {
                for (int i = 0; i < eventGraph.eventParts.Count; i++)
                {
                    if (eventGraph.eventParts[i] == null)
                    {
                        eventGraph.eventParts.RemoveAt(i);
                        break;
                    }

                    if (eventGraph.eventParts[i].isFail) GUI.backgroundColor = Color.red;
                    else if (eventGraph.eventParts[i].isFinal) GUI.backgroundColor = Color.green;
                    else if (i == 0) GUI.backgroundColor = Color.yellow;
                    else GUI.backgroundColor = Color.white;

                    eventGraph.eventParts[i].windowRect = GUI.Window(
                             i,
                             eventGraph.eventParts[i].windowRect,
                             DrawSubEventWindow,
                             eventGraph.eventParts[i].name, BehaviorEditor.storyData.graphSkin.GetStyle("Window"));

                    DrawCurve(eventGraph.eventParts[i]);
                }
            }

            EndWindows();
        }

        /// <summary> Нажатие на коннектор </summary>
        private void ConnectorClick(int id, SubEventPart part)
        {
            _sellectedToConnect = part;
            _sellectionId = id;
        }

        /// <summary> Отрисовка отдельной ноды </summary>
        private void DrawSubEventWindow(int id)
        {
            EditorGUILayout.Space();
            eventGraph.eventParts[id].comment = EditorGUILayout.TextArea(
                        eventGraph.eventParts[id].comment, BehaviorEditor.storyData.graphSkin.GetStyle("TextArea"),
                        GUILayout.Width(100f),
                        GUILayout.Height(80));

            GUI.DragWindow();
        }

        /// <summary> Отрисовка кривой </summary>
        private void DrawCurve(SubEventPart part)
        {
            Color baseConnectColor = new Color(1, 1, 1, 0.75f);

            if (part.moveLeft != null)
            {
                if (eventGraph.eventParts.Contains(part.moveLeft) && part.moveLeft != this)
                    CreateCurve(ConnectPosition(part, 0), part.moveLeft.windowRect, baseConnectColor);
            }

            if (part.moveRight != null)
            {
                if (eventGraph.eventParts.Contains(part.moveRight) && part.moveRight != this)
                    CreateCurve(ConnectPosition(part, 2), part.moveRight.windowRect, baseConnectColor);
            }
        }

        /// <summary> Позиция старта </summary>
        private Rect ConnectPosition(SubEventPart part, int id)
        {
            Rect nodeConnectPosition = new Rect(
                    part.windowRect.x + part.windowRect.width + 5,
                    (part.windowRect.y + 2) + (13 * id),
                    11,
                    11);

            return nodeConnectPosition;
        }

        /// <summary> Создать кривую </summary>
        private void CreateCurve(Rect start, Rect end, Color colorCurve)
        {
            Vector3 startPos = new Vector3(
                start.x + start.width,
                start.y + (start.height * .5f),
                0);

            Vector3 endPos = new Vector3(
                end.x,
                end.y + (end.height * .5f),
                0);

            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;

            Handles.DrawBezier(startPos, endPos, startTan, endTan, colorCurve, null, 3f);
        }

        /// <summary> Отрисовка соединений в редакторе событий </summary>
        private void DrawEventConnectors()
        {
            for (int i = 0; i < eventGraph.eventParts.Count; i++)
            {
                if (eventGraph.eventParts[i].isFail) continue;
                else if (eventGraph.eventParts[i].isFinal) continue;

                if (GUI.Button(ConnectPosition(eventGraph.eventParts[i], 0), _emptyTexture, BehaviorEditor.storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(0, eventGraph.eventParts[i]);
                    tempConnect = 0;
                }

                if (GUI.Button(ConnectPosition(eventGraph.eventParts[i], 2), _emptyTexture, BehaviorEditor.storyData.graphSkin.FindStyle("Button")))
                {
                    ConnectorClick(1, eventGraph.eventParts[i]);
                    tempConnect = 2;
                }

            }
        }

        #region USER_INPUT

        /// <summary> Ввод пользователя </summary>
        private void UserEventInput(Event e)
        {
            if (e.button == 1) { if (e.type == EventType.MouseDown) AddEventToPart(e); }
            if (e.button == 0) { if (e.type == EventType.MouseDown) LeftMouseClick(); }
            if (e.type == EventType.MouseDrag) { if (e.button == 2) eventEditor.OnDrag(e.delta); }
            if (e.type == EventType.KeyDown) { if (e.keyCode == KeyCode.Delete) DeleteKeyDown(); }
        }

        /// <summary> Добавить событие к главе </summary>
        private void AddEventToPart(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Событие/Основное"), false, AddNewEventSubPart, InputEnum.ADD_NEW_EVENT_SUB_PART);
            menu.AddItem(new GUIContent("Событие/Победа"), false, AddNewEventSubPart, InputEnum.ADD_NEW_EVENT_FINAL);
            menu.AddItem(new GUIContent("Событие/Провал"), false, AddNewEventSubPart, InputEnum.ADD_NEW_EVENT_FAIL);
            menu.ShowAsContext();
            e.Use();
        }

        /// <summary> Добавить под-евент </summary>
        private void AddNewEventSubPart(object o)
        {
            InputEnum a = (InputEnum)o;

            if (!AssetDatabase.IsValidFolder("Assets/Resources/GameParts/" + eventGraph.name))
            {
                AssetDatabase.CreateFolder("Assets/Resources/GameParts", eventGraph.name);
            }

            string nameNewEvent = Resources.LoadAll("GameParts/" + eventGraph.name).Length + "_" + eventGraph.name;

            AssetDatabase.CreateAsset(CreateInstance(typeof(SubEventPart)),
                "Assets/Resources/GameParts/" + eventGraph.name + "/" + nameNewEvent + ".asset");

            SubEventPart subPart = (SubEventPart)Resources.Load("GameParts/" + eventGraph.name + "/" + nameNewEvent, typeof(SubEventPart));

            switch (a)
            {
                case InputEnum.ADD_NEW_EVENT_FAIL: subPart.isFail = true; break;
                case InputEnum.ADD_NEW_EVENT_FINAL: subPart.isFinal = true; break;
            }

            subPart.windowRect = new Rect(
                BehaviorEditor._mousePosition.x,
                BehaviorEditor._mousePosition.y,
                BehaviorEditor.storyData.baseNodeLgWidth,
                BehaviorEditor.storyData.baseNodeCommentHeight);

            eventGraph.eventParts.Add(subPart);
        }

        /// <summary> Левый клик мыши </summary>
        private void LeftMouseClick()
        {
            for (int i = 0; i < eventGraph.eventParts.Count; i++)
            {
                if (eventGraph.eventParts[i] != null)
                {
                    if (eventGraph.eventParts[i].windowRect.Contains(BehaviorEditor._mousePosition))
                    {
                        if (_sellectedToConnect != null)
                        {
                            switch (_sellectionId)
                            {
                                case 0: _sellectedToConnect.moveLeft = eventGraph.eventParts[i]; break;
                                case 1: _sellectedToConnect.moveRight = eventGraph.eventParts[i]; break;
                            }

                            _sellectedToConnect = null;
                        }
                        else
                        {
                            _selectedNode = eventGraph.eventParts[i];
                            Selection.activeObject = eventGraph.eventParts[i];
                        }
                        break;
                    }
                }
            }
        }

        /// <summary> Кнопка Delete </summary>
        private void DeleteKeyDown()
        {
            if (_selectedNode != null)
            {
                eventGraph.eventParts.Remove(_selectedNode);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_selectedNode));
                _selectedNode = null;
                GraphChangeController.selectedNode = null;
                SaveData();
                BehaviorEditor.trBehaviorEditor.Repaint();
            }
        }

        /// <summary> Панорамирование поля </summary>
        private void OnDrag(Vector2 delta)
        {
            _drag = delta;

            for (int i = 0; i < eventGraph.eventParts.Count; i++)
            {
                eventGraph.eventParts[i].windowRect.position += _drag;
                BehaviorEditor._offset += delta / eventGraph.eventParts.Count;
            }

            GUI.changed = true;
        }

        /// <summary> Сохранение данных Евента </summary>
        private void SaveData()
        {
            for (int i = 0; i < eventGraph.eventParts.Count; i++)
            {
                if (eventGraph.eventParts[i] == null)
                {
                    eventGraph.eventParts.RemoveAt(i);
                    SaveData();
                    return;
                }
            }

            for (int i = 0; i < eventGraph.eventParts.Count; i++)
            {
                EditorUtility.SetDirty(eventGraph.eventParts[i]);
            }

            EditorUtility.SetDirty(eventGraph);
        }

        #endregion
    }
}