using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHandler : MonoBehaviour
{
    private float _attackRange = 2;
    private NPCMovement _movement;
    private CharacterController _controller;
    private CharacterCombat _combat;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _movement = GetComponent<NPCMovement>();
        _combat = GetComponent<CharacterCombat>();
    }

 public void BeginPhase()
    {
        _controller.FindNearestTarget();
        Transform targetTransform = _controller.Target.transform;
        if (_controller.CanOtherAction && Vector3.Distance(transform.position, targetTransform.position) < _attackRange)
        {
            print("ATTACK");
            _combat.BasicAttack(_controller, _controller.Target.GetComponent<CharacterController>());
            // Attack();
            _controller.CanOtherAction = false;
            // Remove later
            _controller.EndOtherActionPhase();
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
