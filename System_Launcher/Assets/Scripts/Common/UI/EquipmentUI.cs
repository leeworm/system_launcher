using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 장비 UI에 전달할 데이터를 담는 클래스
public class EquipmentUIData : BaseUIData
{
    // 아이템의 고유 시리얼 번호
    public long SerialNumber;
    // 아이템 ID
    public int ItemId;
    // 현재 장착 여부
    public bool IsEquipped;
}

// 장비 장착/해제 UI를 담당하는 클래스
public class EquipmentUI : BaseUI
{
    // 아이템 등급 배경 이미지
    public Image ItemGradeBg;
    // 아이템 아이콘 이미지
    public Image ItemIcon;
    // 아이템 등급 텍스트
    public TextMeshProUGUI ItemGradeTxt;
    // 아이템 이름 텍스트
    public TextMeshProUGUI ItemNameTxt;
    // 공격력 수치 텍스트
    public TextMeshProUGUI AttackPowerAmountTxt;
    // 방어력 수치 텍스트
    public TextMeshProUGUI DefenseAmountTxt;
    // 장착/해제 버튼 텍스트
    public TextMeshProUGUI EquipBtnTxt;

    // 장비 UI 데이터를 저장하는 멤버 변수
    private EquipmentUIData m_EquipmentUIData;

    // UI 정보를 설정하는 메서드
    public override void SetInfo(BaseUIData uiData)
    {
        // 부모 클래스의 SetInfo 호출
        base.SetInfo(uiData);

        // 전달받은 데이터를 EquipmentUIData로 캐스팅
        m_EquipmentUIData = uiData as EquipmentUIData;
        // 캐스팅이 실패한 경우 에러 로그 출력 후 종료
        if (m_EquipmentUIData == null)
        {
            Logger.LogError("m_EquipmentUIData is invalid");
            return;
        }

        // 아이템 ID로 아이템 데이터 가져오기
        var itemData = DataTableManager.Instance.GetItemData(m_EquipmentUIData.ItemId);
        // 아이템 데이터가 없으면 에러 로그 출력 후 종료
        if (itemData == null)
        {
            Logger.LogError($"Item data is invalid. ItemId:{m_EquipmentUIData.ItemId}");
            return;
        }

        // 아이템 ID에서 등급 정보 추출 (천의 자리 숫자)
        var itemGrade = (ItemGrade)((m_EquipmentUIData.ItemId / 1000) % 10);
        // 등급에 해당하는 배경 텍스처 로드
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");
        // 텍스처가 존재하면 스프라이트로 변환하여 배경에 적용
        if (gradeBgTexture != null)
        {
            ItemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }

        // 등급 텍스트 설정
        ItemGradeTxt.text = itemGrade.ToString();
        // 등급별 색상 코드를 저장할 변수
        var hexColor = string.Empty;
        // 등급에 따른 색상 설정
        switch (itemGrade)
        {
            case ItemGrade.Common:
                hexColor = "#1AB3FF";
                break;
            case ItemGrade.Uncommon:
                hexColor = "#51C52C";
                break;
            case ItemGrade.Rare:
                hexColor = "#EA5AFF";
                break;
            case ItemGrade.Epic:
                hexColor = "#FF9900";
                break;
            case ItemGrade.Legendary:
                hexColor = "#F24949";
                break;
            default:
                break;
        }

        // 색상 변환용 변수
        Color color;
        // 16진수 색상 코드를 Color로 변환하여 적용
        if (ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            ItemGradeTxt.color = color;
        }

        // 아이템 ID를 문자열로 변환하여 StringBuilder에 저장
        StringBuilder sb = new StringBuilder(m_EquipmentUIData.ItemId.ToString());
        // 두 번째 자리 숫자를 '1'로 변경 (아이콘 이름 규칙)
        sb[1] = '1';
        // 변경된 문자열을 아이콘 이름으로 사용
        var itemIconName = sb.ToString();

        // 아이콘 텍스처 로드
        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        // 텍스처가 존재하면 스프라이트로 변환하여 아이콘에 적용
        if (itemIconTexture != null)
        {
            ItemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }

        // 아이템 이름 설정
        ItemNameTxt.text = itemData.ItemName;
        // 공격력 수치 설정 (+ 기호와 함께 표시)
        AttackPowerAmountTxt.text = $"+{itemData.AttackPower}";
        // 방어력 수치 설정 (+ 기호와 함께 표시)
        DefenseAmountTxt.text = $"+{itemData.Defense}";
        // 장착 상태에 따라 버튼 텍스트 설정
        EquipBtnTxt.text = m_EquipmentUIData.IsEquipped ? "해제" : "장착";
    }

    // 장착/해제 버튼 클릭 시 호출되는 메서드
    public void OnClickEquipBtn()
    {
        // 사용자 인벤토리 데이터 가져오기
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        // 인벤토리 데이터가 없으면 로그 출력 후 종료
        if (userInventoryData == null)
        {
            Logger.Log("UserInventoryData does not exist.");
            return;
        }

        // 현재 장착 상태에 따라 장착/해제 처리
        if (m_EquipmentUIData.IsEquipped)
        {
            // 아이템 해제
            userInventoryData.UnequipItem(m_EquipmentUIData.SerialNumber, m_EquipmentUIData.ItemId);
        }
        else
        {
            // 아이템 장착
            userInventoryData.EquipItem(m_EquipmentUIData.SerialNumber, m_EquipmentUIData.ItemId);
        }

        // 변경된 데이터 저장
        userInventoryData.SaveData();

        // 활성화된 인벤토리 UI 가져오기
        var inventoryUI = UIManager.Instance.GetActiveUI<InventoryUI>() as InventoryUI;
        // 인벤토리 UI가 존재하면 상태 업데이트
        if (inventoryUI != null)
        {
            // 장착 상태에 따라 인벤토리 UI 업데이트
            if (m_EquipmentUIData.IsEquipped)
            {
                // 해제된 아이템 UI 업데이트
                inventoryUI.OnUnequipItem(m_EquipmentUIData.ItemId);
            }
            else
            {
                // 장착된 아이템 UI 업데이트
                inventoryUI.OnEquipItem(m_EquipmentUIData.ItemId);
            }
        }

        // 현재 UI 닫기
        CloseUI();
    }
}