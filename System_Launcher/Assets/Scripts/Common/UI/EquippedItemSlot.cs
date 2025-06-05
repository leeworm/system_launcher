// 코루틴 관련 라이브러리 사용
using System.Collections;
// 컬렉션 관련 라이브러리 사용
using System.Collections.Generic;
// 문자열 처리를 위한 StringBuilder 사용
using System.Text;
// Unity 기본 라이브러리 사용
using UnityEngine;
// Unity UI 컴포넌트 사용
using UnityEngine.UI;

// 장착된 아이템 슬롯 클래스 - MonoBehaviour를 상속받음
public class EquippedItemSlot : MonoBehaviour
{
    // 추가 아이콘 이미지 컴포넌트 (아이템이 없을 때 표시)
    public Image AddIcon;
    // 장착된 아이템 아이콘 이미지 컴포넌트
    public Image EquippedItemGradeBg;
    public Image EquippedItemIcon;

    // 장착된 아이템 데이터 멤버 변수
    private UserItemData m_EquippedItemData;

    // 아이템을 설정하는 메서드
    public void SetItem(UserItemData userItemData)
    {
        // 전달받은 아이템 데이터 저장
        m_EquippedItemData = userItemData;

        // 추가 아이콘 비활성화
        AddIcon.gameObject.SetActive(false);

        // 장착된 아이템 아이콘 활성화
        EquippedItemGradeBg.gameObject.SetActive(true);
        EquippedItemIcon.gameObject.SetActive(true);

        var itemGrade = (ItemGrade)((m_EquippedItemData.ItemId / 1000) % 10);
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");
        if (gradeBgTexture != null)
        {
            EquippedItemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }
            
        // 아이템 ID를 문자열로 변환하여 StringBuilder 생성
        StringBuilder sb = new StringBuilder(m_EquippedItemData.ItemId.ToString());
        // 두 번째 자리 숫자를 '1'로 변경 (아이콘용 ID 생성)
        sb[1] = '1';
        // 아이콘 이름으로 사용할 문자열 생성
        var itemIconName = sb.ToString();

        // 아이콘 텍스처 로드
        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        // 아이콘 텍스처가 존재하는 경우
        if(itemIconTexture != null)
        {
            // 텍스처로 스프라이트 생성하여 아이콘 이미지에 설정
            EquippedItemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }
    }

    // 아이템을 제거하는 메서드
    public void ClearItem()
    {
        // 장착된 아이템 데이터 초기화
        m_EquippedItemData = null;

        // 추가 아이콘 활성화
        AddIcon.gameObject.SetActive(true);
        // 장착된 아이템 아이콘 비활성화
        EquippedItemGradeBg.gameObject.SetActive(false);
        EquippedItemIcon.gameObject.SetActive(false);
    }

    // 장착된 아이템 슬롯 클릭 시 호출되는 메서드
    public void OnClickEquippedItemSlot()
    {
        // 새로운 장비 UI 데이터 생성
        var uiData = new EquipmentUIData();
        // 시리얼 번호 설정
        uiData.SerialNumber = m_EquippedItemData.SerialNumber;
        // 아이템 ID 설정
        uiData.ItemId = m_EquippedItemData.ItemId;
        // 장착 상태 플래그 설정 (장착됨)
        uiData.IsEquipped = true;
        // 장비 UI 열기
        UIManager.Instance.OpenUI<EquipmentUI>(uiData);
    }
}