using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static EditManager;

public class ToDoManager : MonoBehaviour
{

    public GameObject taskPrefab; //할 일 목록(Scrollview의 Contents)으로 들어갈 task 오브젝트의 프리팹
    public Transform taskTransform; //task가 들어갈 위치(=task의 부모 오브젝트)
    public TextMeshProUGUI AchievementText;

    public int DaysCount; //AchievementText의 'n번째 기적의 날'에 들어갈 숫자 n 
    public int AchievePercent = 0;
    float taskCount;
    float completeCount;
    string review;



    void Start()
    {
        LoadTasks();
        LoadMent();
    }

    public void LoadTasks() //저장된 task를 불러오는 메서드
    {
        if (PlayerPrefs.HasKey("TaskList"))
        {
            string json = PlayerPrefs.GetString("TaskList");
            List<string> taskTexts = JsonUtility.FromJson<Serialization<string>>(json).ToList();

            foreach (string taskText in taskTexts)
            {
                taskCount++;
                GameObject newTask = Instantiate(taskPrefab, taskTransform);
                TextMeshProUGUI textComponent = newTask.GetComponentInChildren<TextMeshProUGUI>();
                textComponent.text = taskText;

                Button taskButton = newTask.GetComponent<Button>();
                taskButton.onClick.AddListener(() => TaskComplete(newTask));
            }
        }
    }

    void LoadMent()
    {
        AchievePercent = (int)(completeCount / taskCount * 100) ;

        if (AchievePercent <= 25)
        {
            review = "많이 부족합니다.";
        }
        else if(AchievePercent > 25 && AchievePercent <= 50)
        {
            review = "더 필요합니다.";
        }
        else if(AchievePercent > 50 && AchievePercent <= 75)
        {
            review = "적당량 모였습니다.";
        }
        else if(AchievePercent == 100)
        {
            review = "전부 모였습니다!";
        }

        AchievementText.text = "오늘로 " + DaysCount + "번째 기적의 날입니다.\r\n일일 기적 달성률은 " + AchievePercent + "% 입니다.\r\n오늘의 기적이 " + review;
    }
    public void TaskComplete(GameObject task)
    {
        TextMeshProUGUI clickedTask = task.GetComponentInChildren<TextMeshProUGUI>();

        // 취소선이 그어져 있다면
        if ((clickedTask.fontStyle & FontStyles.Strikethrough) == FontStyles.Strikethrough)
        {
            clickedTask.fontStyle &= ~FontStyles.Strikethrough; // 취소선 제거
            completeCount--;
            LoadMent();
        }
        else
        {
            //취소선이 없다면, 취소선 추가 (두꺼운 글씨는 유지 유지)
            clickedTask.fontStyle |= FontStyles.Strikethrough;
            completeCount++;
            LoadMent();
        }
    }
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TItleScene");
        }
    }
}
