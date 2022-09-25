using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputData {
  public Vector2 Move   { get; internal set; }
  public Vector2 Turn   { get; internal set; }
  public bool    Charge { get; internal set; }
}

[DisallowMultipleComponent]
public class InputManager : MonoBehaviour
{
  public static InputManager Instance;

  // ================== Accessors
  
  public Dictionary<int, PlayerInputData> P { get; } = new Dictionary<int, PlayerInputData>();

  // ================== Methods

  void Awake()
  { 
    Instance = this;

    P.Add(1, new PlayerInputData());
    P.Add(2, new PlayerInputData());
  }

  // Player 1

  public void OnP1Move(InputAction.CallbackContext context)
  {
    P[1].Move = context.ReadValue<Vector2>();
  }

  public void OnP1Turn(InputAction.CallbackContext context)
  {
    P[1].Turn = context.ReadValue<Vector2>();
  }

	public void onP1Charge(InputAction.CallbackContext context)
  {
    if (context.started)       { P[1].Charge = true; }
    else if (context.canceled) { P[1].Charge = false; }
  }

  // Player 2

  // public void OnP2Move(InputAction.CallbackContext context)
  // {
  //   P[2].Move = context.ReadValue<Vector2>();
  // }

  // public void OnP2Turn(InputAction.CallbackContext context)
  // {
  //   P[2].Turn = context.ReadValue<Vector2>();
  // }

	// public void onP2Charge(InputAction.CallbackContext context)
  // {
  //   if (context.started)       { P[2].Charge = true; }
  //   else if (context.canceled) { P[2].Charge = false; }
  // }

  // // Press to toggle pause
	// public void OnPauseInput(InputAction.CallbackContext context)
  // {
  //   if (context.started) { PauseManager.Instance?.TogglePauseMenu(); }
  // }
}