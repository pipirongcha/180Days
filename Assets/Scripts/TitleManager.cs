using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public static TitleManager Instance; 
    [SerializeField]
    GameObject Popup;
    [SerializeField]
    GameObject ResetPopup;
    [SerializeField]
    GameObject ResetCompletePopup;

    private DateTime lastLoginDate;
    private int dayCount;
    private int loginDays;
    public bool dayChanged = false;
    
    private void Start()
    {
        loginDays = PlayerPrefs.GetInt("LoginDays", 1);
        LoadProgress(); // ���� ���� ��, ������ �α��� ��¥�� �ҷ�����
        CheckDate(); // ��¥ �� �� ���� ���� ���� üũ
        SaveProgress(); // �ֽ� ���� ����

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void LoadProgress()
    {
        // PlayerPrefs���� ������ �α��� ��¥�� �ҷ���
        string lastLoginString = PlayerPrefs.GetString("LastLoginDate", DateTime.Now.ToString());
        lastLoginDate = DateTime.Parse(lastLoginString);

        // PlayerPrefs���� ����� �ϼ� �ҷ�����
        dayCount = PlayerPrefs.GetInt("DayCount", 1);
    }

    void CheckDate()
    {
        // ���� ��¥�� ������ �α��� ��¥�� ��
        DateTime currentDate = DateTime.Now;

        // ��¥�� �޶����ٸ�
        if (currentDate.Date > lastLoginDate.Date)
        {
          
            // ��¥ ���̸�ŭ dayCount ����
            int daysPassed = (currentDate - lastLoginDate).Days;
            dayCount += daysPassed; // ��ĥ�� ���������� ���� �ϼ� ����
            dayChanged = true;
            if (loginDays < 180)
            {
                loginDays++;
            }
        }
    }

    void SaveProgress()
    {
        // ���� ��¥�� ���ڿ��� ����
        PlayerPrefs.SetString("LastLoginDate", DateTime.Now.ToString());
        // ������ �ϼ��� ����
        PlayerPrefs.SetInt("DayCount", dayCount);
        PlayerPrefs.SetInt("LoginDays", loginDays);
        PlayerPrefs.Save();
    }

    
    public void EditPressed()
    {
        
        if (dayCount >= 180)
        {
            SceneManager.LoadScene("ReportScene");
        }
        SceneManager.LoadScene("EditScene");
    }

    public void ToDoPressed()
    {
        if (dayCount >= 180)
        {
            SceneManager.LoadScene("ReportScene");
        }
        else
        {
            SceneManager.LoadScene("ToDoScene");
        }
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
