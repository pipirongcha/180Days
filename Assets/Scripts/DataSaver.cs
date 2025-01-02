using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public int dayCount; // 몇 번째 날인지
    public List<string> tasks; // 할 일 목록
    public int achievePercent; // 완료된 할 일 수
    public string review; // 해당 날의 리뷰
}

public class DataSaver : MonoBehaviour
{
    public static DataSaver Instance;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SaveSceneData()
    {
        // ToDoManager의 싱글톤 인스턴스 접근
        ToDoManager toDoManager = ToDoManager.Instance;

        if (toDoManager == null)
        {
            Debug.LogError("ToDoManager Instance가 존재하지 않습니다!");
            return;
        }

        // 현재 씬 데이터를 객체로 생성
        SceneData sceneData = new SceneData
        {
            dayCount = toDoManager.dayCount,
            tasks = new List<string>(),
            achievePercent = toDoManager.achievePercent,
            review = toDoManager.review
        };

        // Task 목록을 저장
        foreach (Transform task in toDoManager.taskTransform)
        {
            TextMeshProUGUI textComponent = task.GetComponentInChildren<TextMeshProUGUI>();
            sceneData.tasks.Add(textComponent.text);
        }

        // JSON 형식으로 변환 후 저장
        string json = JsonUtility.ToJson(sceneData);
        PlayerPrefs.SetString("SceneData_" + toDoManager.dayCount, json); // 날짜별로 저장
        PlayerPrefs.Save();
    }
}





