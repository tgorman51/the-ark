using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    public GameObject menuObject;

    private CursorLockMode savedCursorLockMode;

    private InputAction _escAction;

    void OnEnable()
    {
        _escAction = InputSystem.actions.FindAction("Esc");
        _escAction.performed += HandleEsc;
    }
    
    void Start()
    {
        menuObject.SetActive(false);
        savedCursorLockMode = Cursor.lockState;
    }

    void OnDestroy()
    {
        _escAction.performed -= HandleEsc;
    }

    private void HandleEsc(InputAction.CallbackContext context)
    {
        if (!menuObject.activeSelf)
        {
            menuObject.SetActive(true);
            savedCursorLockMode = Cursor.lockState;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            CloseMenu();
        }
    }

    private void CloseMenu()
    {
        menuObject.SetActive(false);
        Cursor.lockState = savedCursorLockMode;
    }

    public void CancelQuit()
    {
        CloseMenu();
    }
    
    public void ConfirmQuit()
    {
        Application.Quit();
    }
}
