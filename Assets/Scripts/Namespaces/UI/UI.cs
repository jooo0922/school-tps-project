// UI 관련 클래스 및 구조체 정의 네임스페이스
namespace UI
{
    namespace Parameters
    {
        // UIManager > OnGameResultUpdate 델리게이트 파라미터를 정의한 구조체
        public struct GameResult
        {
            public bool isGameWon;
            public int totalScore;
            public string timeText;
            public string killsText;
        }
    }
}