using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    public GameObject actionMenuPanel;
    public Button moveButton;
    public Button attackButton;
    public Button skillButton;
    public Button itemButton;
    public Button defendButton;
    
    public static event UnityAction OnMoveSelected;
    public static event UnityAction OnAttackSelected;
    public static event UnityAction OnSkillSelected;
    public static event UnityAction OnItemSelected;
    public static event UnityAction OnDefendSelected;
    public static ActionType currentAction = ActionType.None;
    public enum ActionType
    {
        None,       // Default or invalid action
        Move,       // Movement action
        Attack,     // Attack action
        Magic,      // Magic action
        Item,       // Item action
        Defend      // Defend action
    }
    private void Awake()
    {
        actionMenuPanel = GameObject.FindGameObjectWithTag("ActionMenuPanel");
    }

    private void OnEnable()
    {
        CharacterController.OnPhaseStart += EnableActionButtons;
        CharacterController.OnPhaseStart += ShowActionMenu;

        CharacterController.OnLeftOverActions += ShowActionMenu;

        CharacterController.OnEndMovePhase += DisableMoveButton;
        CharacterController.OnEndOtherActionPhase += DisableOtherActionButtons;
    }

    private void OnDisable()
    {
        CharacterController.OnTurnStart -= ShowActionMenu;
    }

    
    
    public void ShowActionMenu()
    {
        UIManager.OpenScreen(ScreenType.ActionMenu);
        if (moveButton.interactable) moveButton.Select();
        else attackButton.Select();
    }

    public void HideActionMenu()
    {
        
    }

    public void EnableActionButtons()
    {
        moveButton.interactable = true;
        attackButton.interactable = true;
        skillButton.interactable = true;
        itemButton.interactable = true;
        defendButton.interactable = true;
    }

    public void DisableMoveButton()
    {
        print("Disabled MOVE BUTTON");
        moveButton.interactable = false;
    }
    public void DisableOtherActionButtons()
    {
        moveButton.interactable = false;
        attackButton.interactable = false;
        skillButton.interactable = false;
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
        UIManager.CloseScreen(ScreenType.ActionMenu);
    }

    public void OnAttackButtonClicked()
    {
        currentAction = ActionType.Attack;
        OnAttackSelected?.Invoke();
        HideActionMenu();
    }

    public void OnSkillButtonClicked()
    {
        OnSkillSelected?.Invoke();
        HideActionMenu();
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
}