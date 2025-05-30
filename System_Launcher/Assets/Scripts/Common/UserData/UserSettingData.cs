using UnityEngine;

// 사용자 설정 데이터 관리 클래스
public class UserSettingsData : IUserData
{
    // 사운드 on/off 설정
    public bool Sound { get; set; }

    // 기본값으로 초기화
    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");

        Sound = true; // 사운드를 기본적으로 켜짐으로 설정
    }

    // PlayerPrefs에서 설정 데이터 로드
    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false; // 로드 결과 저장용 변수

        try
        {
            // PlayerPrefs에서 사운드 설정을 int로 읽어와서 bool로 변환
            Sound = PlayerPrefs.GetInt("Sound") == 1 ? true : false;
            result = true; // 로드 성공

            Logger.Log($"Sound:{Sound}");
        }
        catch (System.Exception e)
        {
            // 로드 실패 처리
            Logger.Log("Load failed (" + e.Message + ")");
        }

        return result; // 로드 결과 반환
    }

    // PlayerPrefs에 설정 데이터 저장
    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");

        bool result = false; // 저장 결과 저장용 변수

        try
        {
            // bool 값을 int로 변환하여 PlayerPrefs에 저장
            PlayerPrefs.SetInt("Sound", Sound ? 1 : 0);
            PlayerPrefs.Save(); // 변경사항을 디스크에 저장

            result = true; // 저장 성공

            Logger.Log($"Sound:{Sound}");
        }
        catch (System.Exception e)
        {
            // 저장 실패 처리
            Logger.Log("Save failed (" + e.Message + ")");
        }

        return result; // 저장 결과 반환
    }
}