using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackBehavior : MonoBehaviour
{
    [SerializeField] private GameObject bomb;
    [SerializeField] private EntityDescription bombDescription;
    
 
    public UnitBehaviour unit { get; set; }
    private StatAttribute bombAmount => unit.GetStat("Bomb Amount");
    
      private void OnEnable()
    {
        unit = GetComponent<UnitBehaviour>();
        unit.unitControlBehaviour.OnAction += UnitActions;

    }

    private void OnDisable()
    {
        unit.unitControlBehaviour.OnAction -= UnitActions;
    }
    private void UnitActions(InputManager.PlayerAction action)
    {
       
        if (action == InputManager.PlayerAction.bomb)
        {
            StartCoroutine(DeployBomb());
        
        }
    }

    private int currentBombCount = 0;
    private float timeElapsed = 0;
    private float lastLaunchTime = -Mathf.Infinity;
    private static float deployCoolDown = .09f;
    

    private IEnumerator DeployBomb()
    {
        var coordinates = unit.coordinates;
        yield return new WaitForSeconds(0.11f);
        
        if (currentBombCount < bombAmount.Value)
        {
            timeElapsed = Time.time - lastLaunchTime;

            if (MapManager.Instance.GetBlockBehaviour(coordinates).name != bombDescription.nameBlock && timeElapsed > deployCoolDown)
            {
                lastLaunchTime = Time.time;
                
                GameObject _bomb = Instantiate(bomb,coordinates , Quaternion.identity);
                var bombBehaviour = _bomb.GetComponent<BombBehaviour>();
                bombBehaviour.SetSourceAttackBehaviour(this);
                bombBehaviour.Initialize(bombDescription, coordinates);
                bombBehaviour.OnExplode += OnBombExplode;
                currentBombCount++;

                bombBehaviour.StartBombBehaviour();
                
                MapManager.Instance.RegisterNewBlock(bombBehaviour);
            }
        }

        yield return null;
    }

    private void OnBombExplode(PlaceableEntityBehaviour placeableEntityBehaviour)
    {
        MapManager.Instance.UnRegisterBlock(placeableEntityBehaviour.coordinates);
        currentBombCount--;
    }
}
