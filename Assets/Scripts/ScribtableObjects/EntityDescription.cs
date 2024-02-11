using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EntityDescription", menuName = "ScriptableObjects/")]
public class EntityDescription : ScriptableObject
{
    public String nameBlock;
    public bool CanBeDamaged;
    public Sprite sprite;
    public bool HasCollider2D;
    public bool _static;

}