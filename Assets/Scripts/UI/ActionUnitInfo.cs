using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionUnitInfo : UIScreen
{
    private CursorMovement Cursor { get; set; }
    [SerializeField] private Image typeIcon;
    [SerializeField] private TextMeshProUGUI species;
    [SerializeField] private TextMeshProUGUI hpBefore;
    [SerializeField] private TextMeshProUGUI arrow;
    [SerializeField] private TextMeshProUGUI hpAfter;
    private CharacterController Unit { get; set; }
    

    private void OnEnable()
    {
        CursorMovement.OnSelectTech += ShowDamagePrediction;
    }
    private void OnDisable()
    {
        CursorMovement.OnSelectTech -= ShowDamagePrediction;
    }
    
    public override void Open()
    {
        base.Open();
        CheckCursor();
        if (Cursor)
        {
            Unit = Cursor.HoverCharacter;
            if (Unit) UpdateUI();
        }
    }

    private void CheckCursor()
    {
        if (!Cursor) Cursor = FindObjectOfType<CursorMovement>();
    }

    private void UpdateUI()
    {
        ResetDamagePrediction();
        typeIcon.sprite = Unit.CharType.icon;
        species.text = Unit.Name;
        hpBefore.text = "HP: " + (int) Unit.Stats.GetStat(Stat.Health) + "/" + (int) Unit.Stats.GetBaseStat(Stat.Health);
    }

    public void ShowDamagePrediction()
    {
        CharacterController activeUnit = TurnManager.GetActivePlayer();
        Technique selectedTech = activeUnit.TechHandler.SelectedTech;
        
        float damage = BattleManager.CalculateDamage(selectedTech, activeUnit, Unit);
        
        arrow.text = ">";
        arrow.color = BattleManager.GetEffectivenessColor(selectedTech, Unit.CharType);
        hpAfter.text = "HP: " + (int) (Unit.Stats.GetStat(Stat.Health) - damage) + "/" + (int) Unit.Stats.GetBaseStat(Stat.Health);
    }

    private void ResetDamagePrediction()
    {
        arrow.text = "";
        hpAfter.text = "";

    }
}
