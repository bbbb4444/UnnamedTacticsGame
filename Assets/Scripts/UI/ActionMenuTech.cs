using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionMenuTech : UIScreen
{
    private PlayerInputController _playerInputController;
    
    public static UnityAction OnTechClicked;
    
    private bool _firstNavigation = true;
    private List<Technique> _techs = new();
    private Technique _currentTech;
    [SerializeField] private Technique none;
    private TechHandler _techHandler;
    
    public Button techButton1;
    public Button techButton2;
    public Button techButton3;
    public Button techButton4;
    private Button[] _techButtons;
    private Dictionary<Button, Technique> ButtonToTech = new Dictionary<Button, Technique>();


    private TextMeshProUGUI[] _techLabels = new TextMeshProUGUI[4];

    // Info panel
    public TextMeshProUGUI powerLabel;
    public TextMeshProUGUI ppLabel;
    public Image typeIcon;
    
    protected override void Awake()
    {
        base.Awake();

        _playerInputController = GetComponent<PlayerInputController>();
    }
    
    private void Start()
    {
        SetupTechNameReferences();
    }

    private void OnEnable()
    {
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
        
        CharacterController activeChar = TurnManager.GetActivePlayer();
        activeChar.tileSelector.ResetSelectableTiles();
        _techHandler = activeChar.TechHandler;
        
        SetupTechs(_techHandler);
        SetupTechNames();
        
        _techButtons[0].Select();
        UpdateTechInfo();
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) _techButtons[0].Select();
    }

    private void SetupTechs(TechHandler th)
    {
        ButtonToTech.Clear();
        _techs.Clear();
        
        List<Technique> techs = th.Techinques;
        
        for (int i = 0; i < _techLabels.Length; i++)
        {
            if (i < techs.Count)
            {
                Technique tech = techs[i];
                _techs.Add(tech);
                ButtonToTech.Add(_techButtons[i], _techs[i]);
            }
            else
            {
                _techs.Add(none);
                ButtonToTech.TryAdd(_techButtons[i], _techs[i]);
            }
        }
    }

    
    private void SetupTechNames()
    {
        for (int i = 0; i < _techLabels.Length; i++)
        {
            if (i >= _techs.Count)
            {
                _techLabels[i].text = "None";
            }
            else _techLabels[i].text = _techs[i].techName;
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
        if (!IsOpen) return;
        
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
         UIManager.Instance.OpenScreen(lastScreen);   
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
        yield return new WaitForSeconds(0.25f);
        currentButton.Select();
        UpdateTechInfo();
        yield return new WaitForSeconds(0.25f);
        OnSubmit();
        OnTechClicked?.Invoke();
    }
    private Button GetTechButton()
    {
        if (ButtonToTech.Count == 0) SetupTechs(_techHandler);
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