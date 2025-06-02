using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class UIManager : SingletonBehaviour<UIManager> // 싱글톤 패턴을 상속받는 UI 관리자 클래스
{
    private Camera uiCamera;
    public Transform UICanvasTrs; // UI 캔버스 Transform
    public Transform ClosedUITrs; // 닫힌 UI를 보관할 Transform

    private BaseUI m_FrontUI; // 현재 가장 앞에 있는 UI
    private Dictionary<System.Type, GameObject> m_OpenUIPool = new Dictionary<System.Type, GameObject>(); // 열린 UI들을 저장하는 딕셔너리
    private Dictionary<System.Type, GameObject> m_ClosedUIPool = new Dictionary<System.Type, GameObject>(); // 닫힌 UI들을 저장하는 딕셔너리
    
    // 재화 UI 컴포넌트
    private GoodsUI m_StatsUI;


    protected override void Init()
    {
        base.Init();

        if (uiCamera == null)
        {
            uiCamera = GameObject.Find("UICamera")?.GetComponent<Camera>();
            if (uiCamera == null)
                Debug.LogError("[UIManager] UICamera not found!");
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        // 씬에서 GoodsUI 컴포넌트 찾기
        m_StatsUI = FindAnyObjectByType<GoodsUI>();
        // GoodsUI를 찾지 못했을 때
        if (!m_StatsUI)
        {
            // 로그 출력
            Logger.Log("No stats ui component found.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
            return;

        var mainCamData = mainCamera.GetUniversalAdditionalCameraData();

        if (!mainCamData.cameraStack.Contains(uiCamera))
            mainCamData.cameraStack.Add(uiCamera);

    }

    private BaseUI GetUI<T>(out bool isAlreadyOpen) // 제네릭 타입으로 UI를 가져오는 메서드
    {
        System.Type uiType = typeof(T); // 제네릭 타입을 System.Type으로 변환

        BaseUI ui = null; // UI 컴포넌트 변수 초기화
        isAlreadyOpen = false; // 이미 열려있는지 여부 초기화

        if (m_OpenUIPool.ContainsKey(uiType)) // 열린 UI 풀에 해당 타입이 있는지 확인
        {
            ui = m_OpenUIPool[uiType].GetComponent<BaseUI>(); // 열린 UI 풀에서 UI 컴포넌트 가져오기
            isAlreadyOpen = true; // 이미 열려있음으로 설정
        }
        else if (m_ClosedUIPool.ContainsKey(uiType)) // 닫힌 UI 풀에 해당 타입이 있는지 확인
        {
            ui = m_ClosedUIPool[uiType].GetComponent<BaseUI>(); // 닫힌 UI 풀에서 UI 컴포넌트 가져오기
            m_ClosedUIPool.Remove(uiType); // 닫힌 UI 풀에서 해당 타입 제거
        }
        else // 어느 풀에도 없는 경우
        {
            var uiObj = Instantiate(Resources.Load($"UI/{uiType}", typeof(GameObject))) as GameObject; // 리소스에서 UI 프리팹 로드 및 인스턴스화
            ui = uiObj.GetComponent<BaseUI>(); // 생성된 오브젝트에서 BaseUI 컴포넌트 가져오기
        }

        return ui; // UI 컴포넌트 반환
    }


    public void OpenUI<T>(BaseUIData uiData) // 제네릭 타입으로 UI를 여는 메서드
    {
        System.Type uiType = typeof(T); // 제네릭 타입을 System.Type으로 변환

        Logger.Log($"{GetType()}::OpenUI({uiType})"); // UI 열기 로그 출력

        bool isAlreadyOpen = false; // 이미 열려있는지 여부 변수
        var ui = GetUI<T>(out isAlreadyOpen); // UI 가져오기

        if (!ui) // UI가 없는 경우
        {
            Logger.LogError($"{uiType} does not exist."); // 에러 로그 출력
            return; // 메서드 종료
        }

        if (isAlreadyOpen) // 이미 열려있는 경우
        {
            Logger.LogError($"{uiType} is already open."); // 에러 로그 출력
            return; // 메서드 종료
        }

        var siblingIdx = UICanvasTrs.childCount -1; // UI 캔버스의 자식 개수를 형제 인덱스로 설정
        ui.Init(UICanvasTrs); // UI 초기화
        ui.transform.SetSiblingIndex(siblingIdx); // UI의 형제 인덱스 설정
        ui.gameObject.SetActive(true); // UI 게임오브젝트 활성화
        ui.SetInfo(uiData); // UI 정보 설정
        ui.ShowUI(); // UI 표시

        m_FrontUI = ui; // 현재 UI를 가장 앞의 UI로 설정
        m_OpenUIPool[uiType] = ui.gameObject; // 열린 UI 풀에 UI 추가
    }

    public void CloseUI(BaseUI ui) // UI를 닫는 메서드
    {
        System.Type uiType = ui.GetType(); // UI의 타입 가져오기

        Logger.Log($"CloseUI UI:{uiType}"); // UI 닫기 로그 출력

        ui.gameObject.SetActive(false); // UI 게임오브젝트 비활성화
        m_OpenUIPool.Remove(uiType); // 열린 UI 풀에서 해당 타입 제거
        m_ClosedUIPool[uiType] = ui.gameObject; // 닫힌 UI 풀에 UI 추가
        ui.transform.SetParent(ClosedUITrs); // UI의 부모를 닫힌 UI Transform으로 설정

        m_FrontUI = null; // 가장 앞의 UI 초기화
        var lastChild = UICanvasTrs.GetChild(UICanvasTrs.childCount - 1); // UI 캔버스의 마지막 자식 가져오기
        if (lastChild) // 마지막 자식이 있는 경우
        {
            m_FrontUI = lastChild.gameObject.GetComponent<BaseUI>(); // 마지막 자식을 가장 앞의 UI로 설정
        }
    }

    public BaseUI GetActiveUI<T>() // 제네릭 타입으로 활성 UI를 가져오는 메서드
    {
        var uiType = typeof(T); // 제네릭 타입을 System.Type으로 변환
        return m_OpenUIPool.ContainsKey(uiType) ? m_OpenUIPool[uiType].GetComponent<BaseUI>() : null; 
        // 열린 UI 풀에 있으면 UI 컴포넌트 반환, 없으면 null 반환
    }

    public bool ExistsOpenUI() // 열린 UI가 존재하는지 확인하는 메서드
    {
        return m_FrontUI != null; // 가장 앞의 UI가 null이 아니면 true 반환
    }

    public BaseUI GetCurrentFrontUI() // 현재 가장 앞의 UI를 가져오는 메서드
    {
        return m_FrontUI; // 가장 앞의 UI 반환
    }

    public void CloseCurrFrontUI() // 현재 가장 앞의 UI를 닫는 메서드
    {
        m_FrontUI.CloseUI(); // 가장 앞의 UI 닫기
    }

    public void CloseAllOpenUI() // 모든 열린 UI를 닫는 메서드
    {
        while (m_FrontUI) // 가장 앞의 UI가 있는 동안 반복
        {
            m_FrontUI.CloseUI(true); // 가장 앞의 UI 닫기 (전체 닫기 모드)
        }
    }

    // 재화 UI 활성화/비활성화
    public void EnableStatsUI(bool value)
    {
        // 재화 UI 게임오브젝트 활성화 상태 설정
        m_StatsUI.gameObject.SetActive(value);

        // 활성화하는 경우
        if (value)
        {
            // 재화 값들 설정
            m_StatsUI.SetValues();
        }
    }

}
