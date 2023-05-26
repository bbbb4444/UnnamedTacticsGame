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
    
    public override void ShowSelectableTiles()
    {
        print("npc show select");
        controller.FindNearestTarget(); 
        CalculatePath(); 
        actualTargetTile.target = true; 
        base.ShowSelectableTiles(); 
    }

    public void MoveToTarget(GameObject t)
    {
        target = t;
        CalculatePath(); 
        actualTargetTile.target = true; 
        base.ShowSelectableTiles(); 
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
