using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Landmark : MonoBehaviour
{
    [SerializeField] private GameObject LinePrefab;
    private List<GameObject> _lineObjects = new();
    private List<Line> _lines = new();
    
    [SerializeField] public List<Landmark> nextLandmarks = new();
    [SerializeField] public Landmark previousLandmark;
    public Landmark NeighborAbove { get; private set; }
    public Landmark NeighborBelow { get; private set; }
    
    [SerializeField] public bool IsCurrent { get; set; }
    public bool IsActive { get; set; }
    public bool IsCompleted { get; set; }
    
    private Renderer _renderer;
    
    private Color _color = Color.gray;
    private Color _inactiveColor = Color.gray;
    private Color _highlight = new(40/255f, 40/255f, 40/255f);
    private Color _activatedColor = new(105/255f, 226/255f, 119/255f);
    private Color _completedColor = new(176/255f, 225/255f, 182/255f);

    public static event UnityAction OnComplete;
    public static event UnityAction OnEnter;
    
    private void Start()
    {
        //_line = GetComponentInChildren<Line>();
        _renderer = GetComponent<Renderer>();
        _renderer.materials[0].color = _color;
        GetNeighbors();
        //ConnectNextLandmarks();
    }

    public virtual void EnterLandmark()
    {
        OnEnter?.Invoke();
    }

    private void ConnectNextLandmarks()
    {
        if (nextLandmarks.Count == 0) return;

        {
            for (int i = 0; i < nextLandmarks.Count; i++)
            {
                _lineObjects.Add(Instantiate(LinePrefab, transform));
                _lines.Add(_lineObjects[i].GetComponent<Line>());

                Vector3 nextLandmarkPos = nextLandmarks[i].transform.position;
                float lineLength = Vector3.Distance(transform.position, nextLandmarkPos);
                _lines[i].Extend(lineLength, nextLandmarkPos);
            }

        }
    }

    public void Select()
    {
        SetColor(_color + _highlight);
    }
    public void Activate()
    {
        SetColor(_activatedColor);
        IsActive = true;
    }

    public void Complete()
    {
        SetColor(_completedColor);
        IsCompleted = true;
        ConnectNextLandmarks();
        
        foreach (Landmark landmark in nextLandmarks)
        {
            landmark.Activate();
        }

        OnComplete?.Invoke();
    }

    private void SetColor(Color color)
    {
        _color = color;
        _renderer.materials[0].color = _color; 
    }

    public void ResetColor()
    {
        if (IsCompleted) SetColor(_completedColor);
        else if (IsActive) SetColor(_activatedColor);
        else SetColor(_inactiveColor);
    }
    
    public void GetNeighbors()
    {
        int layer = LayerMask.NameToLayer("Landmark");
        int layerMask = 1 << layer;
        RaycastHit landmarkHit;
        
        Ray ray = new Ray(transform.position, Vector3.forward);
        if (Physics.Raycast(ray,out landmarkHit, 20, layerMask))
        {
            NeighborAbove = landmarkHit.collider.GetComponent<Landmark>();
        }
        
        ray = new Ray(transform.position, Vector3.back);
        if (Physics.Raycast(ray,out landmarkHit, 20, layerMask))
        {
            NeighborBelow = landmarkHit.collider.GetComponent<Landmark>();
        }
    }

    public void GetNeighborBelow()
    {
        
    }
}
