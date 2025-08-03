using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField] Sprite defaultLeverSprite;
    [SerializeField] Sprite flippedLeverSprite;
    [SerializeField] SpriteRenderer leverSpriteRenderer;
    [SerializeField] UnityEvent OnLeverOff;
    [SerializeField] UnityEvent OnLeverOn;
    bool isFlipped = false;
    public void LeverFlipped()
    {
        if (!isFlipped)
        {
            isFlipped = true;
            leverSpriteRenderer.sprite = flippedLeverSprite;
            OnLeverOn.Invoke();
        }
        else
        {
            isFlipped = false;
            leverSpriteRenderer.sprite = defaultLeverSprite;
            OnLeverOff.Invoke();
        }
    }
}
