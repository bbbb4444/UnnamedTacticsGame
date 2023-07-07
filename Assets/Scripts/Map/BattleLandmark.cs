using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLandmark : Landmark
{
    [SerializeField] public List<GameObject> enemyTeam;
    private EnemyPool.Pool pool;
    private int EnemyCount { get; set; }
    protected override void Awake()
    {
        base.Awake();

        pool = (EnemyPool.Pool) Random.Range(0, 2);
        EnemyCount = Random.Range(2, 3);
        enemyTeam = EnemyPool.Instance.GetRandomEnemies(pool, EnemyCount);
    }

    public override void Complete()
    {
        base.Complete();
        StartCoroutine(ReselectAfterDelay());
        
        UIManager.Instance.OpenScreen(ScreenType.ChooseYourRewardCharacter);
    }

    private IEnumerator ReselectAfterDelay()
    {
        yield return new WaitForSeconds(0f);
        
        LandmarkManager.Instance.SelectLandmark(this);
    }
    
    public override void EnterLandmark()
    {
        GameManager.Instance.enemyTeam = enemyTeam;
        SceneSwitcher.SwitchToScene("SampleScene");

        base.EnterLandmark();
        
        LandmarkManager.Instance.gameObject.SetActive(false);
    }
    

    protected override void CreateAppearance()
    {
        Appearance = GameObject.CreatePrimitive(PrimitiveType.Cube);
        base.CreateAppearance();
    }
}
