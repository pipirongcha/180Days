using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    public GameObject popup; //추가하기 버튼을 누르면 나와줄 팝업
    public GameObject taskPrefab; //할 일 목록(Scrollview의 Contents)으로 들어갈 task 오브젝트의 프리팹
    public Transform taskTransform; //task가 들어갈 위치(=task의 부모 오브젝트)
    public TMP_InputField inputField;

    List<GameObject> taskList = new List<GameObject>(); //task들이 모여있는 리스트
    GameObject selectedTask;

    void Start()
    {
        LoadTasks();
    }

    public void AddPressed() //추가하기 버튼 눌렀을 때 실행될 메서드
    {
        inputField.text = "";
        popup.SetActive(true);
    }

    public void AddCanclePressed() //팝업의 취소 버튼 눌렀을 때 실행될 메서드
    {
        popup.SetActive(false);
    }

    public void AddConfirmPressed() //팝업의 확인 버튼 눌렀을 때 실행될 메서드
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
        {
            return;
        }

        GameObject newTask = Instantiate(taskPrefab, taskTransform);
        TextMeshProUGUI taskText = newTask.GetComponentInChildren<TextMeshProUGUI>();
        taskText.text = inputField.text;

        Button taskButton = newTask.GetComponent<Button>(); //새로 추가된 task의 버튼 컴포넌트를 읽어오게 하고
        taskButton.onClick.AddListener(() => TaskPressed(newTask)); //해당 버튼 클릭 시, TaskPressed 함수가 동작하도록 지정함

        taskList.Add(newTask);
        popup.SetActive(false);

        SaveTasks();

    }


    public void TaskPressed(GameObject task) //task를 누르면 동작하는 함수
    {
        if (selectedTask != null)
        {
            selectedTask.GetComponent<Image>().color = Color.white;
        }

        selectedTask = task;
        selectedTask.GetComponent<Image>().color = Color.gray;
    }

    public void RemovePressed() //삭제하기 를 누르면 동작하는 함수
    {
        if (selectedTask != null)
        {
            taskList.Remove(selectedTask);
            Destroy(selectedTask);
            selectedTask = null;

            SaveTasks();
        }
    }
    void LoadTasks() //저장된 task를 불러오는 메서드
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

    // Serialization 클래스 정의
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

    void SaveTasks() //task를 저장하는 메서드
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
