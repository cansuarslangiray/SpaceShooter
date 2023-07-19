using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 3.5f;
    private float _speedMutiplier = 2;

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject tripleLaserPrefab;
    [SerializeField] private GameObject _ShieldVisualizer;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _lives = 3;

    private float _canFire = -1f;
    private SpawnManger _spawnManger;

    private bool _isTripleShootActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    [SerializeField] private int _score = 0;

    private IU_Manager _ıuManager;
    [SerializeField]
    private GameObject _rightEngine,_leftEngine;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // take the current position = new position(0,0,0);
        transform.position = new Vector3(0, 0, 0);
        _ıuManager = GameObject.Find("Canvas").GetComponent<IU_Manager>();
        _spawnManger = GameObject.Find("Spawn_Manager").GetComponent<SpawnManger>();
        _audioSource = GetComponent<AudioSource>();

        if (_ıuManager == null)
        {
            Debug.LogError("The UI Manager is null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("audioSource on the player is null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalcuateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalcuateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");


        Vector3 direction = new Vector3(horizontal, _vertical, 0);
        transform.Translate(direction * speed * Time.deltaTime);


        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0f), 0f);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0f);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0f);
        }
    }

    void FireLaser()
    {
        if (_isTripleShootActive == true)
        {
            Instantiate(tripleLaserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        else
        {
            _canFire = Time.time + _fireRate;
            Instantiate(laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _ShieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives==1)
        {
            _rightEngine.SetActive(true);
        }
        _ıuManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            _spawnManger.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShootActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShootActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        speed *= _speedMutiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        speed /= _speedMutiplier;
    }

    public void ShieldActive()
    {
        _ShieldVisualizer.SetActive(true);
        _isShieldActive = true;
    }

    public void AddScore(int point)
    {
        _score += point;
        _ıuManager.Score(_score);
    }
}