
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorMovement : MonoBehaviour
{
    private Transform _transform;
    private Transform _cursorGraphic;
    private GameObject _cameraPivot;
    private int _rotationDeg;
    private bool _disable = true;
    private bool _isActive = true;
    
    private CharacterController _activePlayer;
    private bool _isFollowing;
    void Start()
    {
        ActionMenu.OnMoveSelected += Enable;
        TurnManager.OnTurnEnd += Disable;
        CharacterController.OnPhaseStart += MoveToActivePlayer;
        // CharacterController.OnTurnStartNPC += MoveToActivePlayer;
        _transform = transform;
        _cursorGraphic = transform.GetChild(0);
        _cameraPivot = GameObject.FindGameObjectWithTag("CameraPivot");
    }
    public void OnClick()
    {
        if (_disable) return;
        
        print("CLICKED");
        if (ActionMenu.currentAction == ActionMenu.ActionType.Move)
        {
            Tile currentTile = GetTile();
            if (currentTile != null)
            {
                Disable();
                _activePlayer.MoveToTile(currentTile);
            }
        }
        else if (ActionMenu.currentAction == ActionMenu.ActionType.Attack)
        {
            
        }
    }
    
    public void OnMove(InputValue value)
    {
        if (_isFollowing || _disable) return;
        
        int x = (int) value.Get<Vector2>().x;
        int z = (int) value.Get<Vector2>().y;
        Vector3 movement = new(x, 0, z);
        
        _rotationDeg = _cameraPivot.GetComponent<CameraMovement>().currentAngle;
        movement = Quaternion.AngleAxis(_rotationDeg, Vector3.up) * movement;
        
        Vector3 direction = movement.normalized;
        
        int y = CalcHigherY(direction);
        movement.y += y;
        _transform.position += movement;
        
        y = CalcLowerY();
        _transform.position += new Vector3(0,y,0);
    }

    int CalcHigherY(Vector3 direction)
    {
        // Calc positive y (to raise the cursor)
        int y = 0;
        Vector3 origin = _transform.position + direction;
        Ray ray = new Ray(origin, Vector3.up);
        if (Physics.Raycast(ray))
        {
            RaycastHit[] itemsAbove = Physics.RaycastAll(ray);
            foreach (RaycastHit item in itemsAbove)
            {
                Transform itemTransform = item.transform;
                if (itemTransform.CompareTag("Tile"))
                {
                    y++;
                }
            }
        }
        return y;
    }
    int CalcLowerY()
    {
        // Calc negative y (to lower the cursor)
        int y = 0;
        Vector3 origin = _transform.position + new Vector3(0, 0.6f, 0);
        Ray ray = new(origin, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            y = (int) -hit.distance;
        }
        return y;
    }

    public void MoveTo(GameObject targetObj, float minY)
    {
        Vector3 targetPos = targetObj.transform.position;
        Vector3 bottomPos = new Vector3(targetPos.x, minY-0.5f, targetPos.z);
        transform.position = bottomPos;
    }
    public void Follow(GameObject targetObj, float minY)
    {
        Vector3 targetPos = targetObj.transform.position;
        Vector3 bottomPos = new Vector3(targetPos.x, minY-0.5f, targetPos.z);
        transform.position = bottomPos;
        transform.SetParent(targetObj.transform);
        _isFollowing = true;
    }
    public void Follow(GameObject targetObj)
    {
        if (targetObj == null)
        {
            transform.SetParent(null);
            _isFollowing = false;
        }
        /*
        else
        {
            Vector3 targetPos = targetObj.transform.position;
            float minY = targetObj.GetComponent<Renderer>().bounds.min.y;
            Vector3 bottomPos = new Vector3(targetPos.x, minY - 0.5f, targetPos.z);
            transform.position = bottomPos;
            transform.SetParent(targetObj.transform);
            _isFollowing = true;
        }
        */
    }



    private Tile GetTile()
    {
        Physics.Raycast(_cursorGraphic.position+new Vector3(0,1,0), Vector3.down, out RaycastHit hit, 2);
        Tile tile = TileCache.GetTile(hit.collider);
        if (tile.passable)
        {
            return tile;
        }
        else
        {
            return null;
        }
    }
    
    private void MoveToActivePlayer()
    {
        _activePlayer = TurnManager.GetActivePlayer();
        Vector3 targetPos = _activePlayer.transform.position;
        float minY = _activePlayer.Renderer.bounds.min.y;
        Vector3 bottomPos = new Vector3(targetPos.x, minY - 0.5f, targetPos.z);
        transform.position = bottomPos;
    }
    public void Disable(bool d)
    {
        _disable = d;
    }

    void Disable()
    {
        _disable = true;
    }

    void Enable()
    {
        _disable = false;
    }
    
}
