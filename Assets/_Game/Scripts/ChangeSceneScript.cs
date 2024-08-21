using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour
{
    public int currentLevel;

    public int defaultLimitLevel = 7;

    public int limitLevel;

    public bool musicOn;

    private static ChangeSceneScript GlobalData;

    public static ChangeSceneScript Instance => GlobalData;

    private void Awake()
    {
        if (GlobalData != null && GlobalData != this)
        {
            Object.Destroy(base.gameObject);
        }
        GlobalData = this;
        //Object.DontDestroyOnLoad(base.gameObject);
        musicOn = PlayerPrefs.GetInt("GameMusic", 1) == 1;
        limitLevel = PlayerPrefs.GetInt("LimitLevel", defaultLimitLevel);
        currentLevel = PlayerPrefs.GetInt("CurrentGameLevel", 1);
    }

    //public void setMusic(bool value)
    //{
    //    Debug.Log("setMusic(" + value + "), Saved");
    //    PlayerPrefs.SetInt("GameMusic", value ? 1 : 0);
    //    PlayerPrefs.Save();
    //    musicOn = value;
    //}

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void ChangeScene()
    {
        if (currentLevel > 1 && PlayerPrefs.GetInt("Level" + (currentLevel - 1) + "highScore") <= 0)
        {
            ChooseLevelScript.showBlockedLevelMessage = true;
        }
        else
        {
            ChangeScene("Level" + currentLevel);
        }
    }

    public void ChangeScene(int level)
    {
        ChangeScene("Level" + level);
    }

    public void ChangeScene(string scene)
    {
        PlayerPrefs.SetInt("CurrentGameLevel", currentLevel);
        PlayerPrefs.Save();
        SceneManager.LoadScene(scene);
    }
}
