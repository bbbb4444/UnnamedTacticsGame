using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Element
{
    Ground = 0,
    Plant = 1,
    Water = 2,
    Fire = 3,
    Air = 4
}
public enum Augment
{
    Light = 5,
    Dark = 10
}
public enum Type
{
    Ground = 0,
    Plant = 1,
    Water = 2,
    Fire = 3,
    Air = 4,

    Crystal = 5,
    Flower = 6,
    Rainbow = 7,
    Solar = 8,
    Electric = 9,

    Abyss = 10,
    Poison = 11,
    Murk = 12,
    Hell = 13,
    Ghost = 14
}

public enum Mod
{
    WaterDefense
}
public class TypeManager : MonoBehaviour
{
    private Dictionary<Element, Dictionary<Element, int>> _typeEffectiveness = new Dictionary<Element, Dictionary<Element, int>>
    {
        { Element.Ground, new Dictionary<Element, int>
            {
                { Element.Ground, 0 },
                { Element.Plant, 0 },
                { Element.Water, 1 },
                { Element.Fire, 0 },
                { Element.Air, -1 }
            }
        },
        
        { Element.Plant, new Dictionary<Element, int>
            {
                { Element.Ground, 0 },
                { Element.Plant, -1 },
                { Element.Water, -1 },
                { Element.Fire, 1 },
                { Element.Air, 1 }
            }
        },
        
        { Element.Water, new Dictionary<Element, int>
            {
                { Element.Ground, -1 },
                { Element.Plant, 1 },
                { Element.Water, 0 },
                { Element.Fire, -1 },
                { Element.Air, 0 }
            }
        },
        
        { Element.Fire, new Dictionary<Element, int>
            {
                { Element.Ground, 1 },
                { Element.Plant, -1 },
                { Element.Water, 1 },
                { Element.Fire, -1 },
                { Element.Air, 0 }
            }
        },
        
        { Element.Air, new Dictionary<Element, int>
            {
                { Element.Ground, -1 },
                { Element.Plant, -1 },
                { Element.Water, 1 },
                { Element.Fire, 1 },
                { Element.Air, 0 }
            }
        }
    };

    public int GetEffectiveness(Element attacker, Element defender)
    {
        if (_typeEffectiveness.TryGetValue(defender, out var defenderEffectiveness))
        {
            if (defenderEffectiveness.TryGetValue(attacker, out var effectiveness))
            {
                return effectiveness;
            }
        }

        return 0; // Default effectiveness value
    }

    public static Type CalculateType(Element element, Augment augment)
    {
        Type type = (Type) ((int)element + (int)augment);
        return type;
    }

    private void Start()
    {
    }
}
