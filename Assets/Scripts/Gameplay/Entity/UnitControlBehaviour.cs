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

    private Vector2 positionToFollow;
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

        Vector2 possibleMoveVector = Vector2.zero;
        
        if (InputManager.PlayerAction.bomb == action)
        {
            DropBomb();
        }
        if (InputManager.PlayerAction.down == action)
        {
            possibleMoveVector += Vector2.down;
        }
        if (InputManager.PlayerAction.left == action)
        {
            possibleMoveVector += Vector2.left;
        }
        if (InputManager.PlayerAction.up == action)
        {
            possibleMoveVector += Vector2.up;
        }
        if (InputManager.PlayerAction.right == action)
        {
            possibleMoveVector += Vector2.right;
        }
        
        MoveUnit(possibleMoveVector);
    }

    private void MoveUnit(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            Vector2 force = direction.normalized * Speed.Value;
            rb.AddForce(force, ForceMode2D.Impulse);
            float maxVelocity = 2f * Speed.Value;
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
            positionToFollow = rb.position;
        }
    }


    

    private void DropBomb()
    {
    }

}