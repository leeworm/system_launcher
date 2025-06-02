// TextMeshPro UI 컴포넌트를 사용하기 위한 네임스페이스
using TMPro;
// Unity 엔진의 기본 기능을 사용하기 위한 네임스페이스
using UnityEngine;

// 설정 UI를 관리하는 클래스, BaseUI를 상속받음
public class SettingsUI : BaseUI
{
    // 게임 버전을 표시할 텍스트 컴포넌트
    public TextMeshProUGUI GameVersionTxt;
    // 사운드 켜짐 상태를 나타내는 토글 오브젝트
    public GameObject SoundOnToggle;
    // 사운드 꺼짐 상태를 나타내는 토글 오브젝트
    public GameObject SoundOffToggle;

    // 개인정보처리방침 URL 상수
    private const string PRIVACY_POLICY_URL = "https://likelion.net/";

    // UI 정보를 설정하는 메서드 오버라이드
    public override void SetInfo(BaseUIData uiData)
    {
        // 부모 클래스의 SetInfo 메서드 호출
        base.SetInfo(uiData);

        // 게임 버전 설정
        SetGameVersion();

        // 사용자 설정 데이터를 가져옴
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        // 사용자 설정 데이터가 존재하는지 확인
        if (userSettingsData != null)
        {
            // 사운드 설정을 UI에 반영
            SetSoundSetting(userSettingsData.Sound);
        }
    }

    // 게임 버전을 설정하는 메서드
    private void SetGameVersion()
    {
        // 애플리케이션 버전을 텍스트에 설정
        GameVersionTxt.text = $"Version:{Application.version}";
    }

    // 사운드 설정 상태를 UI에 반영하는 메서드
    private void SetSoundSetting(bool sound)
    {
        // 사운드 켜짐 토글의 활성화 상태 설정
        SoundOnToggle.SetActive(sound);
        // 사운드 꺼짐 토글의 활성화 상태 설정 (반대)
        SoundOffToggle.SetActive(!sound);
    }

    // 사운드 켜짐 토글 클릭 시 호출되는 메서드
    public void OnClickSoundOnToggle()
    {
        // 로그 출력
        Logger.Log($"{GetType()}::OnClickSoundOnToggle");

        // 버튼 클릭 효과음 재생
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        // 사용자 설정 데이터를 가져옴
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        // 사용자 설정 데이터가 존재하는지 확인
        if (userSettingsData != null)
        {
            // 사운드를 꺼짐으로 설정
            userSettingsData.Sound = false;
            // 사용자 데이터 저장
            UserDataManager.Instance.SaveUserData();
            // 오디오 음소거
            AudioManager.Instance.Mute();
            // UI에 사운드 설정 반영
            SetSoundSetting(userSettingsData.Sound);
        }
    }

    // 사운드 꺼짐 토글 클릭 시 호출되는 메서드
    public void OnClickSoundOffToggle()
    {
        // 로그 출력
        Logger.Log($"{GetType()}::OnClickSoundOffToggle");

        // 버튼 클릭 효과음 재생
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        // 사용자 설정 데이터를 가져옴
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        // 사용자 설정 데이터가 존재하는지 확인
        if (userSettingsData != null)
        {
            // 사운드를 켜짐으로 설정
            userSettingsData.Sound = true;
            // 사용자 데이터 저장
            UserDataManager.Instance.SaveUserData();
            // 오디오 음소거 해제
            AudioManager.Instance.UnMute();
            // UI에 사운드 설정 반영
            SetSoundSetting(userSettingsData.Sound);
        }
    }

    // 개인정보처리방침 버튼 클릭 시 호출되는 메서드
    public void OnClickPrivacyPolicyBtn()
    {
        // 로그 출력
        Logger.Log($"{GetType()}::OnClickPrivacyPolicyBtn");

        // 버튼 클릭 효과음 재생
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        // 개인정보처리방침 URL을 브라우저에서 열기
        Application.OpenURL(PRIVACY_POLICY_URL);
    }
}