using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionCharacter : Option
{
    [SerializeField] private List<TextMeshProUGUI> statTexts;
    [SerializeField] private List<TextMeshProUGUI> techTexts;
    
    public void SetStats(CharStats stats)
    {
        statTexts[0].text = "HP: " + stats.GetStat(Stat.Health);
        statTexts[1].text = "PWR: " + stats.GetStat(Stat.Strength);
        statTexts[2].text = "DEF: " + stats.GetStat(Stat.Defense);
        statTexts[3].text = "SPD: " + stats.GetStat(Stat.Speed);
    }
    public void SetTechs(List<Technique> techniques)
    {
        for (int i = 0; i < techniques.Count; i++)
        {
            techTexts[i].text = techniques[i].techName;
        }
    }
}
