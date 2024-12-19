using UnityEngine;
using UnityEngine.SceneManagement;

public class EditManager : MonoBehaviour
{
    public GameObject popup;
    
    void Start()
    {
        //���� ��� �ҷ�����
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
        //������ ���� �۾�
        popup.SetActive(false);
    }

    public void RemovePressed()
    {
        //�ش� ���� ������ ���� �۾�
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
