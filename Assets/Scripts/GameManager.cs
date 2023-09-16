using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] blocks;
    public GameObject particle;
    public GameObject zone;
    public Color zoneColor;
    private Color startColor;
    public int indexPrefabs;
    public bool deployed;

    public int knightScore;
    public int rookScore;
    public int bishopScore;
    public int dragonScore;
    public int totalScore;
    public Text scoreText;
    public Text highScoreText; // Menampilkan skor tertinggi

    public int limit;
    public int scoreLimit;

    public bool pathAssist;
    public GameObject[] switchButton;

    public int knightCount;
    public int rookCount;
    public int bishopCount;
    public int dragonCount;

    public float totalTime;
    private float currentTime;
    public bool timerRunning;
    public Text timerText;
    public Image timerFill;

    public GameObject gameOverScene;
    public bool gameOver;

    public AudioSource audioSource;
    public AudioClip[] audioClips;

    public bool isWin = false;
    public Text text;

    public GameObject achievement;
    private void Start()
    {
        deployed = false;
        startColor = timerFill.color;
        currentTime = totalTime;
        timerRunning = false;
        DisplayTime(currentTime);
        totalScore = 0;
        gameOver = false;
        gameOverScene.SetActive(false);

        knightCount = 0;
        rookCount = 0;
        bishopCount = 0;
        dragonCount = 0;

        // Mengambil skor tertinggi saat game dimulai
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }

    private void Update()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if(achievement != null)
        {
            if (highScore >= 1000)
            {
                achievement.SetActive(true);
            }
            else if(highScore < 1000)
            {
                achievement.SetActive(false); 
            }
        }

        if (!gameOver)
        {
            // Generate random block after deployed
            if (!deployed)
            {
                indexPrefabs = Random.Range(0, blocks.Length);
                GameObject slot = GameObject.Find("Slot").transform.GetChild(0).gameObject;
                SpriteRenderer slotRenderer = slot.GetComponent<SpriteRenderer>();
                slotRenderer.sprite = blocks[indexPrefabs].GetComponent<SpriteRenderer>().sprite;
                deployed = true;
                currentTime = totalTime;
            }

            // Timer
            if (timerRunning)
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0)
                {
                    currentTime = 0;
                    timerRunning = false;
                    gameOver = true;
                }
                DisplayTime(currentTime);
            }
            scoreText.text = "Score: " + totalScore;

            // Check and destroy block count
            DestroyBlockWithTag("Dragon", dragonCount);
            DestroyBlockWithTag("Knight", knightCount);
            DestroyBlockWithTag("Rook", rookCount);
            DestroyBlockWithTag("Bishop", bishopCount);

            switchButton[0].SetActive(pathAssist);
            switchButton[1].SetActive(!pathAssist);

            if (totalScore >= scoreLimit)
            {
                // Tingkat kesulitan bertambah
                totalTime -= scoreLimit / scoreLimit;
                limit += scoreLimit / scoreLimit;
                Debug.Log("makin susah");

                if (audioSource != null && audioClips.Length > 2)
                {
                    audioSource.clip = audioClips[2];
                    audioSource.Play();
                }

                // Mengatur skor limit berikutnya
                int scoreMultiplier = totalScore / scoreLimit;
                scoreLimit = (scoreMultiplier + 1) * scoreLimit;
            }



            highScore = PlayerPrefs.GetInt("HighScore", 0);
            if (totalScore > highScore)
            {
                PlayerPrefs.SetInt("HighScore", totalScore);
                PlayerPrefs.Save();
                highScoreText.text = "High Score: " + totalScore;
            }

            if(totalScore >= 1000)
            {
                gameOver = true;
                isWin = true;
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        else if (gameOver)
        {
            // Menyimpan skor tertinggi jika skor saat ini melebihi skor tertinggi sebelumnya

            gameOverScene.SetActive(true);
            currentTime = 0;
            timerRunning = false;
            DisplayTime(currentTime);
            if(isWin)
            {
                text.fontSize = 42;
                text.text = "YOU WIN THE GAME !! CRAZY GUY";
            }
            else if(!isWin)
            {
                text.fontSize = 86;
                text.text = "YOU LOSE";
            }
        }


    }

    private void DestroyBlockWithTag(string tag, int count)
    {
        if (count >= limit)
        {
            var blocks = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject block in blocks)
            {
                Instantiate(particle, block.transform.position, Quaternion.identity);
                audioSource.clip = audioClips[1];
                audioSource.Play();
                Destroy(block);
            }
            switch (tag)
            {
                case "Dragon":
                    dragonCount = 0;
                    break;
                case "Knight":
                    knightCount = 0;
                    break;
                case "Rook":
                    rookCount = 0;
                    break;
                case "Bishop":
                    bishopCount = 0;
                    break;
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.RoundToInt(timeToDisplay / 60f);
        float seconds = Mathf.RoundToInt(timeToDisplay % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        float fillAmount = currentTime / totalTime;
        timerFill.fillAmount = fillAmount;

        if (currentTime <= 3)
            timerFill.color = Color.red;
        else
            timerFill.color = startColor;
    }

    public void RetryScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Switch()
    {
        pathAssist = !pathAssist;
    }
}
