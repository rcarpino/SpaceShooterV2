using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private float _thrusterBoost = 1.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.25f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private int _shieldStrength = 0;
    [SerializeField]
    private bool _isTripleShotActive = false;
    
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _thrusterVisual;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _shieldVisualizer1;
    [SerializeField]
    private GameObject _shieldVisualizer2;

    [SerializeField]
    private GameObject _rightEngineDamageVisualizer, _leftEngineDamageVisualizer;

    [SerializeField]
    private AudioClip _laserSoundClip;
    
    private AudioSource _audioSource;
   
    [SerializeField]
    private int _score;
    [SerializeField]
    private int _currentAmmo = -1;
    private int _maxAmmo = 15;
    

    private UIManager _uiManager;

    
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponentInChildren<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        _currentAmmo = _maxAmmo;

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source on Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        UpdateAmmoCount();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _currentAmmo > 0)
        {
            FireLaser();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            EngageThrusters();
            
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            DisengageThrusters();
           
        }
        

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed  * Time.deltaTime);
        
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0),0);

        if (transform.position.x > 11.4f)
        {
            transform.position = new Vector3(-11.4f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.4f)
        {
            transform.position = new Vector3(11.4f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
            _currentAmmo--;
        }
        _audioSource.Play();
        //play the laser audio clip
        
    }

    public void Damage()
    {
        if(_shieldStrength >= 1)
        {
            _shieldStrength--;
        }
        else
        {
            _lives--;
        }

        switch (_shieldStrength)
        {
            case 2:
                _shieldVisualizer.SetActive(false);
                _shieldVisualizer1.SetActive(true);
                break;
            case 1:
                _shieldVisualizer1.SetActive(false);
                _shieldVisualizer2.SetActive(true);
                break; ;
            case 0:
                _shieldVisualizer2.SetActive(false);
                _shieldStrength--;
                break; 
            default:
                _isShieldActive = false;
                break;
        }
        

        
        if(_lives == 2 && _isShieldActive == false)
        {
            _rightEngineDamageVisualizer.SetActive(true);
        }
        else if(_lives == 1 && _isShieldActive == false)
        {
            _leftEngineDamageVisualizer.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);
        
        if(_lives < 1)
        {
            _spawnManager.onPlayerDeath();
            Destroy(this.gameObject);
        }
    }
  

    public void UpdateAmmoCount()
    {
        _uiManager.UpdateAmmoCount(_currentAmmo, _maxAmmo);
        
    }

    public void HealthPowerup()
    {
        _lives++;
        _uiManager.UpdateLives(_lives);
        
        if(_lives == 3)
        {
            _rightEngineDamageVisualizer.SetActive(false);
        }
        else if ( _lives == 2)
        {
            _leftEngineDamageVisualizer.SetActive(false);
        }
    }
    

    public void AmmoPowerup()
    {
        _currentAmmo = _maxAmmo;
    }

    public void AddtoScore(int points)
    {
        //add 10 to the score.
        _score += points;
        _uiManager.UpdateScore(_score);
        //communicate with UI to update the score.
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostActive()
    {
       
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ShieldActive()
    {
        if(_isShieldActive == false)
        {
            _isShieldActive = true;
            _shieldStrength = 3;
            _shieldVisualizer.SetActive(true);
        }
       
        
    }

    public void EngageThrusters()
    {   
        _speed *= _thrusterBoost;
        _thrusterVisual.gameObject.SetActive(true);
    }

    private void DisengageThrusters()
    {
        _speed /= _thrusterBoost;
        _thrusterVisual.gameObject.SetActive(false);
    }

    
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    } 

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }
}
