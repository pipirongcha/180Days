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
        LoadProgress(); // 게임 시작 시, 마지막 로그인 날짜를 불러오기
        CheckDate(); // 날짜 비교 후 숫자 증가 여부 체크
        SaveProgress(); // 최신 정보 저장

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
        // PlayerPrefs에서 마지막 로그인 날짜를 불러옴
        string lastLoginString = PlayerPrefs.GetString("LastLoginDate", DateTime.Now.ToString());
        lastLoginDate = DateTime.Parse(lastLoginString);

        // PlayerPrefs에서 저장된 일수 불러오기
        dayCount = PlayerPrefs.GetInt("DayCount", 1);
    }

    void CheckDate()
    {
        // 오늘 날짜와 마지막 로그인 날짜를 비교
        DateTime currentDate = DateTime.Now;

        // 날짜가 달라졌다면
        if (currentDate.Date > lastLoginDate.Date)
        {
          
            // 날짜 차이만큼 dayCount 증가
            int daysPassed = (currentDate - lastLoginDate).Days;
            dayCount += daysPassed; // 며칠이 지났는지에 따라 일수 증가
            dayChanged = true;
            if (loginDays < 180)
            {
                loginDays++;
            }
        }
    }

    void SaveProgress()
    {
        // 현재 날짜를 문자열로 저장
        PlayerPrefs.SetString("LastLoginDate", DateTime.Now.ToString());
        // 증가된 일수를 저장
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
