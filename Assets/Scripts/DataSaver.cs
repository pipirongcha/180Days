using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public int dayCount; // �� ��° ������
    public List<string> tasks; // �� �� ���
    public int achievePercent; // �Ϸ�� �� �� ��
    public string review; // �ش� ���� ����
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
        // ToDoManager�� �̱��� �ν��Ͻ� ����
        ToDoManager toDoManager = ToDoManager.Instance;

        if (toDoManager == null)
        {
            Debug.LogError("ToDoManager Instance�� �������� �ʽ��ϴ�!");
            return;
        }

        // ���� �� �����͸� ��ü�� ����
        SceneData sceneData = new SceneData
        {
            dayCount = toDoManager.dayCount,
            tasks = new List<string>(),
            achievePercent = toDoManager.achievePercent,
            review = toDoManager.review
        };

        // Task ����� ����
        foreach (Transform task in toDoManager.taskTransform)
        {
            TextMeshProUGUI textComponent = task.GetComponentInChildren<TextMeshProUGUI>();
            sceneData.tasks.Add(textComponent.text);
        }

        // JSON �������� ��ȯ �� ����
        string json = JsonUtility.ToJson(sceneData);
        PlayerPrefs.SetString("SceneData_" + toDoManager.dayCount, json); // ��¥���� ����
        PlayerPrefs.Save();
    }
}





