using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 데이터 테이블을 관리하는 싱글톤 매니저 클래스
public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTable"; // 데이터 테이블 파일 경로

    private const string CHAPTER_DATA_TABLE = "ChapterDataTable"; // 챕터 데이터 테이블 파일명
    private List<ChapterData> ChapterDataTable = new List<ChapterData>(); // 챕터 데이터 리스트

    // 초기화 함수
    protected override void Init()
    {
        base.Init(); // 부모 클래스 초기화 호출

        LoadChapterDataTable(); // 챕터 데이터 테이블 로드
    }

    // 챕터 데이터 테이블 로드 함수
    private void LoadChapterDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{CHAPTER_DATA_TABLE}"); // CSV 파일 읽기

        foreach (var data in parsedDataTable) // 파싱된 데이터를 순회
        {
            var chapterData = new ChapterData // 새 챕터 데이터 객체 생성
            {
                ChapterNo = Convert.ToInt32(data["chapter_no"]), // 챕터 번호 설정
                TotalStages = Convert.ToInt32(data["total_stages"]), // 총 스테이지 수 설정
                ChapterRewardGem = Convert.ToInt32(data["chapter_reward_gem"]), // 챕터 클리어 보상 보석 설정
                ChapterRewardGold = Convert.ToInt32(data["chapter_reward_gold"]), // 챕터 클리어 보상 골드 설정
            };

            ChapterDataTable.Add(chapterData); // 리스트에 챕터 데이터 추가
        }
    }

    // 특정 챕터 번호의 데이터를 반환하는 함수
    public ChapterData GetChapterData(int chapterNo)
    {
        return ChapterDataTable.Where(item => item.ChapterNo == chapterNo).FirstOrDefault(); // LINQ로 챕터 번호에 해당하는 데이터 검색 후 반환
    }

    // 아이템 데이터 관련 영역 시작
    #region ITEM_DATA
    // 아이템 데이터 테이블 파일명 상수
    private const string ITEM_DATA_TABLE = "ItemDataTable";
    // 아이템 데이터 리스트
    private List<ItemData> ItemDataTable = new List<ItemData>();

    // 아이템 데이터 테이블 로드 메서드
    private void LoadItemDataTable()
    {
        // CSV 파일을 읽어서 파싱된 데이터 가져오기
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{ITEM_DATA_TABLE}");

        // 파싱된 데이터 순회
        foreach (var data in parsedDataTable)
        {
            // 아이템 데이터 객체 생성
            var itemData = new ItemData
            {
                // 아이템 ID 설정
                ItemId = Convert.ToInt32(data["item_id"]),
                // 아이템 이름 설정
                ItemName = data["item_name"].ToString(),
                // 공격력 설정
                AttackPower = Convert.ToInt32(data["attack_power"]),
                // 방어력 설정
                Defense = Convert.ToInt32(data["defense"]),
            };

            // 아이템 데이터를 리스트에 추가
            ItemDataTable.Add(itemData);
        }
    }

    // 특정 아이템 ID의 아이템 데이터 가져오기
    public ItemData GetItemData(int itemId)
    {
        // LINQ를 사용하여 해당 아이템 ID의 데이터 반환
        return ItemDataTable.Where(item => item.ItemId == itemId).FirstOrDefault();
    }
    // 아이템 데이터 관련 영역 끝
    #endregion


}

// 챕터 데이터 클래스
public class ChapterData
{
    public int ChapterNo; // 챕터 번호
    public int TotalStages; // 총 스테이지 수
    public int ChapterRewardGem; // 챕터 클리어 보상 보석
    public int ChapterRewardGold; // 챕터 클리어 보상 골드
}

// 아이템 데이터 클래스
public class ItemData
{
    // 아이템 ID
    public int ItemId;
    // 아이템 이름
    public string ItemName;
    // 공격력
    public int AttackPower;
    // 방어력
    public int Defense;
}

// 아이템 타입 열거형
public enum ItemType
{
    // 무기
    Weapon = 1,
    // 방패
    Shield,
    // 흉갑
    ChestArmor,
    // 장갑
    Gloves,
    // 부츠
    Boots,
    // 악세서리
    Accessory
}

// 아이템 등급 열거형
public enum ItemGrade
{
    // 일반
    Common = 1,
    // 고급
    Uncommon,
    // 희귀
    Rare,
    // 영웅
    Epic,
    // 전설
    Legendary,
}
