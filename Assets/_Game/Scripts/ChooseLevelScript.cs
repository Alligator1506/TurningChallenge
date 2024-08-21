using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelScript : MonoBehaviour
{
    public static bool showBlockedLevelMessage;

    public Text levelText;

    public Text highScoreText;

    //public Toggle musicToggle;

    public Button playButton;

    private bool updateField;

    private Touch touch;

    private ChangeSceneScript globalData;

    private void Start()
    {
        globalData = ChangeSceneScript.Instance;
        updateField = true;
        //musicToggle.isOn = globalData.musicOn;
        //musicToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate
        //{
        //    globalData.setMusic(musicToggle.GetComponent<Toggle>().isOn);
        //});
        playButton.onClick.AddListener(delegate
        {
            globalData.ChangeScene();
        });
    }

    private void Update()
    {
        if (updateField)
        {
            updateField = false;
            levelText.text = "Level: " + globalData.currentLevel;
            int @int = PlayerPrefs.GetInt("Level" + globalData.currentLevel + "highScore");
            highScoreText.text = "High Score: " + @int;
        }
        if (showBlockedLevelMessage)
        {
            highScoreText.text = "Complete previous!";
        }
    }

    public void increaseLevel()
    {
        if (globalData.currentLevel < globalData.limitLevel)
        {
            globalData.currentLevel++;
            showBlockedLevelMessage = false;
            updateField = true;
        }
    }

    public void decreaseLevel()
    {
        if (globalData.currentLevel > 1)
        {
            globalData.currentLevel--;
            showBlockedLevelMessage = false;
            updateField = true;
        }
    }

    //public void setMusic()
    //{
    //    globalData.setMusic(musicToggle.isOn);
    //}
}
