using UnityEngine;

public class CharColor : MonoBehaviour
{
    private CharStats Stats { get; set; }
    private Renderer Renderer { get; set; }

    void Start()
    {
        Stats = GetComponent<CharacterController>().GetStats();
        Renderer = GetComponentsInChildren<Renderer>()[1];

        Renderer.materials[0].color = Stats.type.colors[0];
        if (Stats.type.colors.Count == 2)
        {
            Renderer.materials[1].color = Stats.type.colors[1];
        }
        else
        {
            Renderer.materials[1].color = Stats.type.colors[0];
        }
    }
}
