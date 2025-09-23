using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        thrust.Enable();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(thrust.WasPressedThisFrame())
        {
            Debug.Log("Thrusting!");
        }
    }
}
