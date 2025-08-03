using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] string _interactiveDebug = "None";

    private Interactive _currentInteractive;

    private void Update()
    {
        if (_currentInteractive != null)
        {
            if (Input.GetButtonDown("Interact")) _currentInteractive.Interaction();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Interactive interactive = collision.GetComponent<Interactive>();

        if (interactive != null && interactive != _currentInteractive)
        {
            _currentInteractive = interactive;
            _interactiveDebug = interactive.gameObject.name;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _currentInteractive = null;
        _interactiveDebug = "None";
    }
}
