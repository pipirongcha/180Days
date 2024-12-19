using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    GameObject ExitPopup;

  public void EditPressed()
    {
        SceneManager.LoadScene("EditScene");
    }

    public void ToDoPressed()
    {
        SceneManager.LoadScene("ToDoScene");

    }

    public void CollectionPressed()
    {
        SceneManager.LoadScene("CollectionScene");
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }

    public void ExitCanclePressed()
    {
        ExitPopup.SetActive(false);
    }

   void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPopup.SetActive(true);
        }
    }
}
