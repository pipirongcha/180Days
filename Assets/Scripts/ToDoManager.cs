using UnityEngine;
using UnityEngine.SceneManagement;

public class ToDoManager : MonoBehaviour
{
   
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TItleScene");
        }
    }
}
