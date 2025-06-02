// 시스템 네임스페이스 사용
using System;
// 시스템 컬렉션 네임스페이스 사용
using System.Collections;
// 시스템 컬렉션 제네릭 네임스페이스 사용
using System.Collections.Generic;
// Unity 엔진 네임스페이스 사용
using UnityEngine;

// 직렬화 가능한 클래스로 설정
[Serializable]
// 유저 아이템 데이터 클래스
public class UserItemData
{
    // 고유 시리얼 번호 (유니크 값)
    public long SerialNumber; //unique value
    // 아이템 ID
    public int ItemId;

    // 유저 아이템 데이터 생성자
    public UserItemData(long serialNumber, int itemId)
    {
        // 시리얼 번호 설정
        SerialNumber = serialNumber;
        // 아이템 ID 설정
        ItemId = itemId;
    }
}

// JSONUtility를 사용하여 JSON으로 파싱하기 위한 래퍼 클래스
//wrapper class to parse data to JSON using JSONUtility
// 직렬화 가능한 클래스로 설정
[Serializable]
// 유저 인벤토리 아이템 데이터 리스트 래퍼 클래스
public class UserInventoryItemDataListWrapper
{
    // 인벤토리 아이템 데이터 리스트
    public List<UserItemData> InventoryItemDataList;
}

// 유저 인벤토리 데이터 클래스 (IUserData 인터페이스 구현)
public class UserInventoryData : IUserData
{
    // 인벤토리 아이템 데이터 리스트 프로퍼티 (기본값: 빈 리스트)
    public List<UserItemData> InventoryItemDataList { get; set; } = new List<UserItemData>();

    // 기본 데이터 설정 메서드
    public void SetDefaultData()
    {
        // 로그 출력
        Logger.Log($"{GetType()}::SetDefaultData");

        // 시리얼 번호 => DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4") 
        //serial number => DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4") 
        // 기본 아이템들을 인벤토리에 추가 (시리얼 번호는 현재 시간 + 랜덤 4자리 숫자로 생성)
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11001));
        // 아이템 11002 추가
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11002));
        // 아이템 21001 추가
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 21001));
        // 아이템 21002 추가
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 21002));
        // 아이템 31001 추가
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 31001));
        // 아이템 31002 추가
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 31002));
        // 아이템 41001 추가
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 41001));
        // 아이템 41002 추가
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 41002));
        // 아이템 51001 추가
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 51001));
        // 아이템 51002 추가
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 51002));
        // 아이템 61001 추가
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 61001));
        // 아이템 61002 추가
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 61002));
    }

    // 데이터 로드 메서드
    public bool LoadData()
    {
        // 로그 출력
        Logger.Log($"{GetType()}::LoadData");

        // 결과 변수 초기화
        bool result = false;

        // 예외 처리 시작
        try
        {
            // PlayerPrefs에서 인벤토리 아이템 데이터 리스트 JSON 문자열 가져오기
            string inventoryItemDataListJson = PlayerPrefs.GetString("InventoryItemDataList");
            // JSON 문자열이 null이거나 비어있지 않으면
            if (!string.IsNullOrEmpty(inventoryItemDataListJson))
            {
                // JSON을 UserInventoryItemDataListWrapper 객체로 변환
                UserInventoryItemDataListWrapper itemDataListWrapper = JsonUtility.FromJson<UserInventoryItemDataListWrapper>(inventoryItemDataListJson);
                // 래퍼에서 실제 데이터 리스트 가져오기
                InventoryItemDataList = itemDataListWrapper.InventoryItemDataList;

                // 로그 출력
                Logger.Log("InventoryItemDataList");
                // 인벤토리 아이템들 순회하며 로그 출력
                foreach (var item in InventoryItemDataList)
                {
                    // 각 아이템의 시리얼 번호와 아이템 ID 로그 출력
                    Logger.Log($"SerialNumber:{item.SerialNumber} ItemId:{item.ItemId}");
                }
            }

            // 로드 성공
            result = true;
        }
        // 예외 발생 시 처리
        catch (System.Exception e)
        {
            // 로드 실패 로그 출력
            Logger.Log("Load failed (" + e.Message + ")");
        }

        // 결과 반환
        return result;
    }

    // 데이터 저장 메서드
    public bool SaveData()
    {
        // 로그 출력
        Logger.Log($"{GetType()}::SaveData");

        // 결과 변수 초기화
        bool result = false;

        // 예외 처리 시작
        try
        {
            // 인벤토리 아이템 데이터 리스트 래퍼 객체 생성
            UserInventoryItemDataListWrapper inventoryItemDataListWrapper = new UserInventoryItemDataListWrapper();
            // 래퍼에 인벤토리 아이템 데이터 리스트 설정
            inventoryItemDataListWrapper.InventoryItemDataList = InventoryItemDataList;
            // 래퍼 객체를 JSON 문자열로 변환
            string inventoryItemDataListJson = JsonUtility.ToJson(inventoryItemDataListWrapper);
            // PlayerPrefs에 JSON 문자열 저장
            PlayerPrefs.SetString("InventoryItemDataList", inventoryItemDataListJson);

            // 로그 출력
            Logger.Log("InventoryItemDataList");
            // 인벤토리 아이템들 순회하며 로그 출력
            foreach (var item in InventoryItemDataList)
            {
                // 각 아이템의 시리얼 번호와 아이템 ID 로그 출력
                Logger.Log($"SerialNumber:{item.SerialNumber} ItemId:{item.ItemId}");
            }

            // PlayerPrefs 저장
            PlayerPrefs.Save();
            // 저장 성공
            result = true;
        }
        // 예외 발생 시 처리
        catch (System.Exception e)
        {
            // 저장 실패 로그 출력 (메시지가 잘못 표시됨 - "Load failed"가 아닌 "Save failed"여야 함)
            Logger.Log("Load failed (" + e.Message + ")");
        }

        // 결과 반환
        return result;
    }
}