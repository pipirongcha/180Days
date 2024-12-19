using UnityEngine;
using UnityEngine.SceneManagement;

public class EditManager : MonoBehaviour
{
    public GameObject popup;
    
    void Start()
    {
        //저장 기록 불러오기
    }

    public void AddPressed()
    {
        popup.SetActive(true);
        
    }

    public void AddCanclePressed()
    {
        popup.SetActive(false);
    }

   public void AddConfirmPressed()
    {
        //프리팹 저장 작업
        popup.SetActive(false);
    }

    public void RemovePressed()
    {
        //해당 단일 프리팹 삭제 작업
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
