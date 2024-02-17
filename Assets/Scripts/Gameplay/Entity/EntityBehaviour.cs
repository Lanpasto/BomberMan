using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    public Vector2 coordinates { get; set; }
    public EntityDescription entityDescription { get; set; }
    
    private bool isQuit = false;

    public virtual void Initialize(EntityDescription description, Vector2 coordinates)
    {
        this.entityDescription = description;
        this.coordinates = coordinates;
    }

    public virtual void TakeDamage()
    {
        MapManager.Instance.UnRegisterBlock(this);
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        if(MapManager.Instance != null && !isQuit)
            MapManager.Instance.UnRegisterBlock(this);
    }
    
    private void OnApplicationQuit()
    {
        isQuit = true;
    }
}