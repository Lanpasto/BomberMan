using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitControlBehaviour : MonoBehaviour
{
    public Action<InputManager.PlayerAction> OnAction;
    
    public PlayerInputSO PlayerInputKeys;
    
    public UnitBehaviour unitBehaviour;

    private Rigidbody2D rb;
    private StatAttribute Speed => unitBehaviour.GetStat("Speed");
    
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        InputManager.InputAction += InputAction;
    }
    
    private void OnDisable()
    {
        InputManager.InputAction -= InputAction;
    }

    private void InputAction(KeyCode keyCode)
    {
        foreach (var bind in PlayerInputKeys.PlayerBinds)
        {
            if (bind.key == keyCode)
            {
                UnitActionBehaviour(bind.action);
            }
        }
    }
    
    private void UnitActionBehaviour(InputManager.PlayerAction action)
    {
        OnAction?.Invoke(action);
        
        switch (action)
        {
            case InputManager.PlayerAction.bomb:
                DropBomb();
                break;
            case InputManager.PlayerAction.up:
                MoveUnit(Vector2.up);
                break;
            case InputManager.PlayerAction.down:
                MoveUnit(Vector2.down);
                break;
            case InputManager.PlayerAction.left:
                MoveUnit(Vector2.left);
                break;
            case InputManager.PlayerAction.right:
                MoveUnit(Vector2.right);
                break;
        }
    }

    private void MoveUnit(Vector2 direction)
    {
        var position = (direction * 0.001f * Speed.Value) + rb.position;
        Debug.Log(position);
        rb.MovePosition(position);
    }
    

    private void DropBomb()
    {
    }

}