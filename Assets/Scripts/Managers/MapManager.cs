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
        if(entityBehaviour != null)
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

    public EntityBehaviour GetCoordinatesByBlock(EntityBehaviour ent)
    {
        foreach (var mapunit in mapInfo)
        {
            foreach (var unit in mapunit.unitObjects)
            {
                if (ent == unit)
                {
                    return unit;
                }
            }
        }

        return null;
    }

    public List<EntityBehaviour> GetEntitiesOnWay(Vector2 currentCoordinate, int pierce, bool allDirections = true)
    {
        List<EntityBehaviour> allEntityBehaviourOnWay = new List<EntityBehaviour>();
        if(allDirections)
        {
            allEntityBehaviourOnWay.AddRange(GetEntitiesOnWay(currentCoordinate,pierce,Vector2.down));
            allEntityBehaviourOnWay.AddRange(GetEntitiesOnWay(currentCoordinate,pierce,Vector2.up));
            allEntityBehaviourOnWay.AddRange(GetEntitiesOnWay(currentCoordinate,pierce,Vector2.left));
            allEntityBehaviourOnWay.AddRange(GetEntitiesOnWay(currentCoordinate,pierce,Vector2.right));
        }

        return allEntityBehaviourOnWay;
    }

    private List<EntityBehaviour> GetEntitiesOnWay(Vector2 currentCoordinate, int pierce, Vector2 direction)
    {
        int pierceCount = pierce;
        
        int x = (int)direction.x;
        int y = (int)direction.y;

        List<EntityBehaviour> entities = new List<EntityBehaviour>();
        
        while(true)
        {
            if ((int)currentCoordinate.x + x >= 0 && (int)currentCoordinate.x + x < mapInfo.GetLength(0) &&
                (int)currentCoordinate.y + y >= 0 && (int)currentCoordinate.y + y < mapInfo.GetLength(1))
            {
                var mapUnit = mapInfo[(int)currentCoordinate.x + x, (int)currentCoordinate.y + y];
                bool hasFitEntity = false;
                foreach (var entity in mapUnit.GetAll())
                {
                    if (entity.entityDescription.HasCollider2D)
                    {
                        if (entity.entityDescription.CanBeDamaged)
                        {
                            entities.Add(entity);
                            hasFitEntity = true;
                        }
                        else
                        {
                            if (entity.entityDescription.CanStopExplodeExpansion)
                            {
                                pierceCount = 0;
                                break;
                            }
                        }
                    }
                }
                if (hasFitEntity)
                    pierceCount--;
                
                if (pierceCount <= 0)
                    break;
                
                x += (int)direction.x;
                y += (int)direction.y;
            }
            else
                break;
        }
        return entities;
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