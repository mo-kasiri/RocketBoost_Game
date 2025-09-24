using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    break;
                //case "Fuel":
                //    break;
                case "Finish":
                    Debug.Log("Congrats, you finished the game!");
                    break;
                default:
                    Destroy(gameObject);
                    SceneManager.LoadScene("SandBox");
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuel"))
        {
            Debug.Log("Picked up fuel!");
            Destroy(other.gameObject);
        }
    }
}
