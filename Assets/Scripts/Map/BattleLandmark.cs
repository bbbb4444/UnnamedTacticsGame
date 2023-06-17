using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLandmark : Landmark
{
    [SerializeField] public List<GameObject> enemyTeam;
 
    public override void EnterLandmark()
    {
        GameManager.Instance.enemyTeam = enemyTeam;
        print(GameManager.Instance.enemyTeam.Count);
        SceneSwitcher.SwitchToScene("SampleScene");
        UIManager.Instance.OpenScreen(ScreenType.Ready);
        StartCoroutine(OpenScreenAfterDelay());
        
        base.EnterLandmark();
    }
    
    private IEnumerator OpenScreenAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
