using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Sprite defaultPressurePlate;
    [SerializeField] Sprite activatedPressurePlate;
    [SerializeField] SpriteRenderer pressurePlateSprite;
    [SerializeField] UnityEvent OnPressurePlateEntered;
    [SerializeField] UnityEvent OnPressurePlateLeft;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Pressure Plate Activated!");
        pressurePlateSprite.sprite = activatedPressurePlate;
        OnPressurePlateEntered?.Invoke();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Pressure Plate Deactivated!");
        pressurePlateSprite.sprite = defaultPressurePlate;
        OnPressurePlateLeft?.Invoke();
    }
}
