using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private Actions inputActions;

    [SerializeField] float interactDistance;

    private RaycastHit interactHit;

    private Inventory playerInventory;
    private void Awake()
    {
        inputActions = new();

        playerInventory = GetComponent<Inventory>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Interact.started += Interact;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();

        inputActions.Player.Interact.started -= Interact;
    }

    void Update()
    {
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out interactHit, interactDistance))
        {
            if(interactHit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.OnInteract();
            }
            else if(interactHit.collider.TryGetComponent<IItemInteractable>(out var itemInteractable))
            {
                itemInteractable.OnItemInteract(playerInventory);
            }
        }

    }
}
