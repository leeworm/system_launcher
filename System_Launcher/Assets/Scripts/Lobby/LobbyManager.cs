// System.Collections 네임스페이스 사용
using System.Collections;
// System.Collections.Generic 네임스페이스 사용
using System.Collections.Generic;
// UnityEngine 네임스페이스 사용
using UnityEngine;

// 싱글톤 패턴을 상속받는 로비 관리자 클래스
public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    // 로비 UI 컨트롤러 프로퍼티 (읽기 전용)
    public LobbyUIController LobbyUIController { get; private set; }

    // 초기화 메서드 오버라이드
    protected override void Init()
    {
        // 씬 로드 시 파괴되도록 설정
        m_IsDestroyOnLoad = true;

        // 부모 클래스의 초기화 호출
        base.Init();
    }

    // Start 메서드 (게임 시작 시 호출)
    private void Start()
    {
        // 씬에서 LobbyUIController 찾아서 할당
        LobbyUIController = FindAnyObjectByType<LobbyUIController>();
        // LobbyUIController가 없는 경우
        if (!LobbyUIController)
        {
            // 에러 로그 출력
            Logger.LogError("LobbyUIController does not exist.");
            // 메서드 종료
            return;
        }

        // LobbyUIController 초기화
        LobbyUIController.Init();
        // 로비 배경음악 재생
        AudioManager.Instance.PlayBGM(BGM.lobby);
    }
}