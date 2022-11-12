using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
  private PlayerInputData _inputData;
  
  [SerializeField] private GameObject _damagerObject;
  [SerializeField] private Rigidbody  _rigidbody;
  [SerializeField] private Animator   _animator;
  [SerializeField] private StaminaBar _staminaBar;
  [SerializeField] private ParticleSystem _stabParticles;
  [SerializeField] private ParticleSystem _dashParticles;
  [SerializeField] private ParticleSystem _walkParticles;

  // Health
  private bool  _isInvulnerable = false;
  private float _hitInvDuration = 1f;
  private IEnumerator _currentInvInstance;
  private int _debugCount = 0;
  private int _timesDied = 0;

  // Food Stuff
  private int _maxFood = 3;
  private float _foodBulletSpeed = 20;
  [SerializeField] private List<Transform> _foodSliceTransforms;
  [SerializeField] private GameObject _foodBulletPrefab;
  //All Food Prefabs
  [SerializeField] private GameObject _lambPrefab;
  [SerializeField] private GameObject _onionPrefab;
  [SerializeField] private GameObject _tomatoPrefab;
  [SerializeField] private GameObject _mushroomPrefab;
  [SerializeField] private GameObject _pepperPrefab;
  [SerializeField] private GameObject _eggplantPrefab;

  // Moving and turning
  private Vector3 _idealMove   = Vector3.zero;
  private Vector3 _idealTurn   = Vector3.zero;
  private Vector3 _currentMove = Vector3.zero;
  private Vector3 _currentTurn = Vector3.zero;
  private float _moveSpeed             = 5f;
  private float _moveLerpRate          = 0.2f;
  private float _turnSlerpRate         = 0.08f;
  private float _strafeSpeedMultiplier = 0.75f;
    private bool _moveCheck = false;

  // Charging
  private float _stamina;
  private float _minChargeDuration;
  private float _maxStamina        = 100f;
  private float _minChargeCost     = 10f;  // Minimum stamina cost associated with one charge
  private float _minStaminaToStart = 20f;  // Minimum stamina needed to start a charge
  private float _staminaCostRate   = 100f;
  private float _staminaRegenRate  = 20f;
  private float _regenDelay        = 0.2f; // Delay after charging ends before regen begins
  private bool  _canRegen    = true;
  private float _chargeSpeed = 12f;

  // Shooting
  private float _shootCoolDown = 1f;
  private bool  _isOnShootCoolDown = false;

  // Falling
  private float _fallRespawnWaitDuration = 3; // Delay after falling before teleporting back to top
  private float _fallInvDuration = 2;         // How long the player is invulnerable after teleporting back up
  


  // ================== Accessors

  public float StaminaFraction { get { return _stamina / _maxStamina; } }

  public int  PlayerNumber { get; set; }         = -1;
  public Vector3  RespawnPosition { get; set; }         = new Vector3(0, 100, 0);
  public bool IsCharging   { get; private set; } = false;
  public bool IsOnGround   { get; private set; } = false;
  public Stack<PooledObjectIndex> KebabStack   { get; private set; } = new Stack<PooledObjectIndex>();

  // ================== Methods

  void Start()
  {
    // Player number, position, rotation handled by MultiplayerManager on spawn
    _rigidbody.rotation = Quaternion.LookRotation(Vector3.forward); // FIXME: this should be handled elsewhere

    // Basic player data
    _inputData = GetComponent<PlayerInputScript>().InputData;

    // Stuff to do with child game objects
    _damagerObject.SetActive(false);

    // Initialize some private stuff
    _stamina = _maxStamina;
    _minChargeDuration = _minChargeCost / _staminaCostRate;
    _currentMove = _rigidbody.velocity;
    _currentTurn = _rigidbody.rotation * Vector3.forward;
  }

  void FixedUpdate()
  {
    // Prevent player control in certain situations
    updateIsOnGround();
    if (IsCharging || !IsOnGround) return;

    // Attempt to shoot
    if (_inputData.Shoot && !_isOnShootCoolDown)
    {
      StopCoroutine("shootFood");
      StartCoroutine("shootFood");
      return;
    }

    // Attempt to charge
    if (_inputData.Charge && _stamina > _minStaminaToStart)
    {
      StopCoroutine("charge");
      StartCoroutine("charge");
      return;
    }
    
    if (_inputData.Charge && _stamina <= _minStaminaToStart)
    {
      _staminaBar.shake();
      Debug.Log("Not enough stamina to charge");
    }

    move();
    turn();
    regenStamina();

    // Fixme
    float forwardFloat = Vector3.Dot(_currentMove, _currentTurn);
    _animator.SetFloat("InputY", forwardFloat);
    float rightFloat   = Vector3.Dot(_currentMove, Quaternion.AngleAxis(-90, Vector3.up) * _currentTurn);
    _animator.SetFloat("InputX", rightFloat);
  }

  void OnTriggerEnter(Collider other)
  {
    // Fell through fall trigger collider
    if (other.gameObject.layer == Utils.FallTriggerLayer)
    {
      Debug.Log("Player " + PlayerNumber + " has fallen!");

      StartCoroutine(waitToTeleport(RespawnPosition, true));
      return;
    }

    // Got hit by other player's damager collider OR got hit by food
    if (other.gameObject.layer == Utils.DamagerLayer && !_isInvulnerable && IsOnGround)
    {

      if (other.gameObject.tag == "Player")
            {
                other.transform.parent.gameObject.GetComponent<Movement>().PlayStab();
            }
      Debug.Log("Player " + PlayerNumber + " hit (debug count: " + _debugCount++ + ")!");
      if(!(KebabStack.Count == 0)) {
        KebabStack.Pop(); // Popping first does the "-1" for KebabStack.Count
        // Disable all children
        for (int i = 0; i < _foodSliceTransforms[KebabStack.Count].transform.childCount; ++i) {
          Transform a = _foodSliceTransforms[KebabStack.Count].transform.GetChild(i);
          a.gameObject.SetActive(false);
        }
        Debug.Log("Player " + PlayerNumber + "Has been hit and has this much health: " + KebabStack.Count);
      }
      else{
        Debug.Log("Player " + PlayerNumber + "Has died this many times: " + _timesDied + 1);
        StartCoroutine(deathTeleport(RespawnPosition, true));
        _timesDied += 1;
      }
      startInvulnerability(_hitInvDuration);
      return;
    }
  }

  // Returns true if successful
  public bool AddFood(PooledObjectIndex index)
  {
    // Can't add food if full
    if (KebabStack.Count == _maxFood) return false;
    _stabParticles.Play();

    Debug.Log("Player " + PlayerNumber + " added food. (Pooled object index: " + (int) index + ")"); 

    // Activate the correct position's correct child
    Transform sliceGameObject = _foodSliceTransforms[KebabStack.Count].GetChild((int) index);
    sliceGameObject.gameObject.SetActive(true);

    // Add to stack
    KebabStack.Push(index);

    return true;
  }

  // ================== Helpers
  
  private void updateIsOnGround()
  {
    // FIXME: magic numbers
    IsOnGround =  -0.5 < _rigidbody.position.y && _rigidbody.position.y < 3;
  }

  private void move()
  {
    _idealMove = Utils.V2ToV3(_inputData.Move);

        if ((_moveCheck && (_idealMove == Vector3.zero)) || (_rigidbody.position.y != 0.525f))
        {
            _moveCheck = false;
            _walkParticles.Stop();
        }
        else if ((_moveCheck == false) && (_idealMove != Vector3.zero))
        {
            _moveCheck = true;
            _walkParticles.Play();
        }

        _currentMove = Vector3.Lerp(
      _rigidbody.velocity,
      getAngleDependentSpeed() * _idealMove,
      _moveLerpRate);

    _currentMove.y = _rigidbody.velocity.y; // Avoid changing rigidbody's Y velocity
    _rigidbody.velocity = _currentMove;


  }

  private void turn()
  {
    if (_inputData.Turn.magnitude > 0)
    {
      // Turn with specific input
      _idealTurn = Utils.V2ToV3(_inputData.Turn);
    }
    else if (_idealMove.magnitude > 0) 
    {
      // Turn with movement
      _idealTurn = _idealMove;
    }
    else
    {
      // No inputs
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
    IsCharging = true;
    gameObject.layer = Utils.ChargerLayer;
    _damagerObject.SetActive(true);
    _canRegen = false;
    _rigidbody.velocity = _currentTurn * _chargeSpeed;
    _dashParticles.Play();
    _animator.SetBool("Charging", true);

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
    IsCharging = false;
    gameObject.layer = Utils.PlayerLayer;
    _damagerObject.SetActive(false);
    _dashParticles.Stop();
    _animator.SetBool("Charging", false);

    // Allow regen after some time
    yield return new WaitForSeconds(_regenDelay);
    _canRegen = true;
  }

  private IEnumerator shootFood()
  {
    // Can't shoot if no food
    if (KebabStack.Count == 0) yield break;

    _isOnShootCoolDown = true;

    PooledObjectIndex num = KebabStack.Pop(); // Popping first does the "-1" for KebabStack.Count

    // Disable all children
    for (int i = 0; i < _foodSliceTransforms[KebabStack.Count].transform.childCount; ++i) {
      Transform a = _foodSliceTransforms[KebabStack.Count].transform.GetChild(i);
      a.gameObject.SetActive(false);
    }

    Transform tip = _foodSliceTransforms.Last();    //todo: change location
    GameObject bulletType = _foodBulletPrefab;
    int nnum = (int) num;
    switch (nnum)
    {
      case 1:{ bulletType = _lambPrefab; break;}
      case 2: { bulletType = _onionPrefab; break;}
      case 3: { bulletType = _tomatoPrefab; break;}
      case 4: { bulletType = _mushroomPrefab; break;}
      case 5: { bulletType = _eggplantPrefab; break;}
      case 6: { bulletType = _pepperPrefab; break;}
      default: { break;}
    }    
    GameObject foodBullet = Instantiate(bulletType, tip.position, tip.rotation); // (low priority) TODO: maybe object pool
        foodBullet.GetComponent<FoodShootBase>().StartShot(tip);
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
    Debug.Log("Player " + PlayerNumber + " has started invulnerability!");

    yield return new WaitForSeconds(time);

    _isInvulnerable = false;
    Debug.Log("Player " + PlayerNumber + " is no longer invulnerable!");
  }

  private IEnumerator waitToTeleport(Vector3 respawnPosition, bool takesDamage)
  {
    yield return new WaitForSeconds(_fallRespawnWaitDuration);
    // Start falling from the sky
    _rigidbody.position = respawnPosition; 
    _rigidbody.velocity = Vector3.zero;
    // Take one unit of damage
    if(!(KebabStack.Count == 0)) {
        KebabStack.Pop(); // Popping first does the "-1" for KebabStack.Count
        // Disable all children
        for (int i = 0; i < _foodSliceTransforms[KebabStack.Count].transform.childCount; ++i) {
          Transform a = _foodSliceTransforms[KebabStack.Count].transform.GetChild(i);
          a.gameObject.SetActive(false);
        }
        Debug.Log("Player " + PlayerNumber + "had one health removed by falling and now has: " + KebabStack.Count);
    }
    // Restore stamina
    _stamina = _maxStamina;

    // Start invulnerability
    startInvulnerability(_fallInvDuration);
  }

    private IEnumerator deathTeleport(Vector3 respawnPosition, bool takesDamage)
  {
    // Start falling from the sky
    for(int i = 0; i < 3; i++){
      _rigidbody.position = respawnPosition; 
      _rigidbody.velocity = Vector3.zero;
      yield return new WaitForSeconds(0.2f);
    }

    // Restore stamina
    _stamina = _maxStamina;

    // Start invulnerability
    startInvulnerability(_fallInvDuration);
  }

  public void PlayStab()
  {
    _stabParticles.Play();
  }
}