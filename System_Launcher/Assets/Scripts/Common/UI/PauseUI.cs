using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : BaseUI
{
    public void OnClickResume()
    {
        InGameManager.Instance.ResumeGame();

        CloseUI();
    }

    public void OnClickHome()
    {
        SceneLoader.Instance.LoadScene(SceneType.Lobby);

        CloseUI();
    }
}