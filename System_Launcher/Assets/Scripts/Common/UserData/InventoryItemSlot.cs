// GPM UI 네임스페이스 사용
using Gpm.Ui;
// Unity 엔진 네임스페이스 사용
using UnityEngine;
// Unity UI 네임스페이스 사용
using UnityEngine.UI;
// 시스템 네임스페이스 사용
using System;
// 시스템 텍스트 네임스페이스 사용
using System.Text;

// 인벤토리 아이템 슬롯 데이터 클래스 (InfiniteScrollData 상속)
public class InventoryItemSlotData : InfiniteScrollData
{
    // 아이템 시리얼 번호
    public long SerialNumber;
    // 아이템 ID
    public int ItemId;
}

// 인벤토리 아이템 슬롯 클래스 (InfiniteScrollItem 상속)
public class InventoryItemSlot : InfiniteScrollItem
{
    // 아이템 등급 배경 이미지
    public Image ItemGradeBg;
    // 아이템 아이콘 이미지
    public Image ItemIcon;

    // 인벤토리 아이템 슬롯 데이터 멤버 변수
    private InventoryItemSlotData m_InventoryItemSlotData;

    // 데이터 업데이트 메서드 (부모 클래스 오버라이드)
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        // 부모 클래스의 UpdateData 호출
        base.UpdateData(scrollData);

        // 스크롤 데이터를 InventoryItemSlotData로 캐스팅
        m_InventoryItemSlotData = scrollData as InventoryItemSlotData;
        // 캐스팅된 데이터가 null인 경우
        if (m_InventoryItemSlotData == null)
        {
            // 에러 로그 출력
            Logger.LogError("m_InventoryItemSlotData is invalid.");
            // 메서드 종료
            return;
        }

        // 아이템 ID에서 등급 정보 추출 (천의 자리 숫자를 ItemGrade 열거형으로 변환)
        var itemGrade = (ItemGrade)((m_InventoryItemSlotData.ItemId / 1000) % 10);
        // 등급에 따른 배경 텍스처 로드
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");
        // 배경 텍스처가 존재하는 경우
        if (gradeBgTexture != null)
        {
            // 텍스처를 스프라이트로 변환하여 배경 이미지에 설정
            ItemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }

        // 아이템 ID 문자열을 StringBuilder로 변환
        StringBuilder sb = new StringBuilder(m_InventoryItemSlotData.ItemId.ToString());
        // 두 번째 자리 숫자를 '1'로 변경 (아이콘 이름 규칙)
        sb[1] = '1';
        // 변경된 문자열을 아이템 아이콘 이름으로 사용
        var itemIconName = sb.ToString();

        // 아이템 아이콘 텍스처 로드
        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        // 아이콘 텍스처가 존재하는 경우
        if (itemIconTexture != null)
        {
            // 텍스처를 스프라이트로 변환하여 아이콘 이미지에 설정
            ItemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }
    }
}