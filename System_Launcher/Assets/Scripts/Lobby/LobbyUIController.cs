using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 로비 UI를 제어하는 컨트롤러 클래스
public class LobbyUIController : MonoBehaviour
{
    public TextMeshProUGUI CurrChapterNameTxt;
    public RawImage CurrChapterBg;

    // 초기화 메서드
    public void Init()
    {
        UIManager.Instance.EnableStatsUI(true);
        SetCurrChapter();
    }
    public void SetCurrChapter()
    {
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if (userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        var currChapterData = DataTableManager.Instance.GetChapterData(userPlayData.SelectedChapter);
        if (currChapterData == null)
        {
            Logger.LogError("CurrChapterData does not exist.");
            return;
        }

        CurrChapterNameTxt.text = currChapterData.ChapterName;
        var bgTexture = Resources.Load($"ChapterBG/Background_{userPlayData.SelectedChapter.ToString("D3")}") as Texture2D;
        if (bgTexture != null)
        {
            CurrChapterBg.texture = bgTexture;
        }
    }

    // 매 프레임마다 호출되는 업데이트 메서드
    private void Update()
    {
        // 입력 처리 메서드 호출
        HandleInput();
    }

    // 사용자 입력을 처리하는 메서드
    private void HandleInput()
    {
        // ESC 키가 눌렸다가 떼어질 때
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // UI 버튼 클릭 효과음 재생
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            // 현재 최상위에 있는 UI 가져오기
            var frontUI = UIManager.Instance.GetCurrentFrontUI();
            // 열려있는 UI가 있으면
            if (frontUI)
            {
                // 해당 UI 닫기
                frontUI.CloseUI();
            }
            // 열려있는 UI가 없으면
            else
            {
                // 확인 창 데이터 생성
                var uiData = new ConfirmUIData();
                // 확인 창 타입을 OK/Cancel로 설정
                uiData.ConfirmType = ConfirmType.OK_CANCEL;
                // 제목 텍스트 설정
                uiData.TitleTxt = "종료";
                // 설명 텍스트 설정
                uiData.DescTxt = "게임을 종료하시겠습니까?";
                // OK 버튼 텍스트 설정
                uiData.OKBtnTxt = "종료";
                // Cancel 버튼 텍스트 설정
                uiData.CancelBtnTxt = "취소";
                // OK 버튼 클릭 시 실행할 액션 설정
                uiData.OnClickOKBtn = () =>
                {
                    // 게임 종료
                    Application.Quit();
                };
                // 확인 창 UI 열기
                UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            }
        }
    }

    // 설정 버튼 클릭 시 호출되는 메서드
    public void OnClickSettingsBtn()
    {
        // 로그 출력
        Logger.Log($"{GetType()}::OnClickSettingsBtn");

        // 기본 UI 데이터 생성
        var uiData = new BaseUIData();
        // 설정 UI 열기
        UIManager.Instance.OpenUI<SettingsUI>(uiData);
    }

    public void OnClickProfileBtn()
    {
        Logger.Log($"{GetType()}::OnClickProfileBtn");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<InventoryUI>(uiData);
    }

    public void OnClickCurrChapter()
    {
        Logger.Log($"{GetType()}::OnClickCurrChapter");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<ChapterListUI>(uiData);
    }
}