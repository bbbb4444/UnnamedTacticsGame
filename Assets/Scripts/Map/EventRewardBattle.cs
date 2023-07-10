using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRewardBattle : Event
{
    
    public void OnUnitOptionClicked()
    {
        UIManager.Instance.OpenScreen(ScreenType.ChooseYourRewardCharacter);
    }

    public void OnTechniqueOptionClicked()
    {
        UIManager.Instance.OpenScreen(ScreenType.ChooseYourRewardTechnique);
    }
    
    public override void OnContinueButtonClicked()
    {
        UIManager.Instance.CloseAllScreens();
    }
}
