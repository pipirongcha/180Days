using System.Collections.Generic;
using System;

[Serializable]
public class DailyTaskData
{
    public string date; // 기록된 날짜 (yyyy-MM-dd 형식)
    public List<TaskData> tasks; // 해당 날짜의 Task 리스트
}

[Serializable]
public class DailyTaskCollection
{
    public List<DailyTaskData> dailyTasks = new List<DailyTaskData>();
}
