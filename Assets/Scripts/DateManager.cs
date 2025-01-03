using System;
using UnityEngine;
using static EditManager;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class DateManager : MonoBehaviour
{
    public static DateManager Instance;
    public string selectedDate = DateTime.Now.ToString("yyyy-MM-dd");
    public string firstLoginDate;
    public string lastLoginDate;
    public int loginDays;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
        firstLoginDate = PlayerPrefs.GetString("firstLoginDate", DateTime.Now.ToString());
        PlayerPrefs.SetString("firstLoginDate", firstLoginDate);
        lastLoginDate = PlayerPrefs.GetString("LastLoginDate", DateTime.Now.ToString());
    }

    public void resetSelectedDate()
    { 
        selectedDate = DateTime.Now.ToString("yyyy-MM-dd");
    }

    public void LoadToDoSceneForDate(string date)
    {
        // ������ ��¥�� �����͸� PlayerPrefs���� �ε��ϰ�, �ش� ������ �̵�
        string json = PlayerPrefs.GetString("DailyTaskCollection", "{}");
        DailyTaskCollection collection = JsonUtility.FromJson<DailyTaskCollection>(json) ?? new DailyTaskCollection();

        DailyTaskData selectedData = collection.dailyTasks.Find(d => d.date == date);
        if (selectedData != null)
        {
            selectedDate = selectedData.date;
            // �ش� ��¥�� Task �����͸� PlayerPrefs�� �ӽ� ����
            string tasksJson = JsonUtility.ToJson(new Serialization<TaskData>(selectedData.tasks));
            PlayerPrefs.SetString("SelectedDayTasks", tasksJson);
            PlayerPrefs.Save();

            // ToDoScene���� �̵�
            SceneManager.LoadScene("ToDoScene");
        }
    }

    public void LoadToDoSceneToday()
    {
        LoadToDoSceneForDate(DateTime.Now.ToString("yyyy-MM-dd"));
    }

    public int DayCount()
    {
        return (DateTime.Parse(selectedDate) - DateTime.Parse(firstLoginDate)).Days + 1;
    }
}
