using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    public Transform Transform { get; set; }
    public Vector3 SpawnPos { get; set; }
    public bool Available { get; set; }
    
    void Start()
    {
        Transform = transform;
        SpawnPos = Transform.position + new Vector3(0,0.5f,0);

        GetComponent<Renderer>().enabled = false;

    }
}
