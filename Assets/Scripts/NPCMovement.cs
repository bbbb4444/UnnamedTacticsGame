using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NPCMovement : CharacterMovement
{
    public GameObject target;
    protected override void Start()
    {
        base.Start();
    }
    
    public override void FindSelectableTiles()
    {
        print("npc show select");
        base.FindSelectableTiles(); 
        controller.FindNearestTarget(); 
        CalculatePath(); 
        actualTargetTile.target = true; 
    }

    public void MoveToTarget(GameObject t)
    {
        target = t;
        CalculatePath(); 
        actualTargetTile.target = true; 
        FindSelectableTiles(); 
        controller.MoveToTile(actualTargetTile);
    }
    
    protected override void OnStopMoving()
    {
        base.OnStopMoving();
        TileCursor.Follow(null);
    }
    void CalculatePath()
    {
        Tile targetTile = GetTargetTile(target);
        FindPath(targetTile);
    }

    
}
