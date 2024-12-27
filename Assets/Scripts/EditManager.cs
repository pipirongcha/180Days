using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    public static EditManager Instance;
    public GameObject popup; //�߰��ϱ� ��ư�� ������ ������ �˾�
    public GameObject taskPrefab; //�� �� ���(Scrollview�� Contents)���� �� task ������Ʈ�� ������
    public Transform taskTransform; //task�� �� ��ġ(=task�� �θ� ������Ʈ)
    public TMP_InputField inputField;

    public List<TaskData> taskList = new List<TaskData>(); // Task ������ ����Ʈ
    GameObject selectedTask;

    private void Start()
    {
        Instance = this;
        LoadTasks();
    }

    public void AddPressed() //�߰��ϱ� ��ư ������ �� ����� �޼���
    {
        inputField.text = "";
        popup.SetActive(true);
    }

    public void AddCanclePressed() //�˾��� ��� ��ư ������ �� ����� �޼���
    {
        popup.SetActive(false);
    }

    public void AddConfirmPressed() //�˾��� Ȯ�� ��ư ������ �� ����� �޼���
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
        {
            return;
        }

        TaskData newTaskData = new TaskData
        {
            text = inputField.text,
            completed = false // �⺻���� �Ϸ���� ���� ����
        };

        GameObject newTask = Instantiate(taskPrefab, taskTransform);
        TextMeshProUGUI taskText = newTask.GetComponentInChildren<TextMeshProUGUI>();
        taskText.text = newTaskData.text;

        Button taskButton = newTask.GetComponent<Button>();
        taskButton.onClick.AddListener(() => TaskPressed(newTask));

        taskList.Add(newTaskData);
        popup.SetActive(false);

        SaveTasks();
    }

    public void TaskPressed(GameObject task) // Task�� ������ �����ϴ� �Լ�
    {
        if (selectedTask != null) // �ٸ� Task�� �����ϸ�, ������ ������ Task�� �ٽ� ���� �������� �ǵ���
        {
            selectedTask.GetComponent<Image>().color = Color.white;
        }

        selectedTask = task;
        selectedTask.GetComponent<Image>().color = Color.gray;
    }

    public void RemovePressed() //�����ϱ⸦ ������ �����ϴ� �Լ�
    {
        if (selectedTask != null)
        {
            int index = selectedTask.transform.GetSiblingIndex(); // Task �ε��� ��������
            taskList.RemoveAt(index); // �ش� Task �����͸� ����Ʈ���� ����
            Destroy(selectedTask); // GameObject �ı�
            selectedTask = null;

            SaveTasks();
        }
    }

    void SaveTasks() // Task�� �����ϴ� �޼���
    {
        string json = JsonUtility.ToJson(new Serialization<TaskData>(taskList));
        PlayerPrefs.SetString("TaskList", json);
        PlayerPrefs.Save();
    }

    public void LoadTasks() //����� Task�� �ҷ����� �޼���
    {
        if (PlayerPrefs.HasKey("TaskList"))
        {
            string json = PlayerPrefs.GetString("TaskList");
            taskList = JsonUtility.FromJson<Serialization<TaskData>>(json).ToList();

            foreach (TaskData taskData in taskList)
            {
                GameObject newTask = Instantiate(taskPrefab, taskTransform);
                TextMeshProUGUI textComponent = newTask.GetComponentInChildren<TextMeshProUGUI>();
                textComponent.text = taskData.text;

                Button taskButton = newTask.GetComponent<Button>();
                taskButton.onClick.AddListener(() => TaskPressed(newTask));
            }
        }
    }

    // Serialization Ŭ���� ����
    [System.Serializable]
    public class Serialization<T>
    {
        public List<T> Target;

        public Serialization(List<T> target)
        {
            Target = target;
        }

        public List<T> ToList()
        {
            return Target;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
