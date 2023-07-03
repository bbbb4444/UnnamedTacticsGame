using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI nameLabel;
    [SerializeField] protected TextMeshProUGUI type1;
    [SerializeField] protected TextMeshProUGUI type2;
    
    
    public void SetName(string n)
    {
        nameLabel.text = n;
    }
    public void SetType1(CharType type)
    {
        type1.text = type.type.ToString();
        type1.color = type.colors[0];
    }
    public void SetType2(CharType type)
    {
        type2.text = type.type.ToString();
        type2.color = type.colors[1];
    }
}
