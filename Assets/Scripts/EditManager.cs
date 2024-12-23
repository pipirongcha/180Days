using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    public GameObject popup; //�߰��ϱ� ��ư�� ������ ������ �˾�
    public GameObject taskPrefab; //�� �� ���(Scrollview�� Contents)���� �� task ������Ʈ�� ������
    public Transform taskTransform; //task�� �� ��ġ(=task�� �θ� ������Ʈ)
    public TMP_InputField inputField;

    List<GameObject> taskList = new List<GameObject>(); //task���� ���ִ� ����Ʈ
    GameObject selectedTask;

    void Start()
    {
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

        GameObject newTask = Instantiate(taskPrefab, taskTransform);
        TextMeshProUGUI taskText = newTask.GetComponentInChildren<TextMeshProUGUI>();
        taskText.text = inputField.text;

        Button taskButton = newTask.GetComponent<Button>(); //���� �߰��� task�� ��ư ������Ʈ�� �о���� �ϰ�
        taskButton.onClick.AddListener(() => TaskPressed(newTask)); //�ش� ��ư Ŭ�� ��, TaskPressed �Լ��� �����ϵ��� ������

        taskList.Add(newTask);
        popup.SetActive(false);

        SaveTasks();

    }


    public void TaskPressed(GameObject task) //task�� ������ �����ϴ� �Լ�
    {
        if (selectedTask != null)
        {
            selectedTask.GetComponent<Image>().color = Color.white;
        }

        selectedTask = task;
        selectedTask.GetComponent<Image>().color = Color.gray;
    }

    public void RemovePressed() //�����ϱ� �� ������ �����ϴ� �Լ�
    {
        if (selectedTask != null)
        {
            taskList.Remove(selectedTask);
            Destroy(selectedTask);
            selectedTask = null;

            SaveTasks();
        }
    }
    void LoadTasks() //����� task�� �ҷ����� �޼���
    {
        if (PlayerPrefs.HasKey("TaskList"))
        {
            string json = PlayerPrefs.GetString("TaskList");
            List<string> taskTexts = JsonUtility.FromJson<Serialization<string>>(json).ToList();

            foreach (string taskText in taskTexts)
            {
                GameObject newTask = Instantiate(taskPrefab, taskTransform);
                TextMeshProUGUI textComponent = newTask.GetComponentInChildren<TextMeshProUGUI>();
                textComponent.text = taskText;

                Button taskButton = newTask.GetComponent<Button>();
                taskButton.onClick.AddListener(() => TaskPressed(newTask));

                taskList.Add(newTask);
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

    void SaveTasks() //task�� �����ϴ� �޼���
    {
        List<string> taskTexts = new List<string>();

        foreach (GameObject task in taskList)
        {
            string taskText = task.GetComponentInChildren<TextMeshProUGUI>().text;
            taskTexts.Add(taskText);
        }

        string json = JsonUtility.ToJson(new Serialization<string>(taskTexts));
        PlayerPrefs.SetString("TaskList", json);
        PlayerPrefs.Save();
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

}
