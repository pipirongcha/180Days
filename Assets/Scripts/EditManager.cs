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
    void LoadTasks() //저장된 Task를 불러오기
    {
       //TO-DO : 기존에 저장한 Task들 로드 구현
    }
    void SaveTasks() //추가 또는 삭제한 Task를 저장하기
    {
       
        //TO-DO: Task들 저장 구현
       
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
