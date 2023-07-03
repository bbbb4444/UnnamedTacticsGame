using UnityEngine;

public class Line : MonoBehaviour
{
    private Transform _transform;
    private Renderer _renderer;
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _renderer = GetComponentInChildren<Renderer>();
        
        _renderer.material.color = Color.gray;
        Vector3 pos = _transform.position;
        _transform.position = new Vector3(pos.x, 0, pos.z);
    }

    public void Extend(float length, Vector3 direction)
    {
        _transform.localScale += new Vector3(0,0,length-1);
        direction = new Vector3(direction.x, 0, direction.z);
        _transform.LookAt(direction);
    }

    public void ColorLine()
    {
        _renderer.material.color = Color.yellow;
    }
}
