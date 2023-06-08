using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class Tile : MonoBehaviour
{
    public enum State
    {
        Move,
        Tech
    }
    
    private Color _color;

    private Transform _tform;
    private Renderer _renderer;
    public Vector3 pos;
    public Vector3 posLOS;
    public float height;
    public float sizeY;
    public bool passable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    public List<Tile> _neighborsForward;
    public List<Tile> _neighborsBack;
    public List<Tile> _neighborsLeft;
    public List<Tile> _neighborsRight;
    
    
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
        _tform = GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
        sizeY = GetComponent<Collider>().bounds.size.y;
        
        UpdateNeighbors();
        ComputeAdjacency();
        
        _color = new Color(0.4f + height*0.07f, 0.78f, 0.4f + height*0.07f);
        _renderer.materials[1].color = _color;

        pos = _tform.position + new Vector3(0, 0, 0);
        posLOS = pos + new Vector3(0, sizeY+0.5f, 0);
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
    
    // Calculates the lists of neighbors. Skip if the neighbor is blocked from above.
    private List<Tile> Neighbors(Vector3 direction)
    {
        Vector3 halfExtents = new Vector3(0.25f, 30f, 0.25f);
        Collider[] colliders = new Collider[20]; // array size should be max height of tiles
        int numColliders = Physics.OverlapBoxNonAlloc(transform.position + direction, halfExtents, colliders);

        List<Tile> neighbors = new List<Tile>();
        for (int i = 0; i < numColliders; i++)
        {
            Vector3 origin = colliders[i].transform.position + new Vector3(0, 0.1f, 0);
            int layerMask = 1 << LayerMask.NameToLayer("Tile");
            
            // If this isn't a tile, skip this.
            if (!colliders[i].CompareTag("Tile")) continue;
            
            // If another tile is blocking this tile from above, skip this tile.
            // TODO: Move this to tile processing in TileSelector if raw neighbors are ever needed.
            if (Physics.Raycast(origin, Vector3.up, out RaycastHit _, 1, layerMask)) continue;
            
            neighbors.Add(colliders[i].GetComponent<Tile>());
        }
        return neighbors;
    }
    
    public void ComputeAdjacency()
    {
        CheckTile(Vector3.forward);
        CheckTile(Vector3.back);
        CheckTile(Vector3.left);
        CheckTile(Vector3.right);
    }
    
    private void CheckTile(Vector3 direction)
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
             adjacencyList.Add(tile);
         }
     }


     public CharacterController GetCharacter()
     {
         Ray ray = new(_tform.position + new Vector3(0, 0.1f, 0), Vector3.up);
         RaycastHit hit;
         if (Physics.Raycast(ray, out hit, 1))
         {
             if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("NPC"))
             {
                 return hit.collider.GetComponent<CharacterController>();
             }
         }
         print("No character on tile");
         return null;
     }

     public bool HasCharacter()
     {
         Ray ray = new(_tform.position + new Vector3(0, 0.1f, 0), Vector3.up);
         RaycastHit hit;
         if (Physics.Raycast(ray, out hit, 1))
         {
             if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("NPC"))
             {
                 return true;
             }
         }
         return false;
     }
}
