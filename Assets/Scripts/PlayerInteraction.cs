using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] KeyCode interactKey;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Interactive interactive = collision.GetComponent<Interactive>(); ;
        if (interactive != null)
        {
            if (Input.GetKeyDown(interactKey))
            {
                interactive.Interaction();
            }
        }
    }
}
