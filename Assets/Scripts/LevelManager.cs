using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public BoxCollider2D levelTransition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // You can check if the "other" collider belongs to the player (or something else)
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger!");
            SceneManager.LoadScene("Level1");
        }
    }
}
