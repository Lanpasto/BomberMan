using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceableEntityBehaviour : EntityBehaviour
{
    public bool breakeable { get; set; }
    public string nameID { get; set; }
    protected BoxCollider2D Collider2D { get; set; }

    private void Awake()
    {
        Collider2D = GetComponent<BoxCollider2D>();
    }

    public override void Initialize(EntityDescription description, Vector2 coordinates)
    {
        
        Collider2D.enabled = description.HasCollider2D;
        GetComponent<SpriteRenderer>().sprite = description.sprite;
        this.gameObject.isStatic = description._static;
        this.name = description.nameBlock;
        this.nameID = description.nameBlock;
        
        base.Initialize(description, coordinates);
    }

    private void OnDestroy()
    {
        if(MapManager.Instance != null)
            MapManager.Instance.UnRegisterBlock(coordinates);
    }
}