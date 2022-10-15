using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
  [SerializeField] private GameObject _damagerObject;

  private int _playerNumber;
  private PlayerInputData _inputData;
  private Rigidbody       _rigidbody;

  // Joining
  [SerializeField] private GameObject _playerJoin;

  // Health
  private bool  _isInvulnerable = false;
  private float _hitInvDuration = 1f;
  private IEnumerator _currentInvInstance;
  private int   _debugCount = 0;

  // Food Stuff
  private int _maxFood = 3;
  private float _foodBulletSpeed = 20;
  [SerializeField] private Transform  _sliceSpawn;
  [SerializeField] private GameObject _foodSlicePrefab;
  [SerializeField] private List<GameObject> _foodSlices;
  [SerializeField] private GameObject _foodBulletPrefab;

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
  private bool  _isCharging  = false;
  private bool  _canRegen    = true;
  private float _chargeSpeed = 12f;

  // Shooting
  private float _shootCoolDown = 1f;
  private bool  _isOnShootCoolDown = false;

  // Falling
  private bool    _isNotOnGround = false;
  private float   _fallRespawnWaitDuration = 3;                 // Delay after falling before teleporting back to top
  // private Vector3 _fallRespawnPosition = new Vector3(0, 20, 0);
  // This will affect how much time you should be invulnerable
  private float   _fallInvDuration = 2;                         // How long the player is invulnerable after teleporting back up

  // TODO Delete me
  
  // ================== Accessors

  public float StaminaFraction { get { return _stamina / _maxStamina; } }

  public bool IsCharging {get {return _isCharging;}}

  public Stack<string> KebabStack { get; private set; } = new Stack<string>();

  // ================== Methods

  void Start()
  {
    _playerNumber = PlayerJoin.Instance.CurrentPlayerCount;
    _damagerObject.SetActive(false);

    _inputData = GetComponent<PlayerInputScript>().InputData;
    _rigidbody = GetComponent<Rigidbody>();
    transform.position =
      PlayerJoin.Instance.GetPlayerPosition();

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
    // Prevent player control in certain situations
    updateIsNotOnGround();
    if (_isCharging || _isNotOnGround) return;

    // Attempt to shoot
    if (_inputData.Shoot && !_isOnShootCoolDown)
    {
      Debug.Log("Attempting to shoot.");
      StopCoroutine("shootFood");
      StartCoroutine("shootFood");
      return;
    }

    // Attempt to charge
    if (_inputData.Charge && _stamina > _minStaminaToStart)
    {
      Debug.Log("Attempting to charge.");
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
      StartCoroutine(waitToTeleport(PlayerJoin.Instance.GetPlayerPosition(), true));
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

  public bool AddFood()
  {
    // Can't add food if full
    if (KebabStack.Count == _maxFood) return false;

    Debug.Log("Player " + _playerNumber + " added food."); 

    _foodSlices[KebabStack.Count].SetActive(true);
    KebabStack.Push("GenericFood");

    return true;
  }

  // ================== Helpers
  
  private void updateIsNotOnGround()
  {
    _isNotOnGround = _rigidbody.position.y < -0.5 || _rigidbody.position.y > 1;
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

  private IEnumerator shootFood()
  {
    // Can't shoot if no food
    if (KebabStack.Count == 0) yield break;

    Debug.Log("Player " + _playerNumber + " fired a shot!");
    _isOnShootCoolDown = true;

    // Remove from stack
    KebabStack.Pop(); //popping first does the "- 1" for us
    _foodSlices[KebabStack.Count].SetActive(false);

    // Spawn foodBullet and give it velocity
    Transform tip = _foodSlices[_maxFood - 1].transform; // TODO: spawn at a better place
    GameObject foodBullet = Instantiate(_foodBulletPrefab, tip.position, tip.rotation); // TODO: maybe object pool
    foodBullet.GetComponent<Rigidbody>().velocity = tip.forward * _foodBulletSpeed;  // TODO: this whole thing should be handled by the bullet

    // Wait for cooldown
    yield return new WaitForSeconds(_shootCoolDown);
    _isOnShootCoolDown = false;
  }

  private void startInvulnerability(float time)
  {
    if (_currentInvInstance != null) StopCoroutine(_currentInvInstance);
    _currentInvInstance = invulnerable(time);
    StartCoroutine(_currentInvInstance);
  }

  private IEnumerator invulnerable(float time)
  {
    _isInvulnerable = true;
    Debug.Log("Player " + _playerNumber + " is invulnerable!");

    yield return new WaitForSeconds(time);

    _isInvulnerable = false;
    Debug.Log("Player " + _playerNumber + " is no longer invulnerable!");
  }

  private IEnumerator waitToTeleport(Vector3 respawnPosition, bool takesDamage)
  {
    yield return new WaitForSeconds(_fallRespawnWaitDuration);

    // Start falling from the sky
    _rigidbody.position = respawnPosition; 
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
    foreach (string s in KebabStack)
    {
      Debug.Log(s);
    }
    Debug.Log("==================");
  }   
}