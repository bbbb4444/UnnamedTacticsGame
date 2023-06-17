using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionMenu : UIScreen
{
    public GameObject actionMenuPanel;
    public Button moveButton;
    public Button attackButton;
    public Button techButton;
    public Button itemButton;
    public Button defendButton;
    
    public static event UnityAction OnMoveSelected;
    public static event UnityAction OnAttackSelected;
    public static event UnityAction OnTechSelected;
    public static event UnityAction OnItemSelected;
    public static event UnityAction OnDefendSelected;
    public static ActionType currentAction = ActionType.None;
    public enum ActionType
    {
        None,       // Default or invalid action
        Move,       // Movement action
        Attack,     // Attack action
        Tech,      // Technique action
        Item,       // Item action
        Defend      // Defend action
    }

    private void Update()
    {
        
    }

    
    protected override void Awake()
    {
        base.Awake();
        actionMenuPanel = GameObject.FindGameObjectWithTag("ActionMenuPanel");
    }

    public override void Open()
    {
        base.Open();
        
        TurnManager.GetActivePlayer().tileSelector.ResetSelectableTiles();
        EnableActionButtons();
        if (moveButton.interactable) moveButton.Select();
        else attackButton.Select();
    }

    public override void Close()
    {
        if (!IsOpen) return;
        
        DisableOtherActionButtons();
        
        base.Close();
    }
    private void OnEnable()
    {
        ActionMenuTech.GoBack += EnableActionButtons;
        
        CharacterController.OnEndMovePhase += DisableMoveButton;
        CharacterController.OnEndOtherActionPhase += DisableOtherActionButtons;
        
        AIHandler.PrepareMove += AISelectMove;
        AIHandler.PrepareTech += AISelectTech;
    }

    private void OnDisable()
    {
        ActionMenuTech.GoBack -= EnableActionButtons;
        
        CharacterController.OnEndMovePhase -= DisableMoveButton;
        CharacterController.OnEndOtherActionPhase -= DisableOtherActionButtons;
        
        AIHandler.PrepareMove -= AISelectMove;
        AIHandler.PrepareTech -= AISelectTech;
    }
    

    public void HideActionMenu()
    {
        
    }

    public void EnableActionButtons()
    {
        moveButton.interactable = true;
        attackButton.interactable = true;
        techButton.interactable = true;
        itemButton.interactable = true;
        defendButton.interactable = true;
    }

    public void DisableMoveButton()
    {
        moveButton.interactable = false;
    }
    public void DisableOtherActionButtons()
    {
        moveButton.interactable = false;
        attackButton.interactable = false;
        techButton.interactable = false;
        itemButton.interactable = false;
        defendButton.interactable = false;
    }
    
    public void OnMoveButtonClicked()
    {
        currentAction = ActionType.Move;
        OnMoveSelected?.Invoke();
        if (TurnManager.GetActivePlayer().CompareTag("Player"))
        {
            TurnManager.GetActivePlayer().GetMovement().FindSelectableTiles();
        }
        UIManager.Instance.CloseScreen(ScreenType.ActionMenu);
    }

    public void OnAttackButtonClicked()
    {
        //currentAction = ActionType.Attack;
        //OnAttackSelected?.Invoke();
        //HideActionMenu();
    }

    public void OnTechButtonClicked()
    {
        currentAction = ActionType.Tech;
        UIManager.Instance.OpenScreen(ScreenType.ActionMenuTech);
    }

    public void OnItemButtonClicked()
    {
        OnItemSelected?.Invoke();
        HideActionMenu();
    }

    public void OnDefendButtonClicked()
    {
        OnDefendSelected?.Invoke();
        HideActionMenu();
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