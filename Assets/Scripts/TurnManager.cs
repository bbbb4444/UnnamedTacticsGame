using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{

    public static event UnityAction OnTurnEnd;

    // private static Dictionary<string, List<CharacterController>> _teamUnits = new();
    // private static Queue<string> _teamQueue = new();
    // private static Queue<CharacterController> _currentTeam = new();
    private static Queue<CharacterController> _turnOrder = new Queue<CharacterController>();
    private static CharacterController _activePlayer;
    private void Start()
    {
        //UIManager.Instance.OnActionSelected += HandleActionSelected;
        InitTeamTurnQueue();
    }

    void Update()
    {
    }
  /*  
    public static void StartTurn()
    {
        print("Turn Manager Start");
        if (_currentTeam.Count > 0)
        {
            _activePlayer = _currentTeam.Peek();
            print("Turn Manager Start " + _activePlayer.tag);
            if (_activePlayer.CompareTag("Player")) OnTurnStart?.Invoke();
            else if (_activePlayer.CompareTag("NPC"))
            {
                _activePlayer.BeginTurn();
                OnTurnStartNPC?.Invoke();
            }
        }
    }*/
    public static CharacterController GetActivePlayer()
    {
        return _activePlayer;
    }
    
    
    
/*
    public static void EndTurn()
    {
        _currentTeam.Dequeue();
        OnTurnEnd?.Invoke();
        
        if (_currentTeam.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = _teamQueue.Dequeue();
            _teamQueue.Enqueue(team);
            InitTeamTurnQueue();
        }
    }
*/
    public static void StartTurn()
    {
        print("Turn Manager Start " + _activePlayer.tag);
        _activePlayer.Ready = true;
    }
    public static void EndTurn()
    {
        _turnOrder.Enqueue(_activePlayer);
        OnTurnEnd?.Invoke();
        InitTeamTurnQueue();
    }
    static void InitTeamTurnQueue()
    {
        _activePlayer = _turnOrder.Dequeue();
        StartTurn();
    }

    public static void AddUnit(CharacterController unit)
    {
        _turnOrder.Enqueue(unit);
    }
    /*
    static void InitTeamTurnQueue()
    {
        List<CharacterController> teamList = _teamUnits[_teamQueue.Peek()];
      
        foreach (CharacterController unit in teamList)
        {
            _currentTeam.Enqueue(unit);
        }
        StartTurn();
    }
    public static void AddUnit(CharacterController unit)
    {
        List<CharacterController> list;

        if (!_teamUnits.ContainsKey(unit.tag))
        {
            list = new List<CharacterController>();
            _teamUnits[unit.tag] = list;

            if (!_teamQueue.Contains(unit.tag))
            {
                _teamQueue.Enqueue(unit.tag);
            }
        }
        else
        {
            list = _teamUnits[unit.tag];
        }

        list.Add(unit);
    }
    */
}
