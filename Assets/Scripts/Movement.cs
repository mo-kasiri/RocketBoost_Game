using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] private float _forceValue = 100.0f;
    [SerializeField] private float _rotationStrength = 20.0f;


    private AudioSource _audiosource;
    private Rigidbody _rb;

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            if (_rb != null)
            {
                _rb.AddRelativeForce(Vector3.up * _forceValue * Time.fixedDeltaTime, ForceMode.Impulse);

                if (_audiosource != null) 
                {
                    if(!_audiosource.isPlaying)
                    {
                        _audiosource.Play();
                        AudioFader.Instance.Fade(_audiosource, 1f, Time.deltaTime/2);
                    }
                }
            }
        }
        else if(_audiosource != null && _audiosource.isPlaying) 
        {
            AudioFader.Instance.Fade(_audiosource, 0f, 0.3f);
            //_audiosource.Stop();
        }
    }
    

    private void ProcessRotation()
    {
        _rb.freezeRotation = true;
        float rotationInputValue = rotation.ReadValue<float>() * Mathf.PI * Time.fixedDeltaTime * _rotationStrength;
        // Debug.Log(rotationInputValue);
        this.transform.Rotate(Vector3.forward, rotationInputValue);
        _rb.freezeRotation = false;
    }
}
