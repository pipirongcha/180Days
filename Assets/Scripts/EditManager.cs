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
    void LoadTasks() //����� Task�� �ҷ�����
    {
       //TO-DO : ������ ������ Task�� �ε� ����
    }
    void SaveTasks() //�߰� �Ǵ� ������ Task�� �����ϱ�
    {
       
        //TO-DO: Task�� ���� ����
       
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
