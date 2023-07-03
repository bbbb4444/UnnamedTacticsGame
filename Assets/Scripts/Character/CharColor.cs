using UnityEngine;

public class CharColor : MonoBehaviour
{
    private CharType Type { get; set; }
    private Renderer Renderer { get; set; }

    void Start()
    {
        Type = GetComponent<CharacterController>().CharType;
        Renderer = GetComponentsInChildren<Renderer>()[1];

        UpdateColor(Type);
    }

    public void UpdateColor(CharType type)
    {
        if (!Type) return;
        
        Renderer.materials[0].color = type.colors[0]; 
        if (type.colors.Count == 2) 
        { 
            Renderer.materials[1].color = type.colors[1]; 
        }
        else 
        { 
            Renderer.materials[1].color = type.colors[0]; 
        }
    }
}
