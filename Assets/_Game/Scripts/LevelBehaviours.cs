using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBehaviours : MonoBehaviour
{
	private static ChangeSceneScript globalData;

	public Button nextLevelButton;

	public void restartLevel()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
		Object.Destroy(this);
	}

	public void nextLevel()
	{
		Time.timeScale = 1f;
		int num = globalData.currentLevel + 1;
		if (num <= globalData.limitLevel)
		{
			globalData.currentLevel = num;
			SceneManager.LoadScene("Level" + num);
			Object.Destroy(this);
		}
	}

	public void loadMainMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
		Object.Destroy(this);
	}

	public void Start()
	{
		globalData = ChangeSceneScript.Instance;
		nextLevelButton.interactable = enableNextLevelButton();
	}

	public static bool enableNextLevelButton()
	{
		int @int = PlayerPrefs.GetInt("Level" + globalData.currentLevel + "highScore");
		if (globalData.currentLevel != globalData.limitLevel)
		{
			return @int > 0;
		}
		return false;
	}
}
