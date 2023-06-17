using UnityEngine;

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
    }

    public void MoveToTarget(GameObject t)
    {
        target = t;
        FindSelectableTiles(); 
        CalculatePath(); 
        actualTargetTile.target = true; 
        controller.MoveToTile(actualTargetTile);
    }
    
    protected override void OnStopMoving()
    {
        base.OnStopMoving();
        TileCursor.Follow(null);
    }
    void CalculatePath()
    {
        Tile targetTile = TileSelector.GetTargetTile(target);
        FindPath(targetTile);
    }

    
}
