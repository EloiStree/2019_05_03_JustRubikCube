using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class RubikMono_FakeMouseDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Camera m_mainCamera;
    public InputAction m_clickAction;
    public InputAction m_clickPosition;
    public bool m_passthrough; // Toggle to pass through all objects
    public bool m_useOnDown = true; // Toggle for OnMouseDown event
    public bool m_useOnUp = true; // Toggle for OnMouseUp event

    void OnEnable()
    {
        m_clickAction.Enable();
        m_clickPosition.Enable();
        m_clickAction.performed += OnClick;
        m_clickAction.canceled += OnClick;
    }

    void OnDisable()
    {
        m_clickAction.performed -= OnClick;
        m_clickAction.canceled -= OnClick;
        m_clickAction.Disable();
        m_clickPosition.Disable();
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        if (m_mainCamera == null)
            m_mainCamera = Camera.main;

        Vector2 screenPosition = m_clickPosition.ReadValue<Vector2>();
        Ray ray = m_mainCamera.ScreenPointToRay(screenPosition);

        if (m_passthrough)
        {
            // Raycast through all objects
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                ProcessHit(hit, ctx);
            }
        }
        else
        {
            // Single raycast
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ProcessHit(hit, ctx);
            }
        }
    }

    private void ProcessHit(RaycastHit hit, InputAction.CallbackContext ctx)
    {
        Debug.Log("Clicked on: " + hit.collider.name);

        // Check if the action is performed (mouse down) or canceled (mouse up)
        if (ctx.performed && m_useOnDown)
        {
            hit.collider.gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
            ExecuteEvents.Execute<IPointerDownHandler>(hit.collider.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
        }
        else if (ctx.canceled && m_useOnUp)
        {
            hit.collider.gameObject.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
            ExecuteEvents.Execute<IPointerUpHandler>(hit.collider.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        }
    }

    // Implement IPointerDownHandler
    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_useOnDown)
        {
            gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
        }
    }

    // Implement IPointerUpHandler
    public void OnPointerUp(PointerEventData eventData)
    {
        if (m_useOnUp)
        {
            gameObject.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
        }
    }
}