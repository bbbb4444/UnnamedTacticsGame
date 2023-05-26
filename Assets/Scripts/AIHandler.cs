using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHandler : MonoBehaviour
{
    private float _attackRange = 2;
    private NPCMovement _movement;
    private CharacterController _controller;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _movement = GetComponent<NPCMovement>();
    }
    public void BeginTurn()
    {
        _controller.FindNearestTarget();
        Transform targetTransform = _controller.Target.transform;
        if (Vector3.Distance(transform.position, targetTransform.position) < _attackRange)
        {
            print("ATTACK");
            _controller.BasicAttack(_controller, _controller.Target.GetComponent<CharacterController>());
            // Attack();
            _controller.CanOtherAction = false;
            // Remove later
            _controller.RemoveAction(1);
        }
        else if (_controller.CanMove)
        {
            print(_movement.tag);
            _movement.MoveToTarget(_controller.Target);
            print(_controller.Target);
            print("controller target" + _controller.Target);
        }
        else _controller.RemoveAction(1);
    }
}
