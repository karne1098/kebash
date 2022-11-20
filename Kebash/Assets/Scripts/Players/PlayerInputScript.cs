using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputData {
  public Vector2 Move   { get; internal set; }
  public Vector2 Turn   { get; internal set; }
  public bool    Charge { get; internal set; }
  public bool    Shoot  { get; internal set; }
}

public class PlayerInputScript : MonoBehaviour
{
  public PlayerInputData InputData { get; private set; } = new PlayerInputData();

  public void OnMove(InputAction.CallbackContext context)
  {
    InputData.Move = context.ReadValue<Vector2>();
  }

  public void OnTurn(InputAction.CallbackContext context)
  {
    InputData.Turn = context.ReadValue<Vector2>();
  }

	public void onCharge(InputAction.CallbackContext context)
  {
    if (context.started)       { InputData.Charge = true; }
    else if (context.canceled) { InputData.Charge = false; }
  }

	public void onShoot(InputAction.CallbackContext context)
  {
    if (context.started)       { InputData.Shoot = true; }
    else if (context.canceled) { InputData.Shoot = false; }
  }

  public void onPause(InputAction.CallbackContext context)
  {
    if (context.started) { GameStateManager.Instance.TogglePause(); }
  }
}
