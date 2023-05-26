using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCController : CharacterController
{
    
    public static event UnityAction OnTurnStartNPC;
    public AIHandler _aiHandler;

    protected override void Start()
    {
        base.Start();
        _aiHandler = GetComponent<AIHandler>();
    }

    public override void EndTurn()
    {
        base.EndTurn();
    }

    public override void BeginPhase()
    {
            print("NPC BEGIN TURN GOOOOOOO");
            _aiHandler.BeginPhase();
    }

    public override void RemoveAction(int num)
    {
        base.RemoveAction(num);
    }
    
    public override void FindNearestTarget()
    {
        print("Finding nearest target");
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            float d = Vector3.SqrMagnitude(transform.position - obj.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }

        Target = nearest;
        print("nearest target = " + Target.transform.position);
    }
}
