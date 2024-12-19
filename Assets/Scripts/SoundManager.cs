using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
