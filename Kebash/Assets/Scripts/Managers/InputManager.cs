using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class InputManager : MonoBehaviour
{
  public static InputManager Instance;

	[HideInInspector] public Vector2 Movement { get; private set; }
	[HideInInspector] public bool    Charge   { get; private set; } = false;

  void Awake() { Instance = this; }

  // P1 movement
  public void OnMovementInput(InputAction.CallbackContext context)
  {
    Movement = context.ReadValue<Vector2>();
  }

  // Press/Hold to trigger P1 charge
	public void onChargeInput(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      Charge = true;
    }
    else if (context.canceled)
    {
      Charge = false;
    }
  }

  // // Press to toggle pause
	// public void OnPauseInput(InputAction.CallbackContext context)
  // {
  //   if (context.started)
  //   { 
  //     PauseManager.Instance?.TogglePauseMenu(); // Call the PauseManager
  //   }
  // }
}