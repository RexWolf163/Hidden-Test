using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SystemExitHandler : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.UI.Exit.performed += Exit;
    }

    private void OnDestroy()
    {
        inputActions.UI.Exit.performed -= Exit;
    }

    void OnEnable()
    {
        inputActions.Enable(); // Включаем Input Actions
    }

    void OnDisable()
    {
        inputActions.Disable(); // Выключаем Input Actions
    }

    private void Exit(InputAction.CallbackContext context)
    {
        Debug.Log("Exit");
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}