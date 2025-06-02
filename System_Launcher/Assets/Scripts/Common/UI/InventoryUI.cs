// GPM UI 네임스페이스 사용
using Gpm.Ui;
// TextMeshPro 네임스페이스 사용
using TMPro;

// 인벤토리 UI 클래스 (BaseUI 상속)
public class InventoryUI : BaseUI
{
    // 인벤토리 스크롤 리스트 컴포넌트
    public InfiniteScroll InventoryScrollList;

    // UI 정보 설정 메서드 (부모 클래스 오버라이드)
    public override void SetInfo(BaseUIData uiData)
    {
        // 부모 클래스의 SetInfo 호출
        base.SetInfo(uiData);

        // 인벤토리 설정 메서드 호출
        SetInventory();
    }

    // 인벤토리 설정 메서드
    private void SetInventory()
    {
        // 인벤토리 스크롤 리스트 초기화
        InventoryScrollList.Clear();

        // 유저 인벤토리 데이터 가져오기
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        // 유저 인벤토리 데이터가 존재하는 경우
        if (userInventoryData != null)
        {
            // 인벤토리 아이템 데이터 리스트 순회
            foreach (var itemData in userInventoryData.InventoryItemDataList)
            {
                // 아이템 슬롯 데이터 객체 생성
                var itemSlotData = new InventoryItemSlotData();
                // 시리얼 번호 설정
                itemSlotData.SerialNumber = itemData.SerialNumber;
                // 아이템 ID 설정
                itemSlotData.ItemId = itemData.ItemId;
                // 스크롤 리스트에 아이템 데이터 추가
                InventoryScrollList.InsertData(itemSlotData);
            }
        }
    }
}