using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenu;  // 📒 获得暂停菜单
    public AudioMixer audioMixer; // 📒 获得主音量控制器
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
        // GameObject.Find("Canvas/MainMenu/UI").GetComponent<Canvas>().enabled = true;  // 系统生成的
        GameObject.Find("Canvas/MainMenu/UI").SetActive(true);  // find找到某文件
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);

        // 实现游戏画面暂停
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    // 绑定滑块的值

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("MainVolume", value);
    }
}
