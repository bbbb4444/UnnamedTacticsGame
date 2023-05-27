using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCController : CharacterController
{
    
    public static event UnityAction OnPhaseStartNPC;
    public AIHandler _aiHandler;

    protected override void Start()
    {
        _aiHandler = GetComponent<AIHandler>();
        base.Start();
    }
    

    public override void BeginPhase()
    {
        if (Actions > 0)
        {
            OnPhaseStartNPC?.Invoke();
            print("NPC BEGIN TURN GOOOOOOO");
            Ready = false;
            StartCoroutine(_aiHandler.BeginPhase());
        }
        else EndTurn();
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
