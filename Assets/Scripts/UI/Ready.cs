using UnityEngine;
using UnityEngine.UI;

public class Ready : UIScreen
{
    [SerializeField] private Button StartButton;

    public override void Open()
    {
        base.Open();
        
        StartButton.Select();
    }
    
    public void OnSubmit()
    {
        if (!IsOpen) return;
        
        Close();
        TurnManager.StartTurns();
        print("hello");
    }
}
