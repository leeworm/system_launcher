using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; private set; }

    private int m_SelectedChapter;
    private int m_CurrStage;
    private const string STAGE_PATH = "Stages/";
    private Transform m_StageTrs;
    private SpriteRenderer m_Bg;

    public bool IsPaused { get; private set; }

    protected override void Init()
    {
        m_IsDestroyOnLoad = true;

        base.Init();

        InitVariables();
        LoadStage();

        UIManager.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, true);
    }

    private void InitVariables()
    {
        Logger.Log($"{GetType()}::InitVariables");

        //m_StageTrs = GameObject.Find("Stage").transform;
        //m_Bg = GameObject.Find("Bg").GetComponent<SpriteRenderer>();
        //m_CurrStage = 1;

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if (userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        //m_SelectedChapter = userPlayData.SelectedChapter;
    }

    private void LoadStage()
    {
        Logger.Log($"{GetType()}::LoadStage");
        Logger.Log($"Chapter:{m_SelectedChapter} Stage:{m_CurrStage}");

        // 자신의 게임 넣기

        // var bgTexture = Resources.Load($"ChapterBG/Background_{m_SelectedChapter.ToString("D3")}") as Texture2D;
        // if(bgTexture != null)
        // {
        //     m_Bg.sprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(0.5f, 0.5f));
        // }

        // var stageObj = Instantiate(Resources.Load($"{STAGE_PATH}C{m_SelectedChapter}/C{m_SelectedChapter}_S{m_CurrStage}", typeof(GameObject))) as GameObject;
        // stageObj.transform.SetParent(m_StageTrs);
        // stageObj.transform.localScale = Vector3.one;
        // stageObj.transform.localPosition = Vector3.zero;
    }

    private void Start()
    {
        InGameUIController = FindAnyObjectByType<InGameUIController>();
        if (!InGameUIController)
        {
            Logger.LogError("InGameUIController does not exist.");
            return;
        }

        InGameUIController.Init();
    }

    // 게임 일시정지 메서드
    public void PauseGame()
    {
        // 일시정지 상태를 true로 설정
        IsPaused = true;

        // 게임 매니저의 일시정지 플래그 설정
        // GameManager.Instance.Paused = true;
        // 레벨 매니저의 캐릭터 일시정지 토글
        // LevelManager.Instance.ToggleCharacterPause();
    }

    // 게임 재개 메서드
    public void ResumeGame()
    {
        // 일시정지 상태를 false로 설정
        IsPaused = false;

        // 게임 매니저의 일시정지 플래그 해제
        // GameManager.Instance.Paused = false;
        // 레벨 매니저의 캐릭터 일시정지 토글
        // LevelManager.Instance.ToggleCharacterPause();
    }
}
