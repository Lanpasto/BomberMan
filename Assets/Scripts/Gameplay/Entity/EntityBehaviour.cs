using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    public Vector2 coordinates { get; set; }
    public EntityDescription entityDescription { get; set; }

    public virtual void Initialize(EntityDescription description, Vector2 coordinates)
    {
        this.entityDescription = description;
        this.coordinates = coordinates;
    }
}