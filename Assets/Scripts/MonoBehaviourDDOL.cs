using UnityEngine;

public abstract class MonoBehaviourDDOL<T> : MonoBehaviour
{
    private static T Instance;

    protected void SingletonCheck(T obj)
    {
        if (Instance == null)
        {
            Instance = obj;
            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(gameObject);
    }
}
