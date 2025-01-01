using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EditManager;

public class ToDoManager : MonoBehaviour
{
    public static ToDoManager Instance; 
    public GameObject taskPrefab; //�� �� ���(Scrollview�� Contents)���� �� task ������Ʈ�� ������
    public Transform taskTransform; //task�� �� ��ġ(=task�� �θ� ������Ʈ)
    public TextMeshProUGUI achievementText;

    public int dayCount; //AchievementText�� 'n��° ������ ��'�� �� ���� n 
    public int achievePercent = 0;
    float taskCount = 0;
    public float completeCount = 0;
    public string review;
    
    private DateTime lastLoginDate;
    public List<TaskData> taskList = new List<TaskData>(); // Task ������ ����Ʈ

    int totalAchievement;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        dayCount = PlayerPrefs.GetInt("DayCount", 1);
        totalAchievement = PlayerPrefs.GetInt("TotalAchievement", 0);
        LoadTasks();
        if(TitleManager.Instance.dayChanged == true)
        {
            
            ResetCompletedTasks();
            TitleManager.Instance.dayChanged = false;
        }
    }

    public void LoadTasks()
    {
        if (PlayerPrefs.HasKey("TaskList"))
        {
            string json = PlayerPrefs.GetString("TaskList");
            taskList = JsonUtility.FromJson<Serialization<TaskData>>(json).ToList();

            foreach (TaskData taskData in taskList)
            {
                taskCount++; // ��ü Task ���� ����
                GameObject newTask = Instantiate(taskPrefab, taskTransform);
                TextMeshProUGUI textComponent = newTask.GetComponentInChildren<TextMeshProUGUI>();
                textComponent.text = taskData.text;

                if (taskData.completed)
                {
                    completeCount++; // �Ϸ�� Task ���� ����
                    textComponent.fontStyle |= FontStyles.Strikethrough;
                }

                Button taskButton = newTask.GetComponent<Button>();
                taskButton.onClick.AddListener(() => ToggleTaskCompletion(newTask, taskData));
            }

            LoadMent(); // �޼��� ������Ʈ
        }
    }

    public void ToggleTaskCompletion(GameObject task, TaskData taskData)
    {
        TextMeshProUGUI textComponent = task.GetComponentInChildren<TextMeshProUGUI>();

        // �Ϸ� ���¸� ���
        taskData.completed = !taskData.completed;

        // Strikethrough ��Ÿ�� �߰�/����
        if (taskData.completed)
        {
            textComponent.fontStyle |= FontStyles.Strikethrough;
            completeCount++;
            LoadMent();
        }
        else
        {
            textComponent.fontStyle &= ~FontStyles.Strikethrough;
            completeCount--;
            LoadMent();
        }

        // ����� Task ����Ʈ ����
        SaveTasks();
        SaveDailyTasks();
    }

    void LoadMent()
    {
        achievePercent = taskCount > 0 ? (int)(completeCount / taskCount * 100) : 0;

        if (achievePercent <= 25)
        {
            review = "���� �����մϴ�.";
        }
        else if(achievePercent > 25 && achievePercent <= 50)
        {
            review = "�� �ʿ��մϴ�.";
        }
        else if(achievePercent > 50 && achievePercent <= 75)
        {
            review = "���緮 �𿴽��ϴ�.";
        }
        else if(achievePercent == 100)
        {
            review = "���� �𿴽��ϴ�!";
        }

        achievementText.text = "���÷� " + dayCount + "��° ������ ���Դϴ�.\r\n���� ���� �޼����� " + achievePercent + "% �Դϴ�.\r\n������ ������ " + review;
    }
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DataSaver.Instance.SaveSceneData();
            SaveDailyTasks();
            SceneManager.LoadScene("TItleScene");
        }
    }

   

    void ResetCompletedTasks()
    {
        totalAchievement += achievePercent;
        PlayerPrefs.SetInt("TotalAchievement", totalAchievement);
        PlayerPrefs.Save();
        // Task ��Ͽ��� �Ϸ�� �׸��� �ʱ�ȭ (���� ����)
        foreach (TaskData taskData in taskList)
        {
            if (taskData.completed)
            {
                taskData.completed = false; // �Ϸ� ���� �ʱ�ȭ
            }
        }

        // Task ����� �����Ͽ� ���� ������ �ݿ�
        SaveTasks();
        LoadTasks(); // ����� ���·� �� �� ����� �ٽ� �ε��Ͽ� ȭ�� ����
    }

    public void SaveTasks()
    {
        // Task ����Ʈ�� JSON���� ��ȯ�Ͽ� ����
        string json = JsonUtility.ToJson(new Serialization<TaskData>(taskList));
        PlayerPrefs.SetString("TaskList", json);
        PlayerPrefs.Save();
    }
    public void SaveDailyTasks()
    {
        // ���� �����͸� �ҷ���
        string json = PlayerPrefs.GetString("DailyTaskCollection", "{}");
        DailyTaskCollection collection = JsonUtility.FromJson<DailyTaskCollection>(json) ?? new DailyTaskCollection();

        // ���� ��¥ Ȯ��
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        // ���� ��¥�� �ش��ϴ� ����� �ִ��� Ȯ��
        DailyTaskData existingData = collection.dailyTasks.Find(d => d.date == currentDate);

        if (existingData == null)
        {
            // ���� �߰�
            DailyTaskData newData = new DailyTaskData
            {
                date = currentDate,
                tasks = new List<TaskData>(taskList) // ���� Task �����͸� ����
            };
            collection.dailyTasks.Add(newData);
        }
        else
        {
            // ���� �����͸� ������Ʈ
            existingData.tasks = new List<TaskData>(taskList);
        }

        // ����
        string updatedJson = JsonUtility.ToJson(collection);
        PlayerPrefs.SetString("DailyTaskCollection", updatedJson);
        PlayerPrefs.Save();
    }

    

 


}

