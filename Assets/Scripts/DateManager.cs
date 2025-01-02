using System;
using UnityEngine;

public class DateManager : MonoBehaviour
{
    public static DateManager Instance;
    public string selectedDate = DateTime.Now.ToString("yyyy-MM-dd");
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
