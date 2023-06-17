using System.Collections.Generic;
using UnityEngine;

public class TechHandler : MonoBehaviour
{
    private CharacterController _controller;
    private CharStats _stats;
    [SerializeField]
    public List<Technique> Techinques = new List<Technique>();
    //private Dictionary<Technique, int> _techToPP;
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        /*
        _techToPP = new Dictionary<Technique, int>
        {
            { Techinques[0], Techinques[0].pp },
            { Techinques[1], Techinques[1].pp },
            { Techinques[2], Techinques[2].pp },
            { Techinques[3], Techinques[3].pp }
        };
        */
    }

    public Technique SelectedTech { get; set; }
    
    public void Activate()
    {
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
        _controller.tileSelector.FindSelectableTiles(100, SelectedTech.AOE, center, Tile.State.Tech, RangeStyle.Circle);
    }
}

