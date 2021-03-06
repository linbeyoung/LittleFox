using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenu;  // ð è·å¾æåèå
    public AudioMixer audioMixer; // ð è·å¾ä¸»é³éæ§å¶å¨
    public Slider slider;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void UIEnable()
    {
        // GameObject.Find("Canvas/MainMenu/UI").GetComponent<Canvas>().enabled = true;  // ç³»ç»çæç
        GameObject.Find("Canvas/MainMenu/UI").SetActive(true);  // findæ¾å°ææä»¶
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);

        // å®ç°æ¸¸æç»é¢æå
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    // ç»å®æ»åçå¼

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("MainVolume", value);
    }
}
