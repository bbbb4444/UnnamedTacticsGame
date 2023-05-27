using System;
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

    private void Update()
    {
 
    }

    public IEnumerator BeginPhase()
    {
        _controller.FindNearestTarget();
        Transform targetTransform = _controller.Target.transform;
        if (_controller.CanOtherAction && Vector3.Distance(transform.position, targetTransform.position) < _attackRange)
        {
            print("ATTACK");
            _controller.BasicAttack(_controller, _controller.Target.GetComponent<CharacterController>());
        }
        else if (_controller.CanMove)
        {
            print(_movement.tag);
            _movement.MoveToTarget(_controller.Target);
            print(_controller.Target);
            print("Waiting to be finished moving...");
            yield return new WaitUntil(() => _controller.Ready);
        }
        else
        {
            _controller.EndTurn();
        }
    }
}
