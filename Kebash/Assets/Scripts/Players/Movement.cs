using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
  [SerializeField] private int _playerNumber = 1;
  [SerializeField] private GameObject _damagerObject;
  
  private PlayerInputData _inputData;
  private Rigidbody _rigidbody;

  // Health
  private Stack<string> _stack = new Stack<string>();
  private bool  _isInvulnerable = false;
  private float _hitInvDuration = 1f;
  private IEnumerator _currentInvInstance;
  private int   _debugCount = 0;

  // Moving and turning
  private Vector3 _idealMove;
  private Vector3 _idealTurn;
  private Vector3 _currentTurn;
  private float _moveSpeed = 4f;
  private float _moveLerpRate = 0.2f;
  private float _turnSlerpRate = 0.08f;
  private float _strafeSpeedMultiplier = 1f;

  // Charging
  private float _stamina;
  private float _minChargeDuration;
  private float _maxStamina        = 100f;
  private float _minChargeCost     = 10f;  // Minimum stamina cost associated with one charge
  private float _minStaminaToStart = 20f;  // Minimum stamina needed to start a charge
  private float _staminaCostRate   = 100f;
  private float _staminaRegenRate  = 20f;
  private float _regenDelay        = 0.2f; // Delay after charging ends before regen begins
  private bool  _isCharging = false;
  private bool  _canRegen   = true;
  private float _chargeSpeed = 12f;

  // Falling
  private bool    _isNotOnGround = false;
  private float   _fallRespawnWaitDuration = 3;                 // Delay after falling before teleporting back to top
  private Vector3 _fallRespawnPosition = new Vector3(0, 20, 0); // This will affect how much time you should be invulnerable
  private float   _fallInvDuration = 2;                         // How long the player is invulnerable after teleporting back up

  // ================== Accessors

  public float StaminaFraction { get { return _stamina / _maxStamina; } }

  public bool IsCharging {get {return _isCharging;}}

  public Stack<string> setKabobStack { get { return _stack; } }
  public Stack<string> getKabobStack { set { _stack = value; } }

  // ================== Methods

  void Start()
  {
    _damagerObject.SetActive(false);

    _inputData = InputManager.Instance.P[_playerNumber];
    _rigidbody = transform.GetComponent<Rigidbody>();

    _stack.Push("A");
    _stack.Push("B");
    _stack.Push("C");

    _idealMove = Vector3.zero;
    Vector3 initialTurn = Vector3.forward; // Todo: define an initial value facing the centroid of players
    _idealTurn   = initialTurn;
    _currentTurn = initialTurn;
    _rigidbody.rotation = Quaternion.LookRotation(initialTurn);

    _stamina = _maxStamina;
    _minChargeDuration = _minChargeCost / _staminaCostRate;
  }

  void FixedUpdate()
  {
    if (_isCharging) return;

    groundedCheck();
    if (_isNotOnGround) return;

    if (_inputData.Charge && _stamina > _minStaminaToStart)
    {
      StopCoroutine("charge");
      StartCoroutine("charge");
      return;
    }

    move();
    turn();
    regenStamina();
  }

  void OnTriggerEnter(Collider other)
  {
    // Fell through fall trigger collider
    if (other.gameObject.layer == Utils.FallTriggerLayer)
    {
      Debug.Log("Player " + _playerNumber + " has fallen!");
      StartCoroutine("waitToTeleport");
      return;
    }

    // Got hit by other player's damager collider
    if (other.gameObject.layer == Utils.DamagerLayer && !_isInvulnerable && !_isNotOnGround)
    {
      Debug.Log("Player " + _playerNumber + " hit (debug count: " + _debugCount++ + ")!");

      startInvulnerability(_hitInvDuration);
      return;
    }
  }

  // ================== Helpers
  
  private void groundedCheck()
  {
    _isNotOnGround = (_rigidbody.position.y < -0.5 || _rigidbody.position.y > 1);
  }

  private void move()
  {
    _idealMove = Utils.V2ToV3(_inputData.Move);

    Vector3 newVelocity = Vector3.Lerp(
      _rigidbody.velocity,
      getAngleDependentSpeed() * _idealMove,
      _moveLerpRate);

    newVelocity.y = _rigidbody.velocity.y; // Avoid changing rigidbody's Y velocity
    
    _rigidbody.velocity = newVelocity;
  }

  private void turn()
  {
    if (_inputData.Turn.magnitude > 0)
    {
      _idealTurn = Utils.V2ToV3(_inputData.Turn);
    }
    else if (_idealMove.magnitude > 0) 
    {
      _idealTurn = _idealMove;
    }
    else
    {
      return;
    }
    
    _currentTurn = Vector3.Slerp(
      _currentTurn, 
      _idealTurn, 
      _turnSlerpRate);

    _rigidbody.rotation = Quaternion.LookRotation(_currentTurn);
  }

  private void regenStamina()
  {
    if (_canRegen && _stamina < _maxStamina)
    {
      _stamina += Time.deltaTime * _staminaRegenRate;
      _stamina = Mathf.Min(_stamina, _maxStamina);
    }
  }

  private float getAngleDependentSpeed()
  {
    float speedMultiplier = Mathf.Lerp(
      Mathf.Pow(Vector3.Dot(_idealMove, _currentTurn), 2),
      1,
      _strafeSpeedMultiplier);

    return speedMultiplier * _moveSpeed;
  }
  
  private IEnumerator charge()
  {
    // Start charge
    _isCharging = true;
    gameObject.layer = Utils.ChargerLayer;
    _damagerObject.SetActive(true);
    _canRegen = false;
    _rigidbody.velocity = _currentTurn * _chargeSpeed;

    // Enforce minimum
    _stamina -= _minChargeCost;
    yield return new WaitForSeconds(_minChargeDuration);

    // Continue charging
    while (_stamina > 0 && _inputData.Charge)
    {
      yield return null;
      _stamina -= Time.deltaTime * _staminaCostRate;
    }

    // Stop charge
    _isCharging = false;
    gameObject.layer = Utils.PlayerLayer;
    _damagerObject.SetActive(false);

    // Allow regen after some time
    yield return new WaitForSeconds(_regenDelay);
    _canRegen = true;
  }
  
  private IEnumerator addFood()
  {
    Debug.Log("Player " + _playerNumber + " added food.");
    yield return new WaitForSeconds(0.01f); // idk what else to run lol
  }

<<<<<<< HEAD
  private IEnumerator addFood(){
    Debug.Log("Add meat.");
    yield return new WaitForSeconds(0.01f); //dk what else to run lol
  }

  private IEnumerator invulnerable()
=======
  private void startInvulnerability(float time)
  {
    if (_currentInvInstance != null) StopCoroutine(_currentInvInstance);
    _currentInvInstance = invulnerable(time);
    StartCoroutine(_currentInvInstance);
  }

  private IEnumerator invulnerable(float time)
>>>>>>> fc3c8b4377a462391dcba4442f41c8c4e5489be6
  {
    _isInvulnerable = true;
    Debug.Log("Player " + _playerNumber + " is invulnerable!");

    yield return new WaitForSeconds(time);

    _isInvulnerable = false;
    Debug.Log("Player " + _playerNumber + " is no longer invulnerable!");
  }

  private IEnumerator waitToTeleport()
  {
    yield return new WaitForSeconds(_fallRespawnWaitDuration);

    // Start falling from the sky
    _rigidbody.position = _fallRespawnPosition; 
    _rigidbody.velocity = Vector3.zero;

    // Take one unit of damage
    // Todo

    // Restore stamina
    _stamina = _maxStamina;

    // Start invulnerability
    startInvulnerability(_fallInvDuration);
  }

  private void printStack()
  {
    foreach (string s in _stack)
    {
      Debug.Log(s);
    }
    Debug.Log("==================");
  }
    
}
