using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Ready : UIScreen
{
    [SerializeField] private Button startButton;

    public override void Open()
    {
        base.Open();
        
        startButton.Select();
    }
    
    public void OnSubmit()
    {
        if (!IsOpen) return;
        
        Close();
        TurnManager.StartTurns();
    }
}
