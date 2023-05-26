using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class CharacterMovement : MonoBehaviour
{
    public bool turn = false;
    public CharacterController controller;
    protected Transform Transform;
    protected CursorMovement TileCursor;
    private List<Tile> _selectableTiles = new List<Tile>();
    private Tile[] _tiles;
    
    private Stack<Tile> _path = new Stack<Tile>();
    private Tile _currentTile;
    
    public int move = 5;
    public float jumpHeight = 3f;
    public float moveSpeed = 2f;
    public float jumpVelocity = 4.5f;
    
    private Vector3 _velocity = new Vector3();
    private Vector3 _direction = new Vector3();

    
    private float _halfHeight = 0;

    private bool _fallingDown = false;
    private bool _jumpingUp = false;
    private bool _movingEdge = false;
    private Vector3 _jumpTarget;

    public Tile actualTargetTile;

    
    void OnEnable()
    {
        
    }
    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();
        TileCursor = GameObject.FindWithTag("Cursor").GetComponent<CursorMovement>();
        UpdateTiles();
        _halfHeight = GetComponent<Collider>().bounds.extents.y;
    }
    private void FixedUpdate()
    {
        if (Moving)
        {
            Move();
        }
    }
    public bool Moving { get; set; }
    private void UpdateTiles()
    {
        GameObject[] tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        _tiles = new Tile[tileObjects.Length];
        
        for (int i = 0; i < tileObjects.Length; i++)
        {
            _tiles[i] = tileObjects[i].GetComponent<Tile>();
        }
    }

    public void GetCurrentTile()
    {
        _currentTile = GetTargetTile(gameObject);
        _currentTile.SetCurrent();
    }

    
    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        print(gameObject.tag + ": " + target.transform.position);
        if (Physics.Raycast(target.transform.position, Vector3.down, out hit, 3))
        {
            tile = TileCache.GetTile(hit.collider);
        }

        return tile;
    }

    public void ComputeAdjacencyLists(float jumpHeight, Tile target)
    {
        foreach (Tile tile in _tiles)
        {
            tile.FindNeighbors(jumpHeight, target);
        }
    }

    public virtual void FindSelectableTiles()
    {
        ComputeAdjacencyLists(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();
        
        process.Enqueue(_currentTile);
        _currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();
            
            _selectableTiles.Add(t);
            t.SetSelectable();

            if (t.distance < move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = t.distance + 1;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveTo(Tile tile)
    {
        if (_selectableTiles.Contains(tile))
        {
            BuildPathToTile(tile);
            //CalcDirectionTo(tile.transform.position);
            Moving = true;
        }
    }
    public void BuildPathToTile(Tile tile)
    {
        _path.Clear();
        tile.SetTarget();

        Tile next = tile;
        while (next != null && _path.Count < 50)
        {
            _path.Push(next);
            next = next.parent;
        }
    }

    public void Move()
    {
        if (_path.Count > 0)
        {
            Tile t = _path.Peek();
            Vector3 target = t.transform.position;
            
            //Calculate unit's position on top of target tile
            target.y += _halfHeight + t.sizeY;

            if (Vector3.Distance(transform.position, target) >= 0.02f * (moveSpeed/2.0f))
            {
                bool jump = transform.position.y != target.y;
                 if (jump)
                {
                    print("jump!");
                    Jump(target);
                }
                else
                {
                    CalcDirectionTo(target);
                    SetHorizontalVelocity();
                }
                

                //locomotion
                transform.forward = _direction;
                transform.position += _velocity * Time.deltaTime;
            }
            else
            {
                //Tile center reached
                _fallingDown = false;
                _jumpingUp = false;
                _movingEdge = false;
                
                transform.position = target;
                _path.Pop();
            }
        }
        else
        {
            OnStopMoving();
        }
    }

    public void ResetSelectableTilesList()
    {
        if (_currentTile != null)
        {
            _currentTile.current = false;
            _currentTile = null;
        }
        
        foreach (Tile tile in _tiles)
        {
            tile.Reset();
        }

        _selectableTiles.Clear();
    }

    void CalcDirectionTo(Vector3 target)
    {
        _direction = target - transform.position;
        _direction.Normalize();
    }

    void SetHorizontalVelocity()
    {
        _velocity = _direction * moveSpeed;
    }

    void Jump(Vector3 target)
    {
        if (_fallingDown)
        {
            FallDownward(target);
        }
        else if (_jumpingUp)
        {
            JumpUpward(target);
        }
        else if (_movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }
    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;
        target.y = transform.position.y;

        CalcDirectionTo(target);

        if (transform.position.y > targetY)
        {
            _fallingDown = false;
            _jumpingUp = false;
            _movingEdge = true;

            _jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        else
        {
            _fallingDown = false;
            _jumpingUp = true;
            _movingEdge = false;

            // Initial jump
            float difference = targetY - transform.position.y;
            _velocity = _direction * 1.1f;
            _velocity *= Mathf.Pow(0.87f, difference); // Horizontal speed of jumping up
            _velocity.y = (-0.095f * Mathf.Pow(difference,2.0f)) + (1.867f * difference) + 3.066f; // Vertical power of jumping up
            // Ideal difference - vertical.y values : 1 - 4.8, 2 - 6.5, 3 - 7.8, 4 - 8.95, 5 - 10.06
        }
    }
    void FallDownward(Vector3 target)
    {
        _velocity += Physics.gravity * Time.deltaTime;
        
        if (transform.position.y <= target.y)
        {
            _fallingDown = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            _velocity = new Vector3();
        }
    }
    void JumpUpward(Vector3 target)
    {
        _velocity += (Physics.gravity * Time.deltaTime);
        if (transform.position.y > target.y)
        {
            _jumpingUp = false;
            _fallingDown = true;
        }
    }
    void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, _jumpTarget) >= 0.05f * (moveSpeed/2.0f))
        {
            SetHorizontalVelocity();
        }
        else
        {
            _movingEdge = false;
            _fallingDown = true;

            _velocity /= 4.0f * (moveSpeed/1.6f); //Falling speed when jumping down. Lower factor (Cur: 1.6f) if overshooting
            _velocity.y = 2.0f; // Small jump before jumping down
        }
    }

    // A* Path finding
    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);
        
        return lowest;
    }
    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null && tempPath.Count < 60)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= move)
        {
            return t.parent;
        }

        Tile endTile = null;
        for (int i = 0; i <= move; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;
    }
    protected void FindPath(Tile target)
    {
        ComputeAdjacencyLists(jumpHeight, target);
        GetCurrentTile();

        List<Tile> openList = new();
        List<Tile> closedList = new();

        openList.Add(_currentTile);
        _currentTile.h = Vector3.SqrMagnitude(_currentTile.transform.position - target.transform.position);
        _currentTile.f = _currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);

            closedList.Add(t);

            if (t == target)
            {
                actualTargetTile = FindEndTile(t);
                BuildPathToTile(actualTargetTile);
                return;
            }

            foreach (Tile tile in t.adjacencyList)
            {
                if (closedList.Contains(tile))
                {
                    //do nothing
                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.SqrMagnitude(tile.transform.position - t.transform.position);

                    if (tempG < tile.g)
                    {
                        tile.parent = t;
                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;
                    tile.g = t.g + Vector3.SqrMagnitude(tile.transform.position - t.transform.position);
                    tile.h = Vector3.SqrMagnitude(tile.transform.position - target.transform.position);
                    tile.f = tile.g + tile.h;
                    openList.Add(tile);
                }
            }
            
        }
        
        //TODO: if no path to target tile
    }

    // Start Moving
    public virtual void ShowSelectableTiles()
    {
        GetCurrentTile();
        FindSelectableTiles();
    }

    protected virtual void OnStopMoving()
    {
        Moving = false;
        ResetSelectableTilesList();
        controller.EndMove();
    }
}
