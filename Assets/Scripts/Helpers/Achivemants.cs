﻿using UnityEngine;

[CreateAssetMenu(fileName = "New achivemant",menuName = "Игровые обьекты/Новое достижение")]
public class Achivemants : ScriptableObject
{
    // Описывает игровое достижение

    /// <summary>
    /// Значек достижения
    /// </summary>
    public Sprite achiveIco;

    /// <summary>
    /// Название достижения
    /// </summary>
    public string achiveName;

    /// <summary>
    /// Описание достижения
    /// </summary>
    public string achiveDescript;
}