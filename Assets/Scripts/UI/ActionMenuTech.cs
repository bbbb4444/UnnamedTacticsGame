using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionMenuTech : UIScreen
{
    public static UnityAction OnTechClicked;
    public static UnityAction GoBack;
    
    private bool _firstNavigation = true;
    public Canvas canvas;
    private Technique[] _techs = new Technique[4];
    private Technique _currentTech;

    private TechHandler _techHandler;
    
    public Button techButton1;
    public Button techButton2;
    public Button techButton3;
    public Button techButton4;
    public Button backButton;
    private Button[] _techButtons;
    private Dictionary<Button, Technique> ButtonToTech = new Dictionary<Button, Technique>();


    private TextMeshProUGUI[] _techLabels = new TextMeshProUGUI[4];

    // Info panel
    public TextMeshProUGUI powerLabel;
    public TextMeshProUGUI ppLabel;
    public Image typeIcon;
    

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        
        SetupTechNameReferences();
        EnableButtons(false);
        
        AIHandler.SelectTech += BeginAI;
    }

    private void OnDisable()
    {
        AIHandler.SelectTech -= BeginAI;
    }

    private void SetupTechNameReferences()
    {
        _techButtons = new[] { techButton1, techButton2, techButton3, techButton4 };
        for (int i = 0; i < _techLabels.Length; i++)
        {
            TextMeshProUGUI label = _techButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            _techLabels[i] = label;
        }
    }
    
    // Setup to show menu
    public override void Open()
    {
        base.Open();
        
        EnableButtons(true);
        CharacterController activeChar = TurnManager.GetActivePlayer();
        activeChar.tileSelector.ResetSelectableTiles();
        _techHandler = activeChar.GetTechHandler();
        
        SetupTechs(_techHandler);
        SetupTechNames();

        _techButtons[0].Select();
        UpdateTechInfo();
    }

    public override void Close()
    {
        EnableButtons(false);
        base.Close();
    }
    private void SetupTechs(TechHandler th)
    {
        ButtonToTech.Clear();
        
        for (int i = 0; i < _techs.Length; i++)
        {
            Technique tech = th.GetTech(i);
            _techs[i] = tech;
            
            ButtonToTech.Add(_techButtons[i], _techs[i]);
        }
    }

    
    private void SetupTechNames()
    {
        for (int i = 0; i < _techLabels.Length; i++)
        {
            if (_techs[i] == null)
            {
                _techLabels[i].text = "None";
            }
            else _techLabels[i].text = _techs[i].techName;
        }
    }
    
    void EnableButtons(bool b)
    {
        foreach (Button button in _techButtons)
        {
            button.interactable = b;
        }
    }
    

    
    // Buttons
    public void OnSubmit()
    {
        if (!IsOpen) return;
        
        print("SElECTED: " + _currentTech.techName);
        _techHandler.SelectedTech = _currentTech;
        TurnManager.GetActivePlayer().TechTarget(_currentTech);
        
        UIManager.Instance.CloseScreen(ScreenType.ActionMenuTech);
    }
    
    
    // Navigating
    public void OnNavigate()
    {
        if (!canvas.enabled) return;
        
        if (_firstNavigation)
        {
            _firstNavigation = false;
            return;
        }
        UpdateTechInfo();
        _firstNavigation = true;
    }

    public void OnCancel()
    {
         UIManager.Instance.OpenScreen(UIManager.Instance.LastScreen);   
    }

    private void UpdateTechInfo()
    {
        Button currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        _currentTech = ButtonToTech[currentButton];
        //if (_currentTech.tech == Tech.None) return;
        
        powerLabel.text = _currentTech.power.ToString();
        ppLabel.text =  _techHandler.GetPP(_currentTech) + "/" + _currentTech.pp;
        typeIcon.sprite = _currentTech.type.icon;
    }

    
    
    
    // AI
    private void BeginAI()
    {
        print("Begin AI Tech");
        StartCoroutine(AIActions());
}

    private IEnumerator AIActions()
    {
        Button currentButton = GetTechButton();
        yield return new WaitForSeconds(0.5f);
        currentButton.Select();
        UpdateTechInfo();
        yield return new WaitForSeconds(1f);
        OnSubmit();
        OnTechClicked?.Invoke();
    }
    private Button GetTechButton()
    {
        foreach (KeyValuePair<Button, Technique> entry in ButtonToTech)
        {
            if (entry.Value == _techHandler.SelectedTech)
            {
                return entry.Key;
            }
        }
        print("No button found. Something went terribly wrong.");
        return _techButtons[0];
    }
    /*
    void AISelectMove()
    {
        moveButton.Select();
        StartCoroutine(WaitSeconds(moveButton.onClick.Invoke, 1));
    }

    void AISelectAttack()
    {
        attackButton.Select();
        StartCoroutine(WaitSeconds(moveButton.onClick.Invoke, 1));
    }
    
    IEnumerator WaitSeconds(UnityAction method, int sec)
    {
        yield return new WaitForSeconds(sec);
        method.Invoke();
    }
    */
}