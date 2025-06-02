using System.Diagnostics;

// 로깅을 위한 유틸리티 클래스
// 1. 추가적인 정보 표현 (ex.타임스탬프)
// 2. 출시용 빌드를 위한 로그 제거
public static class Logger
{
    // 일반 로그 메시지를 출력. DEV_VER 조건부 컴파일에서만 동작
    [Conditional("DEV_VER")]
    public static void Log(string msg)
    {
        UnityEngine.Debug.LogFormat("[{0}] {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), msg);
    }

    // 경고 로그 메시지를 출력. DEV_VER 조건부 컴파일에서만 동작 
    [Conditional("DEV_VER")]
    public static void LogWarning(string msg)
    {
        UnityEngine.Debug.LogWarningFormat("[{0}] {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), msg);
    }

    // 에러 로그 메시지를 출력. 항상 동작
    public static void LogError(string msg)
    {
        UnityEngine.Debug.LogErrorFormat("[{0}] {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), msg);
    }
}