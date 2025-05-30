using System.Collections.Generic;
using UnityEngine;

// 사용자 데이터를 관리하는 싱글톤 매니저 클래스
public class UserDataManager : SingletonBehaviour<UserDataManager>
{
    // 저장된 데이터가 존재하는지 여부
    public bool ExistsSavedData { get; private set; }
    // 모든 사용자 데이터 인스턴스를 관리하는 컨테이너
    public List<IUserData> UserDataList { get; private set; } = new List<IUserData>();

    // 초기화 함수
    protected override void Init()
    {
        base.Init(); // 부모 클래스 초기화 호출

        // 모든 사용자 데이터를 UserDataList에 추가
        UserDataList.Add(new UserSettingsData());
        UserDataList.Add(new UserGoodsData());
    }

    // 모든 사용자 데이터를 기본값으로 설정
    public void SetDefaultUserData()
    {
        // 리스트의 모든 데이터에 대해 반복
        for (int i = 0; i < UserDataList.Count; i++)
        {
            UserDataList[i].SetDefaultData(); // 각 데이터를 기본값으로 초기화
        }
    }

    // 사용자 데이터 로드
    public void LoadUserData()
    {
        // PlayerPrefs에서 저장된 데이터 존재 여부 확인
        ExistsSavedData = PlayerPrefs.GetInt("ExistsSavedData") == 1 ? true : false;

        // 저장된 데이터가 있는 경우에만 로드
        if (ExistsSavedData)
        {
            // 리스트의 모든 데이터를 로드
            for (int i = 0; i < UserDataList.Count; i++)
            {
                UserDataList[i].LoadData(); // 각 데이터 로드 실행
            }
        }
    }

    // 사용자 데이터 저장
    public void SaveUserData()
    {
        bool hasSaveError = false; // 저장 오류 발생 여부 추적

        // 리스트의 모든 데이터를 저장 시도
        for (int i = 0; i < UserDataList.Count; i++)
        {
            bool isSaveSuccess = UserDataList[i].SaveData(); // 각 데이터 저장 실행
            if (!isSaveSuccess) // 저장 실패 시
            {
                hasSaveError = true; // 오류 플래그 설정
            }
        }

        // 모든 데이터가 성공적으로 저장된 경우
        if (!hasSaveError)
        {
            ExistsSavedData = true; // 저장 데이터 존재 플래그 설정
            PlayerPrefs.SetInt("ExistsSavedData", 1); // PlayerPrefs에 저장
            PlayerPrefs.Save(); // 변경사항을 디스크에 저장
        }
    }
}