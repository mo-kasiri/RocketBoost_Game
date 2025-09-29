using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;

    [Header("Movement Settings")]
    [SerializeField] private float forceValue = 100f;
    [SerializeField] private float rotationStrength = 20f;

    [Header("Audio")]
    [SerializeField] private AudioClip mainEngineClip;
    private AudioSource engineSFXSource;

    [Header("Particles")]
    [SerializeField] private ParticleSystem mainEngineParticle;
    [SerializeField] private ParticleSystem rightEngineParticle;
    [SerializeField] private ParticleSystem leftEngineParticle;

    private Rigidbody rb;

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        engineSFXSource = GetComponent<AudioSource>();

        mainEngineParticle?.Stop();
        rightEngineParticle?.Stop();
        leftEngineParticle?.Stop();
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
            rb.AddRelativeForce(Vector3.up * forceValue * Time.fixedDeltaTime, ForceMode.Impulse);

            if (!engineSFXSource.isPlaying)
            {
                engineSFXSource.PlayOneShot(mainEngineClip);
                AudioFader.Instance.Fade(engineSFXSource, 1f, Time.deltaTime);
            }

            mainEngineParticle?.Play();
        }
        else
        {
            if (engineSFXSource.isPlaying)
            {
                AudioFader.Instance.Fade(engineSFXSource, 0f, 0.3f);
                engineSFXSource.Stop();
            }

            mainEngineParticle?.Stop();
        }
    }

    private void ProcessRotation()
    {
        rb.freezeRotation = true;

        float rotationInput = rotation.ReadValue<float>() * Mathf.PI * Time.fixedDeltaTime * rotationStrength;
        transform.Rotate(Vector3.forward, rotationInput);

        if (rotationInput > 0f)
        {
            rightEngineParticle?.Play();
            leftEngineParticle?.Stop();
        }
        else if (rotationInput < 0f)
        {
            leftEngineParticle?.Play();
            rightEngineParticle?.Stop();
        }
        else
        {
            rightEngineParticle?.Stop();
            leftEngineParticle?.Stop();
        }

        rb.freezeRotation = false;
    }

    public void DisableMovement()
    {
        engineSFXSource.Stop();
        mainEngineParticle?.Stop();
        leftEngineParticle?.Stop();
        rightEngineParticle?.Stop();

        enabled = false;
    }
}
