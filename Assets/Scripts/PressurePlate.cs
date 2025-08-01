using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Sprite defaultPressurePlate;
    [SerializeField] Sprite activatedPressurePlate;
    bool isPressed = false;
    [SerializeField] SpriteRenderer pressurePlateSprite;
    [SerializeField] UnityEvent OnPressurePlateEntered;
    [SerializeField] UnityEvent OnPressurePlateLeft;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPressed)
        {
            Debug.Log("Pressure Plate Activated!");
            isPressed = true;
            pressurePlateSprite.sprite = activatedPressurePlate;
            OnPressurePlateEntered.Invoke();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (isPressed)
        {
            Debug.Log("Pressure Plate Deactivated!");
            isPressed = false;
            pressurePlateSprite.sprite = defaultPressurePlate;
            OnPressurePlateLeft.Invoke();
        }
    }
}
