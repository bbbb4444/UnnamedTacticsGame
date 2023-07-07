
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CursorMovement : MonoBehaviour
{
    public static event UnityAction OnSelectTech;
    
    private Transform _transform;
    private Transform _cursorGraphic;
    private GameObject _cameraPivot;
    private int _rotationDeg;

    private bool TechConfirm { get; set; }
    
    private CharacterController _activeCharacter;
    public CharacterController HoverCharacter { get; set; }

    private bool _isAIClick;
    public bool EnableClick { get; set; }
    public bool EnableMovement { get; set; }
    void Start()
    {
        TechConfirm = false;
        
        _transform = transform;
        _cursorGraphic = transform.GetChild(0);
        _cameraPivot = GameObject.FindGameObjectWithTag("CameraPivot");
    }

    private void OnEnable()
    {
        ActionMenu.OnMoveSelected += Enable;
        CharacterController.OnTechTarget += Enable;
        
        TurnManager.OnTurnEnd += Disable;
        CharacterController.OnPhaseStart += MoveToActivePlayer;
        
        ActionMenuTech.OnTechClicked += BeginAI;
    }

    private void OnDisable()
    {
        ActionMenu.OnMoveSelected -= Enable;
        CharacterController.OnTechTarget -= Enable;
        
        TurnManager.OnTurnEnd -= Disable;
        CharacterController.OnPhaseStart -= MoveToActivePlayer;
        
        ActionMenuTech.OnTechClicked -= BeginAI;
    }

    public void OnClick()
    {
        if (TurnManager.IsPlayerTurn && EnableClick || _isAIClick)
        {
            print(ActionMenu.CurrentAction);
            switch (ActionMenu.CurrentAction)
            {
                case ActionMenu.ActionType.None:
                    break;
                case ActionMenu.ActionType.Move:
                    TileSelectMove();
                    break;
                case ActionMenu.ActionType.Tech:
                    if (!TechConfirm) TileSelectTech();
                    else ConfirmTech();
                    break;
                case ActionMenu.ActionType.Wait:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }  
        }
   
    }

    public void OnCancel()
    {
        if (!TurnManager.IsPlayerTurn) return;
        if (!EnableClick) return;
        TechConfirm = false;
        Enable(false);
        MoveToActivePlayer();
        UIManager.Instance.OpenScreen(ScreenType.ActionMenu); 
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
        TurnManager.GetActivePlayer().TechHandler.ShowArea(currentTile);
        OnSelectTech?.Invoke();
    }
    private void ConfirmTech()
    {
        TechConfirm = false;
        
        Tile currentTile = GetTile();
        List<CharacterController> targets = TurnManager.GetActivePlayer().tileSelector.GetTargetedCharacters();
        Disable();
        TurnManager.GetActivePlayer().TechAttack(targets);
    }

    private bool CanMove()
    {
        return (TurnManager.IsPlayerTurn && !UIManager.Instance.IsScreenOpen(ScreenType.ActionMenu) &&
                !UIManager.Instance.IsScreenOpen(ScreenType.ActionMenuTech) &&
                EnableMovement);
    }
    // Moving the cursor
    public void OnMove(InputValue value)
    {
        if (!CanMove()) return;

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

        OpenUnitFrame();
    }

    private void OpenUnitFrame()
    {
        UIManager.Instance.CloseScreen(ScreenType.ActionUnitInfo);
        if (GetTile().HasCharacter())
        {
            HoverCharacter = GetTile().GetCharacter();
            UIManager.Instance.OpenScreenAdditive(ScreenType.ActionUnitInfo);
        }
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
        _transform.position = bottomPos;
    }
    public void Follow(GameObject targetObj)
    {
        if (targetObj == null)
        {
            _transform.SetParent(null);
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
        
        _transform.position = bottomPos;
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
        StartCoroutine(EnableDelay());
    }

    private IEnumerator EnableDelay()
    {
        yield return new WaitForSeconds(0.1f);
        EnableClick = true;
        EnableMovement = true;
    }
    
    // AI
    private void BeginAI()
    {
        _activeCharacter = TurnManager.GetActivePlayer();
        StartCoroutine(AIActions());
    }

    private void AIClick()
    {
        _isAIClick = true;
        OnClick();
        _isAIClick = false;
    }
    
    private IEnumerator AIActions()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject targetCharacter = _activeCharacter.Target;
        print(targetCharacter);
        float minY = targetCharacter.GetComponent<CharacterController>().ccollider.bounds.min.y;
        print(minY);
        MoveTo(targetCharacter, minY);
        OpenUnitFrame();
        yield return new WaitForSeconds(0.4f);
        AIClick();
        yield return new WaitForSeconds(0.4f);
        AIClick();
    }
}
