using UnityEngine;

// 사용자 재화 데이터 관리 클래스
public class UserGoodsData : IUserData
{
    // 보석 수량
    public long Gem { get; set; }
    // 골드 수량
    public long Gold { get; set; }

    // 기본값으로 초기화
    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");

        Gem = 0;
        Gold = 0;
    }

    // PlayerPrefs에서 데이터 로드
    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;

        try
        {
            // 저장된 재화 데이터 읽기
            Gem = long.Parse(PlayerPrefs.GetString("Gem"));
            Gold = long.Parse(PlayerPrefs.GetString("Gold"));
            result = true;

            Logger.Log($"Gem:{Gem} Gold:{Gold}");
        }
        catch (System.Exception e)
        {
            // 로드 실패 처리
            Logger.Log("Load failed (" + e.Message + ")");
        }

        return result;
    }

    // PlayerPrefs에 데이터 저장
    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");

        bool result = false;

        try
        {
            // 재화 데이터 저장
            PlayerPrefs.SetString("Gem", Gem.ToString());
            PlayerPrefs.SetString("Gold", Gold.ToString());
            PlayerPrefs.Save();

            result = true;

            Logger.Log($"Gem:{Gem} Gold:{Gold}");
        }
        catch (System.Exception e)
        {
            // 저장 실패 처리
            Logger.Log("Save failed (" + e.Message + ")");
        }

        return result;
    }
}
