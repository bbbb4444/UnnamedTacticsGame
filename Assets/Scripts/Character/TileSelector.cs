using System;
using System.Collections.Generic;
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

    public virtual void FindSelectableTiles(float maxHeight, float maxDistance, Tile centerTile, Tile.State state,
        RangeStyle rangeStyle = RangeStyle.Circle)
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

                switch (state)
                {
                    case Tile.State.Move:
                        if (!MoveCheck(t, tile, maxHeight)) continue;
                        break;
                    case Tile.State.Tech:
                        if (rangeStyle == RangeStyle.Line && !LineCheck(center, tile)) continue;
                        break;
                }

                process.Enqueue(tile);
            }
        }
    }

    public bool MoveCheck(Tile parent, Tile adjacent, float jumpHeight)
    {
        if (adjacent.HasCharacter()) return false;
        if (!adjacent.passable) return false;
        if (Mathf.Abs(parent.height - adjacent.height) >= jumpHeight) return false;
        if (parent.height < adjacent.height && Physics.Raycast(transform.position + new Vector3(0, 0.6f, 0), Vector3.up,
                out RaycastHit _, adjacent.height)) return false;
        if (parent.height > adjacent.height && Physics.Raycast(adjacent.transform.position + new Vector3(0, 0.5f, 0),
                Vector3.up, out RaycastHit _, parent.height - adjacent.height)) return false;
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
