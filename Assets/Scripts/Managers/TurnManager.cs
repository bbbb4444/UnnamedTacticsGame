using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{

    public static event UnityAction OnTurnEnd;
    public static event UnityAction OnBattleEnter;

    public static Dictionary<GameObject, CharacterController> PlayerObjectToController { get; set; }

    private static List<CharacterController> _playerTeam = new();
    private static List<CharacterController> _playerTeamDead = new();
    private static List<CharacterController> _enemyTeam = new();
    private static List<CharacterController> _enemyTeamDead = new();
    
    private static Queue<CharacterController> _turnQueue = new Queue<CharacterController>();
    private static CharacterController _activePlayer;
    
    public static void StartTurns()
    {
        OnBattleEnter?.Invoke();

        InitializeCharacterDict();
        InitializeTurnQueue();
        StartNextTurn();
    }
    
    public static CharacterController GetActivePlayer()
    {
        return _activePlayer;
    }
    
    
    static void StartNextTurn()
    {
        _activePlayer = _turnQueue.Dequeue();
        while (IsDead(_activePlayer))
        {
            _activePlayer = _turnQueue.Dequeue();
        }
        StartTurn();
    }

    private static void StartTurn()
    {
        print("Turn Manager Start " + _activePlayer.tag);
        _activePlayer.BeginTurn();
    }

    public static void EndTurn()
    {
        _turnQueue.Enqueue(_activePlayer);
        OnTurnEnd?.Invoke();
        StartNextTurn();
    }

    static void ResetQueues()
    {
        _playerTeam.Clear();
        _playerTeamDead.Clear();
        _enemyTeam.Clear();
        _enemyTeamDead.Clear();
        _turnQueue.Clear();
        PlayerObjectToController.Clear();
    }

    static void InitializeCharacterDict()
    {
        PlayerObjectToController = new Dictionary<GameObject, CharacterController>();
        for (int i = 0; i < GameManager.Instance.playerControllers.Count; i++)
        {
            PlayerObjectToController.TryAdd(GameManager.Instance.playerTeam[i], GameManager.Instance.playerControllers[i]);
        }
    }
    static void InitializeTurnQueue()
    {
        int totalPlayers = _playerTeam.Count + _enemyTeam.Count;
        List<CharacterController> tempTurnQueue = new();

        foreach (CharacterController player in _playerTeam)
        {
            print(player.CharType.type);
            tempTurnQueue.Add(player);
        }
        foreach (CharacterController enemy in _enemyTeam)
        {
            tempTurnQueue.Add(enemy);
        }
        
        tempTurnQueue.Sort( (char1, char2) => char2.Stats.GetStat(Stat.Speed).CompareTo(char1.Stats.GetStat(Stat.Speed)));
        
        foreach (CharacterController cc in tempTurnQueue)
        {
            cc.Reset();
            _turnQueue.Enqueue(cc);
        }
    }



    public static void AddUnit(CharacterController unit)
    {
        if (unit.CompareTag("Player")) _playerTeam.Add(unit);
        else if (unit.CompareTag("NPC")) _enemyTeam.Add(unit);
    }
    
    public static void RemoveUnit(CharacterController unit)
    {
        print(unit.tag);
        if (unit.CompareTag("Player"))
        {
            _playerTeamDead.Add(unit);
            if (_playerTeamDead.Count == _playerTeam.Count)
            {
                GetActivePlayer().Ready = false;
                ResetQueues();
                GameManager.Instance.LoseBattle();
            }
        }
        else if (unit.CompareTag("NPC"))
        {
            _enemyTeamDead.Add(unit);
            if (_enemyTeamDead.Count == _enemyTeam.Count)
            {
                GetActivePlayer().Ready = false;
                ResetQueues();
                GameManager.Instance.WinBattle();
            }
        }
    }

    private static bool IsDead(CharacterController unit)
    {
        if (unit.CompareTag("Player")) return _playerTeamDead.Contains(unit);
        else if (unit.CompareTag("NPC")) return _enemyTeamDead.Contains(unit);
        else
        {
            print("error, unit is neither player nor npc");
            return false;
        }
    }
}
