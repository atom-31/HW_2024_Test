using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("DeathZone triggered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
    }

}
