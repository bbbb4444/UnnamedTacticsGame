using System.Collections.Generic;
using UnityEngine;

public class TechHandler : MonoBehaviour
{
    private CharacterController _controller;
    private CharStats _stats;
    [SerializeField]
    public List<Technique> Techinques = new();

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (Techinques.Count == 0) AddTech(_controller.Stats.movePool[0]);
        if (CompareTag("Player")) AddTech(Resources.Load<Technique>("OPGG"));
    }

    public Technique SelectedTech { get; set; }
    
    public void AddTech(Technique tech)
    {
        Techinques.Add(Instantiate(tech));
    }
    
    public Technique GetTech(int index)
    {
        return Techinques[index];
    }

    public int GetPP(Technique tech)
    {
        return tech.pp;
    }

    public void AddPP(Technique tech, int valueToAdd)
    {
        tech.pp += valueToAdd;
    }

    public void ShowArea(Tile center)
    {
        _controller.tileSelector.ResetSelectableTiles();
        _controller.tileSelector.FindSelectableTechTiles(SelectedTech.AOE, center);
    }

    public bool CanUse(Technique tech)
    {
        return tech.cooldownCur <= 0;
    }
    
    public void ReduceCooldowns(int value = 1)
    {
        foreach (Technique tech in Techinques)
        {
            tech.cooldownCur -= value;
            if (tech.cooldownCur < 0) tech.cooldownCur = 0;
        }
    }
    
    public void UseSelectedTech(List<CharacterController> targets)
    {
        SelectedTech.cooldownCur = SelectedTech.cooldownMax;
        
        if (SelectedTech.target == Technique.Target.Ally)
        {
            List<CharacterController> allyTargets =
                new List<CharacterController>(targets).FindAll(obj => obj.CompareTag(tag));
            BattleManager.PerformTech(SelectedTech, _controller, allyTargets);
        }
        
        else if (SelectedTech.target == Technique.Target.Enemy)
        {
            List<CharacterController> enemyTargets =
                new List<CharacterController>(targets).FindAll(obj => !obj.CompareTag(tag));
            BattleManager.PerformTech(SelectedTech, _controller, enemyTargets);
        }
    }
}

