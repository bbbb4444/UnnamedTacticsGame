using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for BattleLandmark and EventLandmark. Contains methods common to both.
/// </summary>
public class Landmark : MonoBehaviour
{
    [SerializeField] private GameObject LinePrefab;
    private List<GameObject> _lineObjects = new();
    private List<Line> _lines = new();

    [SerializeField] public List<Landmark> nextLandmarks = new();
    [SerializeField] public List<Landmark> prevLandmarks = new();
    public Landmark NeighborAbove;
    public Landmark NeighborBelow;
    
    [SerializeField] public bool IsCurrent { get; set; }
    public bool IsActive { get; set; }
    public bool IsCompleted { get; set; }
    
    private Renderer _renderer;
    protected GameObject Appearance;    

    private Color _color = Color.gray;
    private Color _inactiveColor = Color.gray;
    private Color _highlight = new(40/255f, 40/255f, 40/255f);
    private Color _activatedColor = new(105/255f, 226/255f, 119/255f);
    private Color _completedColor = new(176/255f, 225/255f, 182/255f);

    public static event UnityAction OnComplete;
    public static event UnityAction OnEnter;
    
    /// <summary>
    /// Creates the appearance of the landmark and sets the position.
    /// </summary>
    protected virtual void CreateAppearance()
    {
        Appearance.transform.position = transform.position;
        Appearance.transform.SetParent(transform);
    }
    
    protected virtual void Awake()
    {
        LinePrefab = Instantiate(Resources.Load<GameObject>("MapLine"));
        CreateAppearance();
        _renderer = Appearance.GetComponent<Renderer>();
        _renderer.materials[0].color = _color;
        StartCoroutine(DelayedSetup());
    }
    
    /// <summary>
    /// Called when entering the landmark.
    /// </summary>
    public virtual void EnterLandmark()
    {
        print(LandmarkManager.Instance.current);
        OnEnter?.Invoke();
    }
    
    /// <summary>
    /// Connects the lines of the landmark to the next landmarks.
    /// </summary>
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
    
    /// <summary>
    /// Colors each line the landmark has.
    /// </summary>
    private void ColorLines()
    {
        foreach (Line line in _lines)
        {
            line.ColorLine();
        }
    }
    
    /// <summary>
    /// Selects the landmark.
    /// </summary>
    public void Select()
    {
        SetColor(_color + _highlight);
    }
    
    /// <summary>
    /// Activates the landmark.
    /// </summary>
    public void Activate()
    {
        SetColor(_activatedColor);
        IsActive = true;
    }

    /// <summary>
    /// Completes the landmark.
    /// </summary>
    public virtual void Complete()
    {
        SetColor(_completedColor);
        IsCompleted = true;
        ColorLines();
        
        foreach (Landmark landmark in nextLandmarks)
        {
            landmark.Activate();
        }

        OnComplete?.Invoke();
    }
    
    /// <summary>
    /// Sets the color of the landmark.
    /// </summary>
    /// <param name="color">The color to set the landmark to.</param>
    private void SetColor(Color color)
    {
        _color = color;
        _renderer.materials[0].color = _color; 
    }
    /// <summary>
    /// Resets the color of the landmark based on its state.
    /// </summary>
    public void ResetColor()
    {
        if (IsCompleted) SetColor(_completedColor);
        else if (IsActive) SetColor(_activatedColor);
        else SetColor(_inactiveColor);
    }

    //Setup must be delayed by at least 1 frame to ensure all references of the landmarks are created.
    private IEnumerator DelayedSetup()
    {
        yield return new WaitForSeconds(0f);
        GetNeighbors();
        ConnectNextLandmarks();
    }
    
    /// <summary>
    /// Gets the neighboring landmarks above and below the current landmark.
    /// </summary>
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
}