using System; // System 네임스페이스 사용
using TMPro; // TextMeshPro 네임스페이스 사용
using UnityEngine.UI; // Unity UI 네임스페이스 사용

public enum ConfirmType // 확인창 타입 열거형
{
    OK, // 확인 버튼만 있는 타입
    OK_CANCEL // 확인과 취소 버튼이 모두 있는 타입
}

public class ConfirmUIData : BaseUIData // BaseUIData를 상속받는 확인창 UI 데이터 클래스
{
    public ConfirmType ConfirmType; // 확인창 타입
    public string TitleTxt; // 제목 텍스트
    public string DescTxt; // 설명 텍스트
    public string OKBtnTxt; // 확인 버튼 텍스트
    public Action OnClickOKBtn; // 확인 버튼 클릭 시 실행할 액션
    public string CancelBtnTxt; // 취소 버튼 텍스트
    public Action OnClickCancelBtn; // 취소 버튼 클릭 시 실행할 액션
}




public class ConfirmUI : BaseUI // BaseUI를 상속받는 확인창 UI 클래스
{
    public TextMeshProUGUI TitleTxt = null; // 제목 텍스트 UI 컴포넌트
    public TextMeshProUGUI DescTxt = null; // 설명 텍스트 UI 컴포넌트
    public Button OKBtn = null; // 확인 버튼 UI 컴포넌트
    public Button CancelBtn = null; // 취소 버튼 UI 컴포넌트
    public TextMeshProUGUI OKBtnTxt = null; // 확인 버튼 텍스트 UI 컴포넌트
    public TextMeshProUGUI CancelBtnTxt = null; // 취소 버튼 텍스트 UI 컴포넌트

    private ConfirmUIData m_ConfirmUIData = null; // 확인창 UI 데이터 저장 변수
    private Action m_OnClickOKBtn = null; // 확인 버튼 클릭 액션 저장 변수
    private Action m_OnClickCancelBtn = null; // 취소 버튼 클릭 액션 저장 변수


    public override void SetInfo(BaseUIData uiData) // UI 정보 설정 메서드 오버라이드
    {
        base.SetInfo(uiData); // 부모 클래스의 SetInfo 호출

        m_ConfirmUIData = uiData as ConfirmUIData; // BaseUIData를 ConfirmUIData로 캐스팅

        TitleTxt.text = m_ConfirmUIData.TitleTxt; // 제목 텍스트 설정
        DescTxt.text = m_ConfirmUIData.DescTxt; // 설명 텍스트 설정
        OKBtnTxt.text = m_ConfirmUIData.OKBtnTxt; // 확인 버튼 텍스트 설정
        m_OnClickOKBtn = m_ConfirmUIData.OnClickOKBtn; // 확인 버튼 클릭 액션 설정
        CancelBtnTxt.text = m_ConfirmUIData.CancelBtnTxt; // 취소 버튼 텍스트 설정
        m_OnClickCancelBtn = m_ConfirmUIData.OnClickCancelBtn; // 취소 버튼 클릭 액션 설정

        OKBtn.gameObject.SetActive(true); // 확인 버튼 활성화
        // 확인창 타입이 OK_CANCEL인 경우에만 취소 버튼 활성화
        CancelBtn.gameObject.SetActive(m_ConfirmUIData.ConfirmType == ConfirmType.OK_CANCEL); 
    }

    public void OnClickOKBtn() // 확인 버튼 클릭 시 호출되는 메서드
    {
        m_OnClickOKBtn?.Invoke(); // 확인 버튼 클릭 액션 실행
        CloseUI(); // UI 닫기
    }

    public void OnClickCancelBtn() // 취소 버튼 클릭 시 호출되는 메서드
    {
        m_OnClickCancelBtn?.Invoke(); // 취소 버튼 클릭 액션 실행
        CloseUI(); // UI 닫기
    }

}
