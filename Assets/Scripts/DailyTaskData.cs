using System.Collections.Generic;
using System;

[Serializable]
public class DailyTaskData
{
    public string date; // ��ϵ� ��¥ (yyyy-MM-dd ����)
    public List<TaskData> tasks; // �ش� ��¥�� Task ����Ʈ
}

[Serializable]
public class DailyTaskCollection
{
    public List<DailyTaskData> dailyTasks = new List<DailyTaskData>();
}
