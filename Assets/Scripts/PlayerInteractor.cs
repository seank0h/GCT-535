using UnityEngine;
using UnityEngine.InputSystem; 
using Suburb;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Settings")]
    public float interactRadius = 2.0f;
    public LayerMask interactLayer; 

    private PlayerInput _playerInput;
    private InputAction _interactAction;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _interactAction = _playerInput.actions["Interact"];
    }


    private void OnInteract(InputValue value)
    {
         if (!value.isPressed) return;

        // Perform the radius check
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRadius, interactLayer);

        foreach (var col in colliders)
        {
            // Look for the door script in the hit object or its parents
            SimpleOpenClose door = col.GetComponentInParent<SimpleOpenClose>();

            if (door != null)
            {
                // Trigger the door logic
                door.SendMessage("ObjectClicked", SendMessageOptions.DontRequireReceiver);

                // Stop after the first door found
                break;
            }
        }
    }

    // Visualizes the radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}