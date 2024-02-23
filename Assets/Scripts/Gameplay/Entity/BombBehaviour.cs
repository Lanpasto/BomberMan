using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : PlaceableEntityBehaviour
{
    [SerializeField] private FlameBeamBehaviour flameBeamBehaviour;
    
    public Action<PlaceableEntityBehaviour> OnExplode = delegate { };
    public UnitAttackBehavior attackBehavior { get; set; }
    
    private StatAttribute BombActivationTime => attackBehavior.unit.GetStat("BombActivationTime");
    private StatAttribute Pierce => attackBehavior.unit.GetStat("Pierce");
    private StatAttribute Radius => attackBehavior.unit.GetStat("Radius");

    public void SetSourceAttackBehaviour(UnitAttackBehavior attackBehavior)
    {
        this.attackBehavior = attackBehavior;
    }

    public override void Initialize(EntityDescription description, Vector2 coordinates)
    {
        base.Initialize(description, coordinates);
        GetComponent<SpriteRenderer>().sortingOrder += 1;
 
        Collider2D.isTrigger = CheckIfAnyPlayerIsInsideBombCollider();
    }

    private bool CheckIfAnyPlayerIsInsideBombCollider()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Collider2D.edgeRadius);
        foreach (Collider2D collider in colliders)
        {
            UnitBehaviour unit = collider.GetComponent<UnitBehaviour>();
            if (unit != null)
            {
                return true; 
            }
        }
        return false; 
    }
   
    private void OnDestroy()
    {
        OnExplode?.Invoke(this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        UnitBehaviour unit;
        if (other.TryGetComponent(out unit))
        {
            Collider2D.isTrigger = false;
        }
    }

    public void StartBombBehaviour()
    {
        StartCoroutine(StartBombCoroutine());
    }
    
    private IEnumerator StartBombCoroutine()
    {
        yield return new WaitForSeconds(BombActivationTime);
        var entitiesOnWay = MapManager.Instance.GetEntitiesOnWay(coordinates, (int)Pierce,(int)Radius);
        foreach (var entity in entitiesOnWay)
        {
             entity.TakeDamage();
        }

        flameBeamBehaviour.Initialize(this.coordinates, entitiesOnWay, 2f);
        Destroy(this.gameObject);
        yield return null;
    }
   
   
}