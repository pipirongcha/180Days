using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static EditManager;
using System.Collections.Generic;

public class CollectionManager : MonoBehaviour
{
    public Button loadButtonPrefab; // ��¥���� ToDoScene�� �ε��� ��ư ������
    public Transform buttonParent; // ��ư���� ��ġ�� �θ� ������Ʈ (��: ScrollView)

    void Start()
    {
        LoadDailyRecords(); // ���� ��, ��ϵ��� �ε��Ͽ� ��ư�� ����
    }

    void LoadDailyRecords()
    {
        // PlayerPrefs���� DailyTaskCollection ������ �ҷ�����
        string json = PlayerPrefs.GetString("DailyTaskCollection", "{}");
        DailyTaskCollection collection = JsonUtility.FromJson<DailyTaskCollection>(json) ?? new DailyTaskCollection();

        // �� ��¥�� ���� ��ư ����
        foreach (DailyTaskData dailyTask in collection.dailyTasks)
        {
            // ���ο� ��ư ����
            Button newButton = Instantiate(loadButtonPrefab, buttonParent);
            // ��ư �ؽ�Ʈ ���� (��¥ ǥ��)
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = dailyTask.date;

            // ��ư Ŭ�� �� �ش� ��¥�� �۾� �����͸� �ε��Ͽ� ToDoScene���� �̵�
            newButton.onClick.AddListener(() => LoadToDoSceneForDate(dailyTask.date));
        }
    }

    void LoadToDoSceneForDate(string date)
    {
        // ������ ��¥�� �����͸� PlayerPrefs���� �ε��ϰ�, �ش� ������ �̵�
        string json = PlayerPrefs.GetString("DailyTaskCollection", "{}");
        DailyTaskCollection collection = JsonUtility.FromJson<DailyTaskCollection>(json) ?? new DailyTaskCollection();

        DailyTaskData selectedData = collection.dailyTasks.Find(d => d.date == date);
        if (selectedData != null)
        {
            DateManager.Instance.selectedDate = selectedData.date;
            // �ش� ��¥�� Task �����͸� PlayerPrefs�� �ӽ� ����
            string tasksJson = JsonUtility.ToJson(new Serialization<TaskData>(selectedData.tasks));
            PlayerPrefs.SetString("SelectedDayTasks", tasksJson);
            PlayerPrefs.Save();

            // ToDoScene���� �̵�
            SceneManager.LoadScene("ToDoScene");
        }
    }

    public void ReportPressed()
    {
        SceneManager.LoadScene("ReportScene");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
