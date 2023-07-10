using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Event : UIScreen
{
    public static event UnityAction OnOpen;
    public static event UnityAction OnClose;
    
    [SerializeField] private Button continueButton;
    [SerializeField] protected List<Button> optionButtons;
    
    public override void Open()
    {
        OnOpen?.Invoke();
        base.Open();

        if (continueButton)
        {
            continueButton.interactable = true;
            continueButton.Select();
            print("clicked 2");
        }
        else optionButtons[0].Select();
    }

    public override void Close()
    {
        base.Close();
        OnClose?.Invoke();
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
