using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event : UIScreen
{
    [SerializeField] private Button continueButton;
    [SerializeField] protected List<Button> optionButtons;
    
    public override void Open()
    {
        base.Open();

        if (continueButton)
        {
            continueButton.interactable = true;
            continueButton.Select();
            print("clicked 2");
        }
        else optionButtons[0].Select();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            OnContinueButtonClicked();
        }
    }

    public virtual void OnContinueButtonClicked()
    {
        print("clicked 1");
        if (nextScreen != ScreenType.None)
        {
            UIManager.Instance.OpenScreen(nextScreen);
        }
    }
    
}
