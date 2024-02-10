using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    [HideInInspector] 
    public Vector2 coordinates;
    
    public bool breakeable;
    public void InitializeBlock(BlockDeScription description, Vector2 coordinates)
    {
        this.coordinates = coordinates;
        GetComponent<BoxCollider2D>().enabled = description.HasCollider2D;
        GetComponent<SpriteRenderer>().sprite = description.sprite;
        this.gameObject.isStatic = description._static;
        this.name = description.nameBlock;
    }

}