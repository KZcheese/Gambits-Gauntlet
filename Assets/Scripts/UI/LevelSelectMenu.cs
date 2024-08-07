using System;
using UI;
using UnityEngine;

public class LevelSelectMenu : Menu
{
    public string[] levelNames;
    public GameObject[] levels;
    private string _currLevel;

    private void Start()
    {
        _currLevel = PlayerPrefs.GetString("CurrentLevel", levelNames[0]);

        int currLevelIdx = Array.IndexOf(levelNames, _currLevel);
        for (int i = 0; i < levels.Length; i++)
            levels[i].SetActive(i <= currLevelIdx);
    }
}