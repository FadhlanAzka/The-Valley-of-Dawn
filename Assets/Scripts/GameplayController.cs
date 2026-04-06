using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    private static GameplayController instance;

    private int point = 0;
    private int health = 100;

    public int Point => point;
    public int Health => health;

    [SerializeField] private float timeRemaining = 60f; // Initial time remaining
    [SerializeField] private bool enableTimer;
    [SerializeField] private AudioSource coinCollectSound;
    [SerializeField] private AudioSource healSound;
    [SerializeField] private AudioSource damageSound;

    public TextMeshProUGUI pointText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI gameoverPoints;
    public TextMeshProUGUI gameMission;

    public GameObject gameover;
    public GameObject minimap;
    public GameObject pauseMenu;

    private void Awake()
    {
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        timeText.gameObject.SetActive(false);

        if (enableTimer)
        {
            StartCoroutine(StartTimer());
            timeText.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame();
        }
    }

    private IEnumerator StartTimer()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            timeRemaining--; // Decrement time remaining
            UpdateTimeUI();
        }

        // Handle game over
        Time.timeScale = 0f; // Freeze gameplay
        Camera.main.GetComponent<CameraController>().enabled = false; // Disable camera movement
        pointText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        minimap.SetActive(false);
        gameover.SetActive(true); // Activate game over screen
        gameoverPoints.text = "Points: " + point; // Display final points
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CoinRed"))
        {
            point += 20;
            UpdatePointUI();
            Debug.Log("Points: " + point);
            collectCoinsSound();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("CoinYellow"))
        {
            point += 30;
            UpdatePointUI();
            Debug.Log("Points: " + point);
            collectCoinsSound();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("CoinGreen"))
        {
            point += 100;
            UpdatePointUI();
            Debug.Log("Points: " + point);
            collectCoinsSound();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("CoinBlue"))
        {
            health += 10;
            if (health > 100)
            {
                health = 100; // Maximal health is 100
                StartCoroutine(DisplayMaxHealthText());
            }
            UpdateHealthUI();

            Debug.Log("Health: " + health);
            healPlay();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("CoinBlack"))
        {
            health -= 20;
            if (health < 0)
                health = 0; // Health cannot be negative
            UpdateHealthUI();
            Debug.Log("Health: " + health);
            damagePlay();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            health -= 20;
            if (health < 0)
                health = 0; // Health cannot be negative
            UpdateHealthUI();
            Debug.Log("Health: " + health);
            damagePlay();
        }
    }

    private void UpdatePointUI()
    {
        pointText.text = "Points: " + point;
    }

    private void UpdateHealthUI()
    {
        healthText.text = "Health: " + health;
    }

    private void UpdateTimeUI()
    {
        timeText.text = "Time: " + timeRemaining.ToString();
    }

    private IEnumerator DisplayMaxHealthText()
    {
        maxHealthText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f); // Show "Max Health!" for 0.5 seconds
        maxHealthText.gameObject.SetActive(false);
    }

    private void collectCoinsSound()
    {
        if (coinCollectSound != null)
        {
            coinCollectSound.Play();
        }
        else
        {
            Debug.LogWarning("Coin collect sound is not assigned!");
        }
    }

    private void healPlay()
    {
        if (healSound != null)
        {
            healSound.Play();
        }
        else
        {
            Debug.LogWarning("Heal sound is not assigned!");
        }
    }

    private void damagePlay()
    {
        if (damageSound != null)
        {
            damageSound.Play();
        }
        else
        {
            Debug.LogWarning("Damage sound is not assigned!");
        }
    }

    private void pauseGame()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);

            if (pauseMenu.activeSelf)
            {
                Time.timeScale = 0f;
                minimap.SetActive(false);
                pointText.gameObject.SetActive(false);
                healthText.gameObject.SetActive(false);
                timeText.gameObject.SetActive(false);
                gameMission.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Camera.main.GetComponent<CameraController>().enabled = false;
            }
            else
            {
                Time.timeScale = 1f;
                minimap.SetActive(true);
                //pointText.gameObject.SetActive(true);
                healthText.gameObject.SetActive(true);
                gameMission.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Camera.main.GetComponent<CameraController>().enabled = true;

                if (enableTimer)
                {
                    timeText.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            Debug.LogWarning("Pause menu GameObject is not assigned!");
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
        #else
                    Application.Quit();
        #endif
    }
}