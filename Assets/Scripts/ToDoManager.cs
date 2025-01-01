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
    public GameObject taskPrefab; //할 일 목록(Scrollview의 Contents)으로 들어갈 task 오브젝트의 프리팹
    public Transform taskTransform; //task가 들어갈 위치(=task의 부모 오브젝트)
    public TextMeshProUGUI achievementText;

    public int dayCount; //AchievementText의 'n번째 기적의 날'에 들어갈 숫자 n 
    public int achievePercent = 0;
    float taskCount = 0;
    public float completeCount = 0;
    public string review;
    
    private DateTime lastLoginDate;
    public List<TaskData> taskList = new List<TaskData>(); // Task 데이터 리스트

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
                taskCount++; // 전체 Task 개수 증가
                GameObject newTask = Instantiate(taskPrefab, taskTransform);
                TextMeshProUGUI textComponent = newTask.GetComponentInChildren<TextMeshProUGUI>();
                textComponent.text = taskData.text;

                if (taskData.completed)
                {
                    completeCount++; // 완료된 Task 개수 증가
                    textComponent.fontStyle |= FontStyles.Strikethrough;
                }

                Button taskButton = newTask.GetComponent<Button>();
                taskButton.onClick.AddListener(() => ToggleTaskCompletion(newTask, taskData));
            }

            LoadMent(); // 달성률 업데이트
        }
    }

    public void ToggleTaskCompletion(GameObject task, TaskData taskData)
    {
        TextMeshProUGUI textComponent = task.GetComponentInChildren<TextMeshProUGUI>();

        // 완료 상태를 토글
        taskData.completed = !taskData.completed;

        // Strikethrough 스타일 추가/제거
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

        // 변경된 Task 리스트 저장
        SaveTasks();
        SaveDailyTasks();
    }

    void LoadMent()
    {
        achievePercent = taskCount > 0 ? (int)(completeCount / taskCount * 100) : 0;

        if (achievePercent <= 25)
        {
            review = "많이 부족합니다.";
        }
        else if(achievePercent > 25 && achievePercent <= 50)
        {
            review = "더 필요합니다.";
        }
        else if(achievePercent > 50 && achievePercent <= 75)
        {
            review = "적당량 모였습니다.";
        }
        else if(achievePercent == 100)
        {
            review = "전부 모였습니다!";
        }

        achievementText.text = "오늘로 " + dayCount + "번째 기적의 날입니다.\r\n일일 기적 달성률은 " + achievePercent + "% 입니다.\r\n오늘의 기적이 " + review;
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
        // Task 목록에서 완료된 항목을 초기화 (밑줄 제거)
        foreach (TaskData taskData in taskList)
        {
            if (taskData.completed)
            {
                taskData.completed = false; // 완료 상태 초기화
            }
        }

        // Task 목록을 저장하여 상태 변경을 반영
        SaveTasks();
        LoadTasks(); // 변경된 상태로 할 일 목록을 다시 로드하여 화면 갱신
    }

    public void SaveTasks()
    {
        // Task 리스트를 JSON으로 변환하여 저장
        string json = JsonUtility.ToJson(new Serialization<TaskData>(taskList));
        PlayerPrefs.SetString("TaskList", json);
        PlayerPrefs.Save();
    }
    public void SaveDailyTasks()
    {
        // 기존 데이터를 불러옴
        string json = PlayerPrefs.GetString("DailyTaskCollection", "{}");
        DailyTaskCollection collection = JsonUtility.FromJson<DailyTaskCollection>(json) ?? new DailyTaskCollection();

        // 현재 날짜 확인
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        // 현재 날짜에 해당하는 기록이 있는지 확인
        DailyTaskData existingData = collection.dailyTasks.Find(d => d.date == currentDate);

        if (existingData == null)
        {
            // 새로 추가
            DailyTaskData newData = new DailyTaskData
            {
                date = currentDate,
                tasks = new List<TaskData>(taskList) // 현재 Task 데이터를 복사
            };
            collection.dailyTasks.Add(newData);
        }
        else
        {
            // 기존 데이터를 업데이트
            existingData.tasks = new List<TaskData>(taskList);
        }

        // 저장
        string updatedJson = JsonUtility.ToJson(collection);
        PlayerPrefs.SetString("DailyTaskCollection", updatedJson);
        PlayerPrefs.Save();
    }

    

 


}

