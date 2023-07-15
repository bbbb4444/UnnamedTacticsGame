using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class TechHandler : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    private CharStats _stats;
    [SerializeField]
    public List<Technique> Techinques = new();
    
    public void Initialize()
    {
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController>();
        if (Techinques.Count == 0)
        {
            List<Technique> movePool = _controller.Stats.movePool;
            for (int i = 0; i < _controller.Stats.startingMoves; i++)
            {
                int rand;
                do rand = Random.Range(0, movePool.Count);
                while (Techinques.Contains(movePool[rand]));
                Techinques.Add(movePool[rand]);
            }
            if (CompareTag("Player")) AddTech(Resources.Load<Technique>("OPGG"));
        }
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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_controller == TurnManager.GetActivePlayer()) _animator.Play("Cast Technique");
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

