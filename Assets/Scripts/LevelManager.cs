using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class LevelManager : MonoBehaviour
{
	private int currentLevel;

	private List<string> scenesInBuild;

	void Start()
	{
		scenesInBuild = new List<string>();
		for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
			int lastSlash = scenePath.LastIndexOf("/");
			scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
		}

        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void StartFirstLevel()
	{
		currentLevel = 0;
        PlayerPrefs.SetInt("currentLevel", currentLevel);
		LoadNextLevel();
	}

	public void LoadNextLevel()
	{
		currentLevel++;
        PlayerPrefs.SetInt("currentLevel", currentLevel);
        SceneManager.LoadScene("Level_"+currentLevel);
	}

	public bool IsItLastLevel()
	{
		return !(scenesInBuild.Contains ("Level_" + (currentLevel + 1)));
	}
}