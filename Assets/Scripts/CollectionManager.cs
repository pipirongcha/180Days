using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static EditManager;
using System.Collections.Generic;

public class CollectionManager : MonoBehaviour
{
    public Button loadButtonPrefab; // 날짜별로 ToDoScene을 로드할 버튼 프리팹
    public Transform buttonParent; // 버튼들이 배치될 부모 오브젝트 (예: ScrollView)

    void Start()
    {
        LoadDailyRecords(); // 시작 시, 기록들을 로드하여 버튼을 생성
    }

    void LoadDailyRecords()
    {
        // PlayerPrefs에서 DailyTaskCollection 데이터 불러오기
        string json = PlayerPrefs.GetString("DailyTaskCollection", "{}");
        DailyTaskCollection collection = JsonUtility.FromJson<DailyTaskCollection>(json) ?? new DailyTaskCollection();

        // 각 날짜에 대한 버튼 생성
        foreach (DailyTaskData dailyTask in collection.dailyTasks)
        {
            // 새로운 버튼 생성
            Button newButton = Instantiate(loadButtonPrefab, buttonParent);
            // 버튼 텍스트 설정 (날짜 표시)
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = dailyTask.date;

            // 버튼 클릭 시 해당 날짜의 작업 데이터를 로드하여 ToDoScene으로 이동
            newButton.onClick.AddListener(() => LoadToDoSceneForDate(dailyTask.date));
        }
    }

    void LoadToDoSceneForDate(string date)
    {
        // 선택한 날짜의 데이터를 PlayerPrefs에서 로드하고, 해당 씬으로 이동
        string json = PlayerPrefs.GetString("DailyTaskCollection", "{}");
        DailyTaskCollection collection = JsonUtility.FromJson<DailyTaskCollection>(json) ?? new DailyTaskCollection();

        DailyTaskData selectedData = collection.dailyTasks.Find(d => d.date == date);
        if (selectedData != null)
        {
            // 해당 날짜의 Task 데이터를 PlayerPrefs에 임시 저장
            string tasksJson = JsonUtility.ToJson(new Serialization<TaskData>(selectedData.tasks));
            PlayerPrefs.SetString("SelectedDayTasks", tasksJson);
            PlayerPrefs.Save();

            // ToDoScene으로 이동
            SceneManager.LoadScene("ToDoScene");
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
