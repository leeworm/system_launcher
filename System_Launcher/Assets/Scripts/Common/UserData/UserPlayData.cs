// 필요한 네임스페이스 불러오기
using System.Collections;
// 제네릭 컬렉션 네임스페이스 불러오기
using System.Collections.Generic;
// Unity 엔진 네임스페이스 불러오기
using UnityEngine;

// 사용자 플레이 데이터를 관리하는 클래스, IUserData 인터페이스 구현
public class UserPlayData : IUserData
{
    // 최대 클리어한 챕터 번호 프로퍼티
    public int MaxClearedChapter { get; set; }
    // PlayerPrefs에 저장되지 않는 현재 선택된 챕터 번호 (기본값 1)
    //not saved to playerprefs
    public int SelectedChapter { get; set; } = 1;

    // 기본 데이터 설정 메서드
    public void SetDefaultData()
    {
        // 메서드 호출 로그 출력
        Logger.Log($"{GetType()}::SetDefaultData");

        // 최대 클리어 챕터를 0으로 초기화
        MaxClearedChapter = 0;
        // 선택된 챕터를 1로 초기화
        SelectedChapter = 1;
    }

    // 저장된 데이터를 불러오는 메서드
    public bool LoadData()
    {
        // 메서드 호출 로그 출력
        Logger.Log($"{GetType()}::LoadData");

        // 로드 결과를 저장할 변수
        bool result = false;

        // 예외 처리를 위한 try-catch 블록 시작
        try
        {
            // PlayerPrefs에서 최대 클리어 챕터 값 불러오기
            MaxClearedChapter = PlayerPrefs.GetInt("MaxClearedChapter");
            // 선택된 챕터를 최대 클리어 챕터 + 1로 설정
            SelectedChapter = MaxClearedChapter + 1;

            // 로드 성공으로 설정
            result = true;

            // 불러온 최대 클리어 챕터 로그 출력
            Logger.Log($"MaxClearedChpater:{MaxClearedChapter}");
        }
        // 예외 발생 시 처리
        catch (System.Exception e)
        {
            // 로드 실패 로그 출력
            Logger.Log($"Load failed. (" + e.Message + ")");
        }

        // 로드 결과 반환
        return result;
    }

    // 데이터를 저장하는 메서드
    public bool SaveData()
    {
        // 메서드 호출 로그 출력
        Logger.Log($"{GetType()}::SaveData");

        // 저장 결과를 저장할 변수
        bool result = false;

        // 예외 처리를 위한 try-catch 블록 시작
        try
        {
            // PlayerPrefs에 최대 클리어 챕터 값 저장
            PlayerPrefs.SetInt("MaxClearedChapter", MaxClearedChapter);
            // PlayerPrefs 저장 실행
            PlayerPrefs.Save();

            // 저장 성공으로 설정
            result = true;

            // 저장된 최대 클리어 챕터 로그 출력
            Logger.Log($"MaxClearedChpater:{MaxClearedChapter}");
        }
        // 예외 발생 시 처리
        catch (System.Exception e)
        {
            // 저장 실패 로그 출력
            Logger.Log($"Save failed. (" + e.Message + ")");
        }

        // 저장 결과 반환
        return result;
    }
}