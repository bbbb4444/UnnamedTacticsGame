
using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private bool TechConfirm { get; set; }
    
    private CharacterController _activeCharacter;
    private CharacterController _targetCharacter;
    
    private bool _isFollowing;
    
    public bool EnableClick { get; set; }
    public bool EnableMovement { get; set; }
    void Start()
    {
        TechConfirm = false;
        
        ActionMenu.OnMoveSelected += Enable;
        CharacterController.OnTechTarget += Enable;
        
        TurnManager.OnTurnEnd += Disable;
        CharacterController.OnPhaseStart += MoveToActivePlayer;

        ActionMenuTech.OnTechClicked += BeginAI;
        
        _transform = transform;
        _cursorGraphic = transform.GetChild(0);
        _cameraPivot = GameObject.FindGameObjectWithTag("CameraPivot");
    }
    public void OnClick()
    {
        if (!EnableClick) return;
        print(ActionMenu.currentAction);
        switch (ActionMenu.currentAction)
        {
            case ActionMenu.ActionType.None:
                break;
            case ActionMenu.ActionType.Move:
                TileSelectMove();
                break;
            case ActionMenu.ActionType.Attack:
                break;
            case ActionMenu.ActionType.Tech:
                if (!TechConfirm) TileSelectTech();
                else ConfirmTech();
                break;
            case ActionMenu.ActionType.Item:
                break;
            case ActionMenu.ActionType.Defend:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnCancel()
    {
        if (!EnableClick) return;
        TechConfirm = false;
        print("Last Screen: " + UIManager.LastScreen);
        Enable(false);
        MoveToActivePlayer();
        UIManager.OpenScreen(UIManager.LastScreen);
    }
    
    private void TileSelectMove()
    {
        Tile currentTile = GetTile();
        if (currentTile == null) return;
        
        Disable();
        _activeCharacter.MoveToTile(currentTile);
    }

    private void TileSelectTech()
    {
        TechConfirm = true;
        
        EnableMovement = false;
        Tile currentTile = GetTile();
        if (currentTile == null || !currentTile.selectable) return;
        TurnManager.GetActivePlayer().GetTechHandler().ShowArea(currentTile);
    }
    private void ConfirmTech()
    {
        TechConfirm = false;
        
        Tile currentTile = GetTile();
        List<CharacterController> targets = TurnManager.GetActivePlayer().tileSelector.GetTargetedCharacters();
        Disable();
        TurnManager.GetActivePlayer().TechAttack(targets);
    }
    
    
    // Moving the cursor
    public void OnMove(InputValue value)
    {
        if (_isFollowing || !EnableMovement) return;
        
        int x = (int) value.Get<Vector2>().x;
        int z = (int) value.Get<Vector2>().y;
        Vector3 movement = new(x, 0, z);
        
        _rotationDeg = _cameraPivot.GetComponent<CameraMovement>().currentAngle;
        movement = Quaternion.AngleAxis(_rotationDeg, Vector3.up) * movement;
        
        Vector3 direction = movement.normalized;

        float y = CalcHigherY(direction);
        movement.y += y;
        _transform.position += movement;
        
        y = CalcLowerY();
        _transform.position += new Vector3(0,y,0);
    }

    float CalcHigherY(Vector3 direction)
    {
        // Calc positive y (to raise the cursor)
        float y = 0;
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
        TechConfirm = false;
        
        _activeCharacter = TurnManager.GetActivePlayer();
        Vector3 targetPos = _activeCharacter.transform.position;
        float minY = _activeCharacter.ccollider.bounds.min.y;
        Vector3 bottomPos = new Vector3(targetPos.x, minY - 0.5f, targetPos.z);
        transform.position = bottomPos;
    }
    public void Enable(bool d)
    {
        EnableClick = d;
        EnableMovement = d;
    }

    void Disable()
    {
        EnableClick = false;
        EnableMovement = false;
    }

    void Enable()
    {
        EnableClick = true;
        EnableMovement = true;
    }
    
    
    // AI
    private void BeginAI()
    {
        _activeCharacter = TurnManager.GetActivePlayer();
        StartCoroutine(AIActions());
    }

    private IEnumerator AIActions()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject targetCharacter = _activeCharacter.Target;
        print(targetCharacter);
        float minY = targetCharacter.GetComponent<CharacterController>().ccollider.bounds.min.y;
        print(minY);
        MoveTo(targetCharacter, minY);
        yield return new WaitForSeconds(0.5f);
        OnClick();
        yield return new WaitForSeconds(0.5f);
        OnClick();
    }
}
