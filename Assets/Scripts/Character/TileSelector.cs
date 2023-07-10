using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public Tile[] Tiles { get; set; }
    public List<Tile> SelectableTiles { get; set; }
    public Tile CurrentTile { get; set; }

    private void Start()
    {
        SelectableTiles = new List<Tile>();
    }

    private void OnEnable()
    {
        TurnManager.OnBattleEnter += UpdateTiles;
    }

    private void OnDisable()
    {
        TurnManager.OnBattleEnter -= UpdateTiles;
    }

    public void UpdateTiles()
    {
        GameObject[] tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        Tiles = new Tile[tileObjects.Length];

        for (int i = 0; i < tileObjects.Length; i++)
        {
            Tiles[i] = tileObjects[i].GetComponent<Tile>();
        }
    }

    public void GetCurrentTile()
    {
        CurrentTile = GetTargetTile(gameObject);
        CurrentTile.SetCurrent();
    }


    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        if (Physics.Raycast(target.transform.position, Vector3.down, out hit, 3))
        {
            tile = TileCache.GetTile(hit.collider);
        }

        return tile;
    }

    public void FindSelectableTechTiles(float maxDistance, Tile centerTile)
    {
        Tile center;
        if (centerTile == null)
        {
            GetCurrentTile();
            center = CurrentTile;
        }
        else center = centerTile;
        
        
        Queue<Tile> process = new Queue<Tile>();
        process.Enqueue(center);
        center.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();


            SelectableTiles.Add(t);
            t.SetSelectable();


            if (t.distance >= maxDistance) continue;

            foreach (Tile tile in t.adjacencyList)
            {
                if (tile.visited) continue;
                tile.parent = t;
                tile.visited = true;
                tile.distance = t.distance + 1;
                if (tile.distanceFromActive) tile.distanceFromActive.text = tile.distance.ToString();
                process.Enqueue(tile);
            }
        }
    }

    public Tile GetFarthestInRangeTile(CharacterController target, Technique tech)
    {
        List<Tile> sTiles = SelectableTiles.ToList();
        ResetSelectableTiles();

        List<Tile> inRangeTiles = new();
        foreach (Tile tile in sTiles)
        {
            FindSelectableTechTiles(tech.range, tile);
            if (GetTargetedCharacters().Contains(target))
            {
                inRangeTiles.Add(tile);
            }
            ResetSelectableTiles();
        }

        Tile farthestTile = sTiles[0];
        foreach (Tile tile in inRangeTiles)
        {
            FindSelectableTechTiles(tech.range, tile);
            if ((tile.transform.position - target.transform.position).sqrMagnitude >= (farthestTile.transform.position - target.transform.position).sqrMagnitude)
            {
                farthestTile = tile;
            }
        }
        
        return farthestTile;
    }
    
    public virtual void FindSelectableMoveTiles(float jumpHeight, float maxMoveDistance)
    {

        GetCurrentTile();
        Tile center = CurrentTile;

        Queue<Tile> process = new Queue<Tile>();
        process.Enqueue(center);
        center.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();


            SelectableTiles.Add(t);
            t.SetSelectable();


            if (t.distance >= maxMoveDistance) continue;

            foreach (Tile tile in t.GetMovementAdjacency(jumpHeight))
            {
                if (tile.visited) continue;
                tile.parent = t;
                tile.visited = true;
                tile.distance = t.distance + 1;
                if (tile.distanceFromActive) tile.distanceFromActive.text = tile.distance.ToString();
                process.Enqueue(tile);
            }
        }
    }
/*
        foreach (Tile tile in t.adjacencyList)
            {
                if (tile.visited)
                {
                    continue;
                }

                tile.parent = t;
                tile.visited = true;
                tile.distance = t.distance + 1;
                
                if (tile.distanceFromActive) tile.distanceFromActive.text = tile.distance.ToString();
                
                switch (state)
                {
                    case Tile.State.Move:
                        if (!MoveCheck(t, tile, maxHeight))
                        {
                            //if (tile.distanceFromActive) tile.distanceFromActive.text = "F";
                            //if (tile.distance < maxDistance) tile.visited = false;
                            continue;
                        }
                        break;
                    case Tile.State.Tech:
                        break;
                }
                if (tile.distanceFromActive) tile.distanceFromActive.text = tile.distance.ToString();
                process.Enqueue(tile);
            }
            
        }*/
    

    public bool MoveCheck(Tile parent, Tile adjacent, float jumpHeight)
    {
        if (adjacent.HasCharacter())
        {
            if (adjacent.distanceFromActive) adjacent.distanceFromActive.text = "F-Ch";
            return false;
        }

        if (!adjacent.passable)
        {
            if (adjacent.distanceFromActive) adjacent.distanceFromActive.text = "F-P";
            return false;
        }

        if (Mathf.Abs(parent.height - adjacent.height) >= jumpHeight)
        {
            if (adjacent.distanceFromActive) adjacent.distanceFromActive.text = "F-JL";
            return false;
        }

        if (parent.height < adjacent.height && Physics.Raycast(parent.transform.position + new Vector3(0, 0.6f, 0), Vector3.up,
                out RaycastHit _, adjacent.height))
        {
            if (adjacent.distanceFromActive) adjacent.distanceFromActive.text = "F-BA";
            return false;
        }

        if (parent.height > adjacent.height && Physics.Raycast(adjacent.transform.position + new Vector3(0, 0.5f, 0),
                Vector3.up, out RaycastHit _, parent.height - adjacent.height))
        {
            if (adjacent.distanceFromActive) adjacent.distanceFromActive.text = "F-BB";
            return false;
        }
        return true;
    }

    private bool LineCheck(Tile currentTile, Tile targetTile)
    {
        Vector3 currentTilePos = currentTile.pos;
        float tolerance = 0.1f;
        return (Math.Abs(currentTilePos.x - targetTile.pos.x) < tolerance) ||
               (Math.Abs(currentTilePos.z - targetTile.pos.z) < tolerance);
    }


    public void RemoveNoLOSTiles(Vector3 losPos)
    {
        foreach (Tile tile in SelectableTiles)
        {
            Ray ray = new(losPos, (tile.posLOS - losPos).normalized);
            float distance = Vector3.Distance(losPos, tile.posLOS);
            print("distance: " + distance);
            RaycastHit hitInfo;

            if (!Physics.Raycast(ray, out hitInfo, distance)) continue;
            if (hitInfo.collider.CompareTag("Tile"))
            {
                tile.Reset();
            }

        }
    }

    public bool HaveLOS(Vector3 origin, Vector3 tilePos)
    {
        Ray ray = new(origin, (tilePos - origin).normalized);
        float distance = Vector3.Distance(origin, tilePos);
        print("distance: " + distance);

        return Physics.Raycast(ray, out _, distance);
    }

    private void Update()
    {
        
        
        /*
        if (TurnManager.GetActivePlayer() == null) return;
        CharacterController activePlayer = TurnManager.GetActivePlayer();
        Vector3 origin = activePlayer.LosPos;
        Vector3 direction = (new Vector3(7.5f, 1.5f, -4.5f) - origin);
        Debug.DrawRay(origin, direction);
        foreach (Tile tile in SelectableTiles)
        {
            Ray ray = new(origin, (tile.posLOS - origin).normalized);
            float distance = Vector3.Distance(origin, tile.posLOS);
            RaycastHit hitInfo;
            Debug.DrawRay(origin, (tile.posLOS - origin));
            
        }
        */
    }

    public List<CharacterController> GetTargetedCharacters()
    {
        List<CharacterController> targets = new List<CharacterController>();
        foreach (Tile tile in SelectableTiles)
        {
            if (tile.HasCharacter())
            {
                CharacterController target = tile.GetCharacter();
                targets.Add(target);
            }
        }

        return targets;
    }

    public void ResetSelectableTiles()
    {
        if (CurrentTile != null)
        {
            CurrentTile.current = false;
            CurrentTile = null;
        }
        
        foreach (Tile tile in Tiles)
        {
            tile.Reset();
        }

        SelectableTiles.Clear();
    }
}
