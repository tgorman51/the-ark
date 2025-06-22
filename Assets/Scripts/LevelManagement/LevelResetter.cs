using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelResetter : MonoBehaviour
{
    void Start()
    {
        InputSystem.actions.FindAction("LevelReset").performed += HandleLevelReset;
    }

    private void HandleLevelReset(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
