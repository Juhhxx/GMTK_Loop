using UnityEngine;
using UnityEngine.Events;

public class Interactive : MonoBehaviour
{
    [SerializeField] UnityEvent OnInteraction;
    public void Interaction()
    {
        OnInteraction.Invoke();
    }
}
