using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionManager : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TItleScene");
        }
    }
}
