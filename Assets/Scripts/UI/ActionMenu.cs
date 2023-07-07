using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ActionMenu : UIScreen
{
    private PlayerInputController _playerInputController;
    
    public Button moveButton;
    public Button techButton;
    public Button waitButton;

    public static event UnityAction OnMoveSelected;
    public static ActionType CurrentAction = ActionType.None;
    public enum ActionType
    {
        None,       // Default or invalid action
        Move,       // Movement action
        Tech,      // Technique action
        Wait      // Wait action
    }

    

    public override void Open()
    {
        base.Open();
        


        TurnManager.GetActivePlayer().tileSelector.ResetSelectableTiles();

        moveButton.interactable = TurnManager.GetActivePlayer().CanMove;
        techButton.interactable = TurnManager.GetActivePlayer().CanOtherAction;


        if (!TurnManager.IsPlayerTurn) UIManager.Instance.DisablePlayerInput();
        else UIManager.Instance.EnablePlayerInput();
        
        
        if (moveButton.interactable) moveButton.Select();
        else techButton.Select();
    }

    protected override void Awake()
    {
        base.Awake();

        _playerInputController = GetComponent<PlayerInputController>();
    }
    
    private void OnEnable()
    {
        AIHandler.PrepareMove += AISelectMove;
        AIHandler.PrepareTech += AISelectTech;
    }

    private void OnDisable()
    {
        AIHandler.PrepareMove -= AISelectMove;
        AIHandler.PrepareTech -= AISelectTech;
    }

    public void OnCancel()
    {
        if (!TurnManager.GetActivePlayer().CanOtherAction) return;
        if (TurnManager.GetActivePlayer().CanMove) return;
        
        TurnManager.GetActivePlayer().ResetMovement();
        TurnManager.GetActivePlayer().BeginPhase();
    }

    public void OnMoveButtonClicked()
    {
        CurrentAction = ActionType.Move;
        OnMoveSelected?.Invoke();
        if (TurnManager.IsPlayerTurn)
        {
            TurnManager.GetActivePlayer().GetMovement().FindSelectableTiles();
        }
        UIManager.Instance.CloseScreen(ScreenType.ActionMenu);
    }
    

    public void OnTechButtonClicked()
    {
        CurrentAction = ActionType.Tech;
        UIManager.Instance.OpenScreen(ScreenType.ActionMenuTech);
    }

    public void OnWaitButtonClicked()
    {
        TurnManager.GetActivePlayer().EndTurn();
    }
    
    // AI
    void AISelectMove()
    {
        moveButton.Select();
        StartCoroutine(WaitSeconds(moveButton.onClick.Invoke, 1));
    }

    void AISelectTech()
    {
        techButton.Select();
        StartCoroutine(WaitSeconds(techButton.onClick.Invoke, 1));
    }
    
    IEnumerator WaitSeconds(UnityAction method, int sec)
    {
        yield return new WaitForSeconds(sec);
        method.Invoke();
    }
}