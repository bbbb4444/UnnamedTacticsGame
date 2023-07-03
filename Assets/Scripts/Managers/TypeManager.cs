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
    None = 0,
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
    Ghost = 14,
    None
}

public enum Bonus
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
    [SerializeField]
    public static Sprite ground;
    public static Sprite water;
    public static Sprite plant;
    public static Sprite fire;
    public static Sprite air;
    public static Sprite crystal;
    public static Sprite rainbow;
    public static Sprite flower;
    public static Sprite solar;
    public static Sprite electric;
    public static Sprite abyss;
    public static Sprite murk;
    public static Sprite poison;
    public static Sprite hell;
    public static Sprite ghost;
    private Dictionary<Type, Sprite> typeToIcon = new Dictionary<Type, Sprite>
    {
        {Type.Ground, ground},
        {Type.Water, water},
        {Type.Plant, plant},
        {Type.Fire, fire},
        {Type.Air, air},
        {Type.Crystal, crystal},
        {Type.Rainbow, rainbow},
        {Type.Flower, flower},
        {Type.Solar, solar},
        {Type.Electric, electric},
        {Type.Abyss, abyss},
        {Type.Murk, murk},
        {Type.Poison, poison},
        {Type.Hell, hell},
        {Type.Ghost, ghost}
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

    public Sprite GetIcon(Type type)
    {
        return typeToIcon[type];
    }
    
    public static Type CalculateType(Element element, Augment augment)
    {
        Type type = (Type) ((int)element + (int)augment);
        return type;
    }
    
}
