using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    private void Awake()
    {
        Instance = this;
    }   

    private MapUnit[,] mapInfo;
    
    public void SetMap(MapUnit[,] generatedMap)
    { 
        mapInfo = new MapUnit[generatedMap.GetLength(0), generatedMap.GetLength(1)];
        mapInfo = generatedMap;
    }

    public MapUnit[,] GetMap()
    {
        return mapInfo;
    }

    public void RegisterNewBlock(EntityBehaviour entityBehaviour)
    {
        mapInfo[(int)entityBehaviour.coordinates.x, (int)entityBehaviour.coordinates.y].Add(entityBehaviour);
    }

    public void UnRegisterBlock(EntityBehaviour entityBehaviour)
    {
        mapInfo[(int)entityBehaviour.coordinates.x, (int)entityBehaviour.coordinates.y].Delete(entityBehaviour);
    }

    public MapUnit GetBlockBehaviour(Vector2 coordinates)
    {
        return mapInfo[(int)coordinates.x, (int)coordinates.y];
    }

    public Vector2 GetPositionByCoordinates(Vector2 coordinates)
    {
        return mapInfo[(int)coordinates.x, (int)coordinates.y].GetTransform();
    }


}

public class MapUnit
{
   public List<EntityBehaviour> unitObjects { get; set; }

    public MapUnit()
    {
        unitObjects = new List<EntityBehaviour>();
    }
    
    public void Add(EntityBehaviour behaviour)
    {
        unitObjects.Add(behaviour);
    }

    public void Delete(EntityBehaviour behaviour)
    {
        unitObjects.Remove(behaviour);
    }

    public EntityBehaviour Find(EntityBehaviour behaviour)
    {
        foreach (var obj in unitObjects)
        {
            if (obj == behaviour)
            {
                return behaviour;
            }
        }
        
        return null;
    }

    public bool Contains(EntityDescription description)
    {
        foreach (var obj in unitObjects)
        {
            if (obj.entityDescription == description)
            {
                return true;
            }
        }
        
        return false;
    }

    public List<EntityBehaviour> GetAll()
    {
        return unitObjects;
    }

    private EntityBehaviour GetAny()
    {
        if (unitObjects.Count > 0)
            return unitObjects[0];
        else
        {
            Debug.LogError("No objects in MapUnit");
            return null;
        }
    }

    public Vector3 GetTransform()
    {
        return GetAny().transform.position;
    }

    public Vector2 GetCoordinates()
    {
        return GetAny().coordinates;
    }
}