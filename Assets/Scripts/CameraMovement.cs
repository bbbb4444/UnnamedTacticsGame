using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    public Camera uiCamera;
    public Camera mainCamera;
    private GameObject _cursor;
    [SerializeField] private int amountOfTiles;
    
    [SerializeField]
    private float speed = 0.25f;
    [SerializeField]
    float zoomPower = 1f;

    
    [FormerlySerializedAs("rotationDeg")] public int currentAngle = 0;
    private int _angleToRotate = 90;
    
    private Vector3 _movement;
    private bool _freeCamera = false;
    private bool _rotating;
    private bool _zooming;
    void Start()
    {
        uiCamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
        _cursor = GameObject.FindWithTag("Cursor");
    }
    
    void FixedUpdate()
    {
        if (_freeCamera)
        {
            MoveCamera();
        }
        else
        {
            MoveCameraTo(_cursor.transform.position);
        }
    }

    public void OnFreeCamera()
    {
        _cursor.GetComponent<CursorMovement>().Enable(_freeCamera);
        _freeCamera = !_freeCamera;
    }
    public void OnZoom(InputValue zoomDirection)
    {
        int zd = (int) zoomDirection.Get<float>();
        ZoomCamera(zd);
    }
    void ZoomCamera(int zoomDirection)
    {
        float newSize = Mathf.Clamp(mainCamera.orthographicSize + zoomPower*zoomDirection, 2, 7);
        mainCamera.orthographicSize = newSize;
        uiCamera.orthographicSize = newSize;
    }

    public void OnMove(InputValue value)
    {
        float x = value.Get<Vector2>().x;
        float z = value.Get<Vector2>().y;
        _movement = new Vector3(x, 0, z);
        _movement *= speed;
    }
    void MoveCamera()
    {
        transform.Translate(_movement);
    }
    
    public void OnRotate(InputValue rotateDirection)
    {
        int rd = (int) rotateDirection.Get<float>();
        StartCoroutine(RotateCamera(rd));
    }
    IEnumerator RotateCamera(int rotateDirection)
    {
        if (_rotating) yield break;
        
        _rotating = true;
        int angle = _angleToRotate * rotateDirection;
        currentAngle += angle;
        currentAngle %= 360;
        
        float lerpDuration = 0.5f;
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, angle, 0);
        while (timeElapsed < lerpDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        _rotating = false;
    }
    



    void HoverTile()
    {
        Ray ray = new Ray(transform.position - new Vector3(0,1f, 0), Vector3.up);
        // Perform the raycast and get all hits
        RaycastHit[] hits = Physics.RaycastAll(ray);

        // Find the highest hit
        foreach (RaycastHit hit in hits)
        {
            
            Tile tile = hit.collider.GetComponent<Tile>();
            print(hit.collider.name);
            if (tile != null)
            {
                print("HIT");
                tile.SetTarget();
            }
        }
    }

    void MoveCameraTo(Vector3 target)
    {
        Transform cameraTrans = transform;
        Vector3 cameraPos = cameraTrans.position;
        cameraTrans.position = Vector3.Lerp(cameraPos, target + new Vector3(0,1,0), (speed+1.75f) * Time.deltaTime);
    }



}
