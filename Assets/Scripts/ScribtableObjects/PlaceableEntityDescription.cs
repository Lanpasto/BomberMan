using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableEntityDescription", menuName = "ScriptableObjects/PlaceableEntityDescription")]
public class PlaceableEntityDescription : ScriptableObject
{
    public String nameBlock;
    public bool IsBreakable;
    public Sprite sprite;
    public bool HasCollider2D;
    public bool _static;

}
