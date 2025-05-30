using System;
using System.Collections.Generic;
using System.Linq;

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
}

// 챕터 데이터 클래스
public class ChapterData
{
    public int ChapterNo; // 챕터 번호
    public int TotalStages; // 총 스테이지 수
    public int ChapterRewardGem; // 챕터 클리어 보상 보석
    public int ChapterRewardGold; // 챕터 클리어 보상 골드
}