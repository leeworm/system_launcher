// Unity 엔진 네임스페이스 사용
using UnityEngine;
// Unity 에디터 네임스페이스 사용
using UnityEditor;

// Unity 에디터에서만 컴파일되는 조건부 컴파일 지시문
#if UNITY_EDITOR
// Unity 에디터 커스텀 도구 클래스
public class CustomTools : Editor
{
    // 유저 젬을 10개 추가하는 메뉴 아이템
    [MenuItem("Inflearn/Add User Gem (+10)")]
    // 유저 젬 추가 메서드
    public static void AddUserGem()
    {
        // PlayerPrefs에서 젬 값을 문자열로 가져와서 long 타입으로 변환
        var Gem = long.Parse(PlayerPrefs.GetString("Gem"));
        // 젬 값에 10 추가
        Gem += 10;

        // 업데이트된 젬 값을 문자열로 변환하여 PlayerPrefs에 저장
        PlayerPrefs.SetString("Gem", Gem.ToString());
        // PlayerPrefs 저장 내용을 디스크에 즉시 저장
        PlayerPrefs.Save();
    }

    // 유저 골드를 100개 추가하는 메뉴 아이템
    [MenuItem("Inflearn/Add User Gold (+100)")]
    // 유저 골드 추가 메서드
    public static void AddUserGold()
    {
        // PlayerPrefs에서 골드 값을 문자열로 가져와서 long 타입으로 변환
        var Gold = long.Parse(PlayerPrefs.GetString("Gold"));
        // 골드 값에 100 추가
        Gold += 100;

        // 업데이트된 골드 값을 문자열로 변환하여 PlayerPrefs에 저장
        PlayerPrefs.SetString("Gold", Gold.ToString());
        // PlayerPrefs 저장 내용을 디스크에 즉시 저장
        PlayerPrefs.Save();
    }
}
// Unity 에디터 조건부 컴파일 지시문 종료
#endif