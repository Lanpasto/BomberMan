using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackBehavior : MonoBehaviour
{
    [SerializeField] private GameObject bomb;
    private UnitBehaviour unit;

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
            DeployBomb(unit.GeUnitPositionOnMap());
        
        }
    }

    private void DeployBomb(Vector2 position)
    {
        Instantiate(bomb, unit.GeUnitPositionOnMap(), Quaternion.identity);
        unit.GeUnitPositionOnMap();
        Debug.Log("Sykasss3 " + unit.GeUnitPositionOnMap());
    }

}
