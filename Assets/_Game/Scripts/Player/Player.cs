using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum GameState
    {
        Loaded = 0,
        Playing = 1,
        Over = 2
    }

    private ChangeSceneScript globalData;

    private bool tabScreen;

    private Touch touch;

    public Text stepsLeftBarText;

    public Slider stepsLeftSlider;

    public Text diamondsLeftText;

    public Text gameOverText;

    public Text currentScoreText;

    public Text highScoreText;

    public Text currentLevelText;

    public float speed;

    public int stepsLeft;

    public float slowdownDelta = 0.03f;

    public float slowdownMinimal = 0.1f;

    public GameObject playmodeCanvas;

    public GameObject gameOverCanvas;

    //public AudioSource audioSource;

    public Button nextLevelButton;

    //public Animator playerAnimator;

    public bool resetHighScore;

    private int takenDiamonds;

    private int availableDiamonds;

    private GameState gameState;

    private bool updateCanvas = true;

    private bool isColliding;

    private bool gameWon;

    private int direction = 1;

    private Vector3[] arrows = new Vector3[4]
    {
        Vector3.left,
        Vector3.forward,
        Vector3.right,
        Vector3.back
    };

    private int[] speedDeltas = new int[4];

    private int arrowsLength;

    private int speedDeltasLength = 4;

    private StringBuilder sb;

    private void Start()
    {
        gameState = GameState.Loaded;
        globalData = ChangeSceneScript.Instance;
        stepsLeftSlider.minValue = 0f;
        stepsLeftSlider.maxValue = stepsLeft;
        //AudioListener.pause = false;
        availableDiamonds = GameObject.FindGameObjectsWithTag("Coin").Length;
        currentLevelText.text = $"Level: {globalData.currentLevel}";
        //audioSource.mute = !globalData.musicOn;
        switch (globalData.currentLevel)
        {
            case 3:
                arrows = new Vector3[4]
                {
                Vector3.left,
                Vector3.back,
                Vector3.right,
                Vector3.forward
                };
                break;
            case 4:
                speedDeltas = new int[4] { 0, 0, 4, 0 };
                break;
            case 5:
                arrows = new Vector3[4]
                {
                Vector3.forward,
                Vector3.back,
                Vector3.left,
                Vector3.right
                };
                break;
            case 6:
                speedDeltas = new int[4] { 4, 0, 4, 0 };
                break;
            case 7:
                arrows = new Vector3[4]
                {
                Vector3.forward,
                Vector3.back,
                Vector3.forward,
                Vector3.right
                };
                break;
        }
        arrowsLength = arrows.Length;
    }

    private void Update()
    {
        if (gameState == GameState.Over)
        {
            return;
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Ended && touch.tapCount >= 1)
            {
                tabScreen = true;
            }
        }
        printCanvas();
        if (gameState != 0)
        {
            base.transform.Translate(arrows[direction % arrowsLength] * (speed + (float)speedDeltas[direction % speedDeltasLength]) * Time.deltaTime);
        }
        switch (gameState)
        {
            case GameState.Loaded:
            case GameState.Playing:
                if (Input.GetKeyDown("space") || tabScreen)
                {
                    gameState = GameState.Playing;
                    direction++;
                    consumeStep();
                    turnPlayer();
                    tabScreen = false;
                }
                if (stepsLeft <= 0)
                {
                    endGame();
                }
                if (takenDiamonds == availableDiamonds)
                {
                    gameWon = true;
                    endGame();
                }
                isColliding = false;
                break;
            case GameState.Over:
                if (Time.timeScale > slowdownMinimal || Time.timeScale > 0f)
                {
                    Time.timeScale = Mathf.Abs(Time.timeScale - slowdownDelta);
                }
                else
                {
                    Time.timeScale = slowdownMinimal;
                }
                break;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isColliding)
        {
            return;
        }
        isColliding = true;
        if (other.gameObject.CompareTag("Coin"))
        {
            base.gameObject.GetComponent<ParticleSystem>().Play();
            if (!other.gameObject.GetComponent<DiamondScript>().WasTaken())
            {
                other.gameObject.GetComponent<DiamondScript>().TakeThis();
                takenDiamonds++;
            }
            updateCanvas = true;
        }
        else if (other.gameObject.CompareTag("Terminator"))
        {
            endGame();
        }
    }

    private void printCanvas()
    {
        if (updateCanvas)
        {
            stepsLeftBarText.text = stepsLeft.ToString();
            stepsLeftSlider.value = stepsLeft;
            diamondsLeftText.text = takenDiamonds + " / " + availableDiamonds;
            updateCanvas = false;
        }
    }

    private void consumeStep()
    {
        updateCanvas = true;
        if (stepsLeft > 0)
        {
            stepsLeft--;
        }
    }

    private void endGame()
    {
        if (gameState != GameState.Over)
        {
            gameState = GameState.Over;
            transform.Translate(Vector3.zero);
            int score = stepsLeft * takenDiamonds;
            currentScoreText.text = "Score: " + score;
            if (gameWon)
            {
                gameWon = true;
                gameOverText.text = "You Won!";
            }
            else
            {
                gameOverText.text = "You Lost";
            }
            showScores(score);
            sendMetrics(ChangeSceneScript.Instance.currentLevel, score, gameWon);
            nextLevelButton.interactable = LevelBehaviours.enableNextLevelButton();
            playmodeCanvas.SetActive(value: false);
            gameOverCanvas.SetActive(value: true);
        }
    }

    private void showScores(int score)
    {
        int num = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "highScore");
        bool flag = false;
        if (gameWon && score > num)
        {
            flag = true;
            num = score;
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "highScore", score);
            PlayerPrefs.Save();
        }
        currentScoreText.text = "Score: " + score;
        highScoreText.text = "High Score: " + num;
        if (flag)
        {
            highScoreText.text += " New!";
        }
        if (resetHighScore)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "highScore", 0);
            PlayerPrefs.Save();
        }
    }

    private void sendMetrics(int level, int score, bool win)
    {
        Analytics.CustomEvent("gameOver", new Dictionary<string, object>
        {
            { "level", level },
            { "score", score },
            { "win", win }
        });
    }

    public void ShowAd()
    {
    }

    private void turnPlayer()
    {
    }
}
