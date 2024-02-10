using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockDescription", menuName = "ScriptableObjects/BlockDescription")]
public class BlockDeScription : ScriptableObject
{
    public String nameBlock;
    public bool IsBreakable;
    public Sprite sprite;
    public bool HasCollider2D;
    public bool _static;

}
