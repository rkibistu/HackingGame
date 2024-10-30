using StarterAssets;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(PlayerInput))]
#endif
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _interactionDistance = 1000.0f;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask _interactiveLayer;
    [SerializeField]
    private UIController _uiController;

#if ENABLE_INPUT_SYSTEM
    private PlayerInput _playerInput;
#endif
    private StarterAssetsInputs _input;

    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
#else
		Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
    }

    void Update()
    {
        CheckForInteractable();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        if (_input.interact)
        {
            _input.interact = false;
        }
    }
    private void CheckForInteractable()
    {
        // Create a ray from the camera
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, _interactionDistance, _interactiveLayer))
        {
            // Check if the object hit has the Interactible layer
            if (hit.collider != null)
            {
                // Show UI hints
                InteractiveElement element = hit.transform.GetComponent<InteractiveElement>();
                _uiController.ShowHint(element.HintDescription);

                if (_input.interact)
                {
                    element.DoSomething();
                    _input.interact = false;
                }
            }
        }
        else
        {
            _uiController.HideHint();
        }
    }
}
