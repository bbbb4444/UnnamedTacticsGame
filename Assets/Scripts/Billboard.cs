using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera MainCamera { get; set; }
    void Start()
    {
        MainCamera = Camera.main;
    }
    void LateUpdate()
    {
        if (!MainCamera) MainCamera = Camera.main;
        
        else transform.rotation = MainCamera.transform.rotation;
    }
}
