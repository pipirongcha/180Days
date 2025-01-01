using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReportManager : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    int dayCount;
    int loginDays;
    int totalAchievement;
    void Start()
    {
        totalAchievement = PlayerPrefs.GetInt("TotalAchievement", 0);
        dayCount = PlayerPrefs.GetInt("DayCount", 1);
        loginDays = PlayerPrefs.GetInt("LoginDays", 1);

        int totalAchievePercent = totalAchievement / loginDays;

        if (dayCount > 180)
        {
            dayCount = 180;
        }

        if (loginDays > 180)
        {
            loginDays = 180;
        }

        if (dayCount >= 180)
        {
            textBox.text = "180일간 고생하셨습니다.\n다시 기적을 쌓고싶다면, 메인화면에서 ESC를 눌러 리셋해주세요." +
                "180일 중 " + loginDays+ "일 출석하셨습니다.\n 당신의 평균 기적 수집률은 " + totalAchievePercent + "% 입니다.";
        }
        else
        {
            textBox.text = "오늘로 " + dayCount + "일째 날이며, " + (loginDays - 1) + "일 출석하셨습니다.\n 당신의 평균 기적 수집률은 " + totalAchievePercent + "% 입니다."
                +"\n(전날을 기준으로 하여, 금일 기록은 오전 12시 이후에 반영됩니다.)";
        }

    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (dayCount >= 180)
            {
                SceneManager.LoadScene("TitleScene");
            }
            else
            {
                SceneManager.LoadScene("CollectionScene");
            }
        }
    }
}
