// TextMeshPro 네임스페이스 사용
using TMPro;
// Unity 엔진 네임스페이스 사용
using UnityEngine;

// 유저 재화 정보를 표시하는 UI 클래스
public class GoodsUI : MonoBehaviour
{
    // 골드 수량을 표시하는 텍스트 컴포넌트
    public TextMeshProUGUI GoldAmountTxt;
    // 젬 수량을 표시하는 텍스트 컴포넌트
    public TextMeshProUGUI GemAmountTxt;

    // 재화 값들을 설정하는 메서드
    public void SetValues()
    {
        // 유저 재화 데이터 가져오기
        var userGoodData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        // 유저 재화 데이터가 없으면
        if (userGoodData == null)
        {
            // 에러 로그 출력
            Logger.LogError("No user goods data.");
            // 메서드 종료
            return;
        }

        // 골드 수량을 천단위 콤마 형식으로 텍스트에 설정
        GoldAmountTxt.text = userGoodData.Gold.ToString("N0");
        // 젬 수량을 천단위 콤마 형식으로 텍스트에 설정
        GemAmountTxt.text = userGoodData.Gem.ToString("N0");
    }
}