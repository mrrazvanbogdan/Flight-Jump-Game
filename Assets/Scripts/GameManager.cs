using UnityEditor;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI topScoreText;
    public GameObject playButton;
    public GameObject replayButton;
    public GameObject quitButton;
    public GameObject gameOver;
    public GameObject logo;

    private int score;
    private int topScore;

    [SerializeField] private AudioClip scoreSound;
    private AudioSource audioSource;

    public void Awake()
    {
        Application.targetFrameRate = 60;

        audioSource = GetComponent<AudioSource>();

        topScore = PlayerPrefs.GetInt("TopScore", 0);
        UpdateScoreText();

        gameOver.SetActive(false);
        logo.SetActive(true);
        playButton.SetActive(true);
        replayButton.SetActive(false);
        quitButton.SetActive(false);

        Time.timeScale = 0f;
        player.enabled = false;

        Pause();
    }

    public void Play()
    {
        score = 0;
        UpdateScoreText();
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        replayButton.SetActive(false);
        quitButton.SetActive(false);
        gameOver.SetActive(false);
        logo.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

        Pipes[] pipes = FindObjectsByType<Pipes>(FindObjectsSortMode.None);

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }
    
    public void GameOver()
    {
        gameOver.SetActive(true);
        logo.SetActive(false);
        playButton.SetActive(false);
        replayButton.SetActive(true);
        quitButton.SetActive(true);

        if (score > topScore)
        {
            topScore = score;
            PlayerPrefs.SetInt("TopScore", topScore);
            PlayerPrefs.Save();
        }

        UpdateScoreText();
        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        UpdateScoreText();
        scoreText.text = score.ToString();

        if (scoreSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(scoreSound);
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
        topScoreText.text = "Top Score: " + topScore;
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game"); // Apare doar în editor
        Application.Quit(); // Închide aplicația în build-ul final
    }

}
