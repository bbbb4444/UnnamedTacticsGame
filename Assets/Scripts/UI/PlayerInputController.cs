using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class PlayerInputController : MonoBehaviour
{
    private EventSystem eventSystem;
    private InputSystemUIInputModule inputModule;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        eventSystem = EventSystem.current;
        inputModule = eventSystem.GetComponent<InputSystemUIInputModule>();
    }

    public void DisableInput()
    {
        if (inputModule)
        {
            print("disabling inputsystem");
            inputModule.DeactivateModule();
        }

        if (playerInput)
        {
            print("disabling playerinput");
            playerInput.enabled = false;
        }
    }
    public void EnableInput()
    {
        if (inputModule)
        {
            print("enabling inputsystem");
            inputModule.ActivateModule();
        }

        if (playerInput)
        {
            print("enabling playerinput");
            playerInput.enabled = true;
        }
    }
    
}