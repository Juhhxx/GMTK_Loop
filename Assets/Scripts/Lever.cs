using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField] Sprite defaultLeverSprite;
    [SerializeField] Sprite flippedLeverSprite;
    [SerializeField] SpriteRenderer leverSpriteRenderer;
    [SerializeField] UnityEvent OnLeverOff;
    [SerializeField] UnityEvent OnLeverOn;
    [SerializeField] KeyCode interactKey;
    bool isFlipped = false;
    [SerializeField] Rigidbody2D rb2d;
    void OnTriggerStay2D(Collider2D collision)
    {
        rb2d.sleepMode = RigidbodySleepMode2D.NeverSleep;
        if (Input.GetKeyDown(interactKey) && rb2d != null)
        {
            LeverFlipped();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        rb2d.sleepMode = RigidbodySleepMode2D.StartAwake;
    } 
    public void LeverFlipped()
    {
        if (!isFlipped)
        {
            Debug.Log("aaaaaaa");
            isFlipped = true;
            leverSpriteRenderer.sprite = flippedLeverSprite;
            OnLeverOn.Invoke();
        }
        else
        {
            Debug.Log("pppppppp");
            isFlipped = false;
            leverSpriteRenderer.sprite = defaultLeverSprite;
            OnLeverOff.Invoke();
        }
    }
}
