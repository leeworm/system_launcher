// 코루틴 및 컬렉션 관련 네임스페이스 사용
using System.Collections;
// 제네릭 컬렉션 관련 네임스페이스 사용
using System.Collections.Generic;
// Unity 엔진 관련 네임스페이스 사용
using UnityEngine;

// 싱글톤 패턴을 상속받는 로비 매니저 클래스 정의
public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    // 로비 UI 컨트롤러에 대한 읽기 전용 프로퍼티
    public LobbyUIController LobbyUIController { get; private set; }

    // 인게임 로딩 중 여부를 나타내는 플래그 변수
    private bool m_IsLoadingInGame;

    // 초기화 메서드 오버라이드
    protected override void Init()
    {
        // 씬 로드 시 오브젝트 파괴하도록 설정
        m_IsDestroyOnLoad = true;
        // 인게임 로딩 플래그를 false로 초기화
        m_IsLoadingInGame = false;

        // 부모 클래스의 Init 메서드 호출
        base.Init();
    }

    // Unity의 Start 라이프사이클 메서드
    private void Start()
    {
        // 씬에서 LobbyUIController 컴포넌트 찾기
        LobbyUIController = FindAnyObjectByType<LobbyUIController>();
        // LobbyUIController가 존재하지 않는 경우
        if (!LobbyUIController)
        {
            // 에러 메시지 로그 출력
            Logger.Log("LobbyUIController does not exist.");
            // 메서드 실행 종료
            return;
        }

        // LobbyUIController 초기화 실행
        LobbyUIController.Init();
        // 로비 배경음악 재생
        AudioManager.Instance.PlayBGM(BGM.lobby);
    }

    // 인게임 시작 메서드
    public void StartInGame()
    {
        // 이미 인게임 로딩 중인 경우
        if (m_IsLoadingInGame)
        {
            // 메서드 실행 종료 (중복 실행 방지)
            return;
        }

        // 인게임 로딩 플래그를 true로 설정
        m_IsLoadingInGame = true;

        // 검은색 페이드 인 효과 실행 (0.5초 동안)
        UIManager.Instance.Fade(Color.black, 0f, 1f, 0.5f, 0f, false, () =>
        {
            // 열려있는 모든 UI 창 닫기
            UIManager.Instance.CloseAllOpenUI();
            // 인게임 씬으로 로드
            SceneLoader.Instance.LoadScene(SceneType.InGame);
        });
    }
}