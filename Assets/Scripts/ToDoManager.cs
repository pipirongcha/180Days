using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static EditManager;

public class ToDoManager : MonoBehaviour
{

    public GameObject taskPrefab; //�� �� ���(Scrollview�� Contents)���� �� task ������Ʈ�� ������
    public Transform taskTransform; //task�� �� ��ġ(=task�� �θ� ������Ʈ)
    public TextMeshProUGUI AchievementText;

    public int DaysCount; //AchievementText�� 'n��° ������ ��'�� �� ���� n 
    public int AchievePercent = 0;
    float taskCount;
    float completeCount;
    string review;



    void Start()
    {
        LoadTasks();
        LoadMent();
    }

    public void LoadTasks() //����� task�� �ҷ����� �޼���
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
            review = "���� �����մϴ�.";
        }
        else if(AchievePercent > 25 && AchievePercent <= 50)
        {
            review = "�� �ʿ��մϴ�.";
        }
        else if(AchievePercent > 50 && AchievePercent <= 75)
        {
            review = "���緮 �𿴽��ϴ�.";
        }
        else if(AchievePercent == 100)
        {
            review = "���� �𿴽��ϴ�!";
        }

        AchievementText.text = "���÷� " + DaysCount + "��° ������ ���Դϴ�.\r\n���� ���� �޼����� " + AchievePercent + "% �Դϴ�.\r\n������ ������ " + review;
    }
    public void TaskComplete(GameObject task)
    {
        TextMeshProUGUI clickedTask = task.GetComponentInChildren<TextMeshProUGUI>();

        // ��Ҽ��� �׾��� �ִٸ�
        if ((clickedTask.fontStyle & FontStyles.Strikethrough) == FontStyles.Strikethrough)
        {
            clickedTask.fontStyle &= ~FontStyles.Strikethrough; // ��Ҽ� ����
            completeCount--;
            LoadMent();
        }
        else
        {
            //��Ҽ��� ���ٸ�, ��Ҽ� �߰� (�β��� �۾��� ���� ����)
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
