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
            textBox.text = "180�ϰ� ����ϼ̽��ϴ�.\n�ٽ� ������ �װ�ʹٸ�, ����ȭ�鿡�� ESC�� ���� �������ּ���." +
                "180�� �� " + loginDays+ "�� �⼮�ϼ̽��ϴ�.\n ����� ��� ���� �������� " + totalAchievePercent + "% �Դϴ�.";
        }
        else
        {
            textBox.text = "���÷� " + dayCount + "��° ���̸�, " + (loginDays - 1) + "�� �⼮�ϼ̽��ϴ�.\n ����� ��� ���� �������� " + totalAchievePercent + "% �Դϴ�."
                +"\n(������ �������� �Ͽ�, ���� ����� ���� 12�� ���Ŀ� �ݿ��˴ϴ�.)";
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
