using UnityEngine;

public class Hazards : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            Debug.Log("PLAYER IN HAZARD");
            playerMovement.KillSelf();
        }
    }
}
