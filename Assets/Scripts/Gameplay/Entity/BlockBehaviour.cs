using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    public Vector2 coordinates { get; set; }
    
    public bool breakeable { get; set; }
    public string nameID { get; set; }
    protected BoxCollider2D Collider2D { get; set; }

    private void Awake()
    {
        Collider2D = GetComponent<BoxCollider2D>();
    }

    public virtual void Initialize(BlockDeScription description, Vector2 coordinates)
    {
        this.coordinates = coordinates;
        Collider2D.enabled = description.HasCollider2D;
        GetComponent<SpriteRenderer>().sprite = description.sprite;
        this.gameObject.isStatic = description._static;
        this.name = description.nameBlock;
        this.nameID = description.nameBlock;
    }

}