using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

<<<<<<<< Updated upstream:Assets/ScriptsArtem/BlockDeScription.cs
[CreateAssetMenu(fileName = "BlockDescription", menuName = "ScriptableObjects/BlockDescription")]
public class BlockDeScription : ScriptableObject
========
[CreateAssetMenu(fileName = "EntityDescription", menuName = "ScriptableObjects/")]
public class EntityDescription : ScriptableObject
>>>>>>>> Stashed changes:Assets/Scripts/ScribtableObjects/EntityDescription.cs
{
    public String nameBlock;
    public bool CanBeDamaged;
    public Sprite sprite;
    public bool HasCollider2D;
    public bool _static;

}
