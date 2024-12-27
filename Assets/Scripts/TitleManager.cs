using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    GameObject Popup;
    [SerializeField]
    GameObject ResetPopup;
    [SerializeField]
    GameObject ResetCompletePopup;
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

    public void CanclePressed()
    {
        Popup.SetActive(false);
        ResetPopup.SetActive(false);
    }

    public void ResetPressed()
    {
        Popup.SetActive(false);
        ResetPopup.SetActive(true);
    }

    public void ResetOKPressed()
    {
        PlayerPrefs.DeleteAll();
        ResetPopup.SetActive(false);
        ResetCompletePopup.SetActive(true);
    }

    public void ResetCompleteClosePressed()
    {
        ResetCompletePopup.SetActive(false);

    }

   void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Popup.SetActive(true);
        }
    }
}
