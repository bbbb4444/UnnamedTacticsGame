using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    private Color _color;
    
    private Renderer _renderer;
    public float height;
    public float sizeY;
    public bool passable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    private List<Tile> _neighborsForward;
    private List<Tile> _neighborsBack;
    private List<Tile> _neighborsLeft;
    private List<Tile> _neighborsRight;
    
    
    public List<Tile> adjacencyList = new List<Tile>();
    
    
    //Needed BFS
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    // Needed A*
    public float f = 0;
    public float g = 0;
    public float h = 0;
    
    void Start()
    {
        CalcHeight();
        _renderer = GetComponent<Renderer>();
        sizeY = GetComponent<Collider>().bounds.size.y;
        UpdateNeighbors();
        _color = new Color(0.4f + height*0.07f, 0.78f, 0.4f + height*0.07f);
        _renderer.materials[1].color = _color;
    }
    
    public void SetSelectable()
    {
        selectable = true;
        _renderer.materials[1].color = Color.yellow;
    }
    public void SetCurrent()
    {
        current = true;
        _renderer.materials[1].color = Color.magenta;
    }
    public void SetTarget()
    {
        target = true;
        _renderer.materials[1].color = Color.green;
    }
    private void CalcHeight()
    {
        height = transform.position.y;
    }

  
    public void Reset()
    {
        _renderer.materials[1].color = _color;
        adjacencyList.Clear();
        
        current = false; 
        target = false; 
        selectable = false;
        
        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }
    
    private void UpdateNeighbors()
    {
        _neighborsForward = Neighbors(Vector3.forward);
        _neighborsBack = Neighbors(Vector3.back);
        _neighborsLeft = Neighbors(Vector3.left);
        _neighborsRight = Neighbors(Vector3.right);
    }
    
    private List<Tile> Neighbors(Vector3 direction)
    {
        Vector3 halfExtents = new Vector3(0.25f, 30f, 0.25f);
        Collider[] colliders = new Collider[20]; // array size should be max height of tiles
        int numColliders = Physics.OverlapBoxNonAlloc(transform.position + direction, halfExtents, colliders);

        List<Tile> neighbors = new List<Tile>();
        for (int i = 0; i < numColliders; i++)
        {
            if (colliders[i].CompareTag("Tile"))
            {
                neighbors.Add(colliders[i].GetComponent<Tile>());
            }
        }
        return neighbors;
    }
    
    public void FindNeighbors(float jumpHeight, Tile targetTile)
    {
        CheckTile(Vector3.forward, jumpHeight, targetTile);
        CheckTile(Vector3.back, jumpHeight, targetTile);
        CheckTile(Vector3.left, jumpHeight, targetTile);
        CheckTile(Vector3.right, jumpHeight, targetTile);
    }
    
     private void CheckTile(Vector3 direction, float jumpHeight, Tile targetTile)
     { 
         List<Tile> neighbors = new List<Tile>();
         if (direction == Vector3.forward)
             neighbors = _neighborsForward;
         else if (direction == Vector3.back)
             neighbors = _neighborsBack;
         else if (direction == Vector3.left)
             neighbors = _neighborsLeft;
         else if (direction == Vector3.right)
             neighbors = _neighborsRight;
         
        foreach (Tile tile in neighbors)
        {
            float adjHeight = tile.height;
            
            // Skip if AT is not passable
            if (!tile.passable) continue;

            // Skip if the height difference is larger than the jump height
            if (Mathf.Abs(height - adjHeight) >= jumpHeight) continue;
            
            // Skip if something is directly on top of AT and if the AT isn't the target tile
            // TODO: Combine with height > adjHeight condition
            if (Physics.Raycast(tile.transform.position+new Vector3(0,0.1f,0), Vector3.up, out RaycastHit _, 1) && tile != targetTile) continue;
            // Skip when there is something blocking the parent tile from above (AT is higher than parent tile).
            if (height < adjHeight &&
                Physics.Raycast(transform.position + new Vector3(0, 0.6f, 0), Vector3.up, out RaycastHit _, adjHeight)) continue;
            // Skip when there is something blocking the AT from above (AT is lower than parent tile).
            if (height > adjHeight &&
                Physics.Raycast(tile.transform.position + new Vector3(0, 0.5f, 0), Vector3.up, out RaycastHit _,
                    height - adjHeight)) continue;
            
            /* AT will only be added to the adjacency list if the following conditions are met:
                1. Is labeled passable
                2. Height difference between parent and adjacent tile is low enough for jumping
                3. There is nothing directly on top of it
                4. There is nothing blocking a character from jumping upwards to the tile (When the adjacent tile is higher than the parent tile)
                5. There is nothing blocking a character from jumping downwards to the tile (When the adjacent tile is lower than the parent tile) 
                */
            adjacencyList.Add(tile);
            
        }
    }
}
