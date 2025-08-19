using UnityEngine;
using UnityEngine.InputSystem;

public class ClickHandler : MonoBehaviour
{
    public Camera mainCamera;
    public InputAction clickAction;

    void OnEnable()
    {
        clickAction.Enable();
        clickAction.performed += OnClick;
    }

    void OnDisable()
    {
        clickAction.performed -= OnClick;
        clickAction.Disable();
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        if(mainCamera ==null)
            mainCamera = Camera.main;

        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Clicked on: " + hit.collider.name);
            hit.collider.gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
            hit.collider.gameObject.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
        }
    }
}
