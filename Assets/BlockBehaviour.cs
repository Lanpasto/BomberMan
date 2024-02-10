using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    public bool breakeable;
    public void InitializeBlock(BlockDeScription deScription)
    {

        GetComponent<BoxCollider2D>().enabled = deScription.HasCollider2D;
        GetComponent<SpriteRenderer>().sprite = deScription.sprite;
        this.gameObject.isStatic = deScription._static;
        this.name = deScription.nameBlock;
    }

}
// bool breakeable = deScription.IsBreakable;
// bool HasCollider2D = deScription.HasCollider2D;
// bool _static = deScription._static;
// Sprite sprite = deScription.sprite;
// String nameBlock = deScription.nameBlock;