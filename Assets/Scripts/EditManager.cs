using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    public static EditManager Instance;
    public GameObject popup; //추가하기 버튼을 누르면 나와줄 팝업
    public GameObject taskPrefab; //할 일 목록(Scrollview의 Contents)으로 들어갈 task 오브젝트의 프리팹
    public Transform taskTransform; //task가 들어갈 위치(=task의 부모 오브젝트)
    public TMP_InputField inputField;

    public List<TaskData> taskList = new List<TaskData>(); // Task 데이터 리스트
    GameObject selectedTask;

    private void Start()
    {
        Instance = this;
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

        TaskData newTaskData = new TaskData
        {
            text = inputField.text,
            completed = false // 기본값은 완료되지 않은 상태
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

    public void TaskPressed(GameObject task) // Task를 누르면 동작하는 함수
    {
        if (selectedTask != null) // 다른 Task를 선택하면, 이전에 선택한 Task는 다시 원래 색상으로 되돌림
        {
            selectedTask.GetComponent<Image>().color = Color.white;
        }

        selectedTask = task;
        selectedTask.GetComponent<Image>().color = Color.gray;
    }

    public void RemovePressed() //삭제하기를 누르면 동작하는 함수
    {
        if (selectedTask != null)
        {
            int index = selectedTask.transform.GetSiblingIndex(); // Task 인덱스 가져오기
            taskList.RemoveAt(index); // 해당 Task 데이터를 리스트에서 제거
            Destroy(selectedTask); // GameObject 파괴
            selectedTask = null;

            SaveTasks();
        }
    }

    void SaveTasks() // Task를 저장하는 메서드
    {
        string json = JsonUtility.ToJson(new Serialization<TaskData>(taskList));
        PlayerPrefs.SetString("TaskList", json);
        PlayerPrefs.Save();
    }

    public void LoadTasks() //저장된 Task를 불러오는 메서드
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
