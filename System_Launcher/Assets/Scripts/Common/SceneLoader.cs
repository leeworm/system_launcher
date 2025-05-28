using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    Lobby,
    InGame
}
public class SceneLoader : SingletonBehaviour<SceneLoader>
{
    // 지정된 씬으로 전환하는 메서드
    public void LoadScene(SceneType sceneType)
    {
        Logger.Log($"{sceneType} scene loading...");

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneType.ToString());
    }
    // 현재 씬을 다시 로드하는 메서드
    public void ReloadScene()
    {
        Logger.Log($"{SceneManager.GetActiveScene().name} scene loading...");

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
