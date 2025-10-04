using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CollisionHandler : MonoBehaviour
{
    private int _sceneCount;
    private Movement _movement;
    private AudioSource _sfxSource;
    private Collider _collider;

    [Header("Cine Machine Camera")]
    [SerializeField] private CinemachineCamera vcam;


    [Header("Shatterd Rocket")]
    [SerializeField] private GameObject _shatteredRocket;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _explosionSFX;
    [SerializeField] private AudioClip _successSFX;
    [SerializeField] private AudioClip _fuelPickupSFX;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _successParticles;
    [SerializeField] private ParticleSystem _crashParticles;
    [SerializeField] private ParticleSystem _mainEngineParticles;

    private void Awake()
    {
        _sceneCount = SceneManager.sceneCountInBuildSettings;
    }

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _movement = GetComponent<Movement>();
        _sfxSource = GetComponents<AudioSource>()[1];
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // Do nothing
                break;

            case "Ending": 
                StartSuccessSequence();
                break;

            default:
                StartCrashSequence();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuel"))
        {
            Debug.Log("Picked up fuel!");
            if (_fuelPickupSFX != null)
            {
                PlayAudioClip(_fuelPickupSFX);
            }
            Destroy(other.gameObject);
        }
    }

    private void StartCrashSequence()
    {
        _movement.DisableMovement();

        if (_explosionSFX != null)
        {
           if (!_sfxSource.isPlaying && _crashParticles != null)
            {
                PlayAudioClip(_explosionSFX, ReloadLevel);
                Instantiate(_shatteredRocket, transform.position, transform.rotation);
                _crashParticles.Play();
                HidePlayer();
            }
        }
        else
        {
            ReloadLevel();
        }
    }

    private void StartSuccessSequence()
    {
        _movement.DisableMovement();

        if (_successSFX != null)
        {
            if(!_sfxSource.isPlaying && _successParticles != null)
            {
                PlayAudioClip(_successSFX, LoadNextLevel);
                _successParticles.Play();
            }
        }
        else
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene + 1 == _sceneCount)
        {
            Debug.Log("Congrats, you finished the game!");
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log(currentScene);
            SceneManager.LoadScene(currentScene + 1);
        }
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Plays an audio clip and optionally runs an action after it finishes.
    /// </summary>
    private void PlayAudioClip(AudioClip clip, System.Action onComplete = null)
    {
        if (_sfxSource == null || clip == null)
        {
            onComplete?.Invoke();
            return;
        }

        _sfxSource.Stop();
        _sfxSource.PlayOneShot(clip);

        if (onComplete != null)
        {
            Invoke(onComplete.Method.Name, clip.length);
        }
    }

    private void HidePlayer()
    {
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = false;
            // Unset camera follow/lookAt
        if (vcam != null)
        {
            vcam.Follow = null;
            vcam.LookAt = null;
        }
        }
    }

    void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.isPressed)
        {
            Debug.Log("Debug: Loading next level");
            LoadNextLevel();
        }
        if (Keyboard.current.cKey.isPressed)
        {
            // Toggle collision
            _collider.enabled = !_collider.enabled;
        }
    }
}
