using System.Collections.Generic;
using UnityEngine;

// BGM 타입 열거형
public enum BGM
{
    lobby, // 로비 BGM
    COUNT // 총 BGM 개수
}

// SFX 타입 열거형
public enum SFX
{
    chapter_clear, // 챕터 클리어 효과음
    stage_clear, // 스테이지 클리어 효과음
    ui_button_click, // UI 버튼 클릭 효과음
    COUNT // 총 SFX 개수
}

// 오디오를 관리하는 싱글톤 매니저 클래스
public class AudioManager : SingletonBehaviour<AudioManager>
{
    public Transform BGMTrs; // BGM 오디오 소스들의 부모 트랜스폼
    public Transform SFXTrs; // SFX 오디오 소스들의 부모 트랜스폼

    private const string AUDIO_PATH = "Audio"; // 오디오 리소스 파일 경로

    private Dictionary<BGM, AudioSource> m_BGMPlayer = new Dictionary<BGM, AudioSource>(); // BGM 플레이어 딕셔너리
    private AudioSource m_CurrBGMSource; // 현재 재생 중인 BGM 소스

    private Dictionary<SFX, AudioSource> m_SFXPlayer = new Dictionary<SFX, AudioSource>(); // SFX 플레이어 딕셔너리

    // 초기화 함수
    protected override void Init()
    {
        base.Init(); // 부모 클래스 초기화 호출

        LoadBGMPlayer(); // BGM 플레이어 로드
        LoadSFXPlayer(); // SFX 플레이어 로드
    }

    // BGM 플레이어들을 로드하는 함수
    private void LoadBGMPlayer()
    {
        for (int i = 0; i < (int)BGM.COUNT; i++) // 모든 BGM 타입에 대해 반복
        {
            var audioName = ((BGM)i).ToString(); // BGM 이름 가져오기
            var pathStr = $"{AUDIO_PATH}/{audioName}"; // 오디오 파일 경로 생성
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip; // 오디오 클립 로드
            if (!audioClip) // 오디오 클립이 없는 경우
            {
                Logger.LogError($"{audioName} clip does not exist."); // 에러 로그 출력
                continue; // 다음 루프로 건너뛰기
            }

            var newGO = new GameObject(audioName); // 새 게임 오브젝트 생성
            var newAudioSource = newGO.AddComponent<AudioSource>(); // 오디오 소스 컴포넌트 추가
            newAudioSource.clip = audioClip; // 오디오 클립 설정
            newAudioSource.loop = true; // 반복 재생 설정
            newAudioSource.playOnAwake = false; // 자동 재생 비활성화
            newGO.transform.parent = BGMTrs; // 부모 트랜스폼 설정

            m_BGMPlayer[(BGM)i] = newAudioSource; // 딕셔너리에 오디오 소스 추가
        }
    }

    // SFX 플레이어들을 로드하는 함수
    private void LoadSFXPlayer()
    {
        for (int i = 0; i < (int)SFX.COUNT; i++) // 모든 SFX 타입에 대해 반복
        {
            var audioName = ((SFX)i).ToString(); // SFX 이름 가져오기
            var pathStr = $"{AUDIO_PATH}/{audioName}"; // 오디오 파일 경로 생성
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip; // 오디오 클립 로드
            if (!audioClip) // 오디오 클립이 없는 경우
            {
                Logger.LogError($"{audioName} clip does not exist."); // 에러 로그 출력
                continue; // 다음 루프로 건너뛰기
            }

            var newGO = new GameObject(audioName); // 새 게임 오브젝트 생성
            var newAudioSource = newGO.AddComponent<AudioSource>(); // 오디오 소스 컴포넌트 추가
            newAudioSource.clip = audioClip; // 오디오 클립 설정
            newAudioSource.loop = false; // 반복 재생 비활성화
            newAudioSource.playOnAwake = false; // 자동 재생 비활성화
            newGO.transform.parent = SFXTrs; // 부모 트랜스폼 설정

            m_SFXPlayer[(SFX)i] = newAudioSource; // 딕셔너리에 오디오 소스 추가
        }
    }



    public void OnLoadUserData()
    {
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        if (userSettingsData != null)
        {
            if (!userSettingsData.Sound)
            {
                Mute();
            }
        }
    }






    // BGM 재생 함수
    public void PlayBGM(BGM bgm)
    {
        if (m_CurrBGMSource) // 현재 재생 중인 BGM이 있는 경우
        {
            m_CurrBGMSource.Stop(); // 현재 BGM 정지
            m_CurrBGMSource = null; // 현재 BGM 소스 초기화
        }

        if (!m_BGMPlayer.ContainsKey(bgm)) // 요청한 BGM이 딕셔너리에 없는 경우
        {
            Logger.LogError($"Invalid clip name. {bgm}"); // 에러 로그 출력
            return; // 함수 종료
        }

        m_CurrBGMSource = m_BGMPlayer[bgm]; // 새 BGM 소스 설정
        m_CurrBGMSource.Play(); // BGM 재생 시작
    }

    // BGM 일시정지 함수
    public void PauseBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Pause(); // 현재 BGM이 있으면 일시정지
    }

    // BGM 재생 재개 함수
    public void ResumeBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.UnPause(); // 현재 BGM이 있으면 재생 재개
    }

    // BGM 정지 함수
    public void StopBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Stop(); // 현재 BGM이 있으면 정지
    }

    // SFX 재생 함수
    public void PlaySFX(SFX sfx)
    {
        if (!m_SFXPlayer.ContainsKey(sfx)) // 요청한 SFX가 딕셔너리에 없는 경우
        {
            Logger.LogError($"Invalid clip name. ({sfx})"); // 에러 로그 출력
            return; // 함수 종료
        }

        m_SFXPlayer[sfx].Play(); // SFX 재생
    }

    // 모든 오디오 음소거 함수
    public void Mute()
    {
        foreach (var audioSourceItem in m_BGMPlayer) // 모든 BGM 플레이어에 대해
        {
            audioSourceItem.Value.volume = 0f; // 볼륨을 0으로 설정
        }

        foreach (var audioSourceItem in m_SFXPlayer) // 모든 SFX 플레이어에 대해
        {
            audioSourceItem.Value.volume = 0f; // 볼륨을 0으로 설정
        }
    }

    // 모든 오디오 음소거 해제 함수
    public void UnMute()
    {
        foreach (var audioSourceItem in m_BGMPlayer) // 모든 BGM 플레이어에 대해
        {
            audioSourceItem.Value.volume = 1f; // 볼륨을 1로 설정
        }

        foreach (var audioSourceItem in m_SFXPlayer) // 모든 SFX 플레이어에 대해
        {
            audioSourceItem.Value.volume = 1f; // 볼륨을 1로 설정
        }
    }
}
