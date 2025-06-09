// 코루틴 및 컬렉션 관련 네임스페이스 사용
using System.Collections;
// 제네릭 컬렉션 관련 네임스페이스 사용
using System.Collections.Generic;
// Unity 엔진 관련 네임스페이스 사용
using UnityEngine;

// MonoBehaviour를 상속받는 인게임 UI 컨트롤러 클래스 정의
public class InGameUIController : MonoBehaviour
{
    // 초기화 메서드 (현재 비어있음)
    public void Init()
    {

    }

    // 애플리케이션 포커스 변경 시 호출되는 Unity 이벤트 메서드
    private void OnApplicationFocus(bool focus)
    {
        // 앱이 포커스를 잃은 경우 (백그라운드로 이동)
        if (!focus)
        {
            // 현재 게임이 일시정지 상태가 아닌 경우
            if (!InGameManager.Instance.IsPaused)
            {
                // 기본 UI 데이터 생성
                var uiData = new BaseUIData();
                // 일시정지 UI 열기
                UIManager.Instance.OpenUI<PauseUI>(uiData);

                // 게임 일시정지 실행
                InGameManager.Instance.PauseGame();
            }
        }
    }

    // Unity의 Update 라이프사이클 메서드
    private void Update()
    {
        // 게임이 일시정지 상태가 아닌 경우
        if (!InGameManager.Instance.IsPaused)
        {
            // 입력 처리 메서드 호출
            HandleInput();
        }
    }

    // 입력 처리를 담당하는 메서드
    private void HandleInput()
    {
        // ESC 키를 뗀 경우
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // UI 버튼 클릭 사운드 재생
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            // 기본 UI 데이터 생성
            var uiData = new BaseUIData();
            // 일시정지 UI 열기
            UIManager.Instance.OpenUI<PauseUI>(uiData);

            // 게임 일시정지 실행
            InGameManager.Instance.PauseGame();
        }
    }

    // 일시정지 버튼 클릭 시 호출되는 메서드
    public void OnClickPauseBtn()
    {
        // UI 버튼 클릭 사운드 재생
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        // 기본 UI 데이터 생성
        var uiData = new BaseUIData();
        // 일시정지 UI 열기
        UIManager.Instance.OpenUI<PauseUI>(uiData);

        // 게임 일시정지 실행
        InGameManager.Instance.PauseGame();
    }
}