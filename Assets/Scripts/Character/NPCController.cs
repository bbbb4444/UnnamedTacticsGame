using Unity.VisualScripting;
using UnityEngine;

public class NPCController : CharacterController
{
    public AIHandler _aiHandler;

    protected override void Awake()
    {
        _aiHandler = GetComponent<AIHandler>() ? GetComponent<AIHandler>() : this.AddComponent<AIHandler>();
        base.Awake();
    }
    
    public override void BeginPhase()
    {
        Ready = false;
        if (Actions > 0)
        {
            UIManager.Instance.OpenScreen(ScreenType.ActionMenu);
            InvokeOnPhaseStart();
            StartCoroutine(_aiHandler.BeginPhase());
        }
        else EndTurn();
    }
    
    
    public override void FindNearestTarget()
    {
        print("Finding nearest target");
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            float d = Vector3.SqrMagnitude(transform.position - obj.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }

        Target = nearest;
    }
}
