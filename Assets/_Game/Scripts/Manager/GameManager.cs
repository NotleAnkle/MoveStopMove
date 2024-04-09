using _Framework.Event.Scripts;
using _Framework.Singleton;
using UnityEngine;

namespace _UI.Scripts.UI
{
    public enum GameState
    {
        MainMenu = 1,
        GamePlay = 2,
        Finish = 3,
        Revive = 4,
        Setting = 5,
    }
    public enum GameResult
    {
        Win = 1,
        Lose = 2,
    }
    public class GameManager : Singleton<GameManager>
    {
        //[SerializeField] UserData userData;
        //[SerializeField] CSVData csv;
        private static GameState _gameState;
        public static void ChangeState(GameState state)
        {
            _gameState = state;
        }
        public static bool IsState(GameState state) => _gameState == state;
        private void Awake()
        {
            // Tranh viec nguoi choi cham da diem vao man hinh
            Input.multiTouchEnabled = false;
            // Target frame rate ve 60 fps
            Application.targetFrameRate = 60;
            // Tranh viec tat man hinh
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            // Xu tai tho
            int maxScreenHeight = 1280;
            float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
            if (Screen.currentResolution.height > maxScreenHeight)
            {
                Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
            }
            
            //csv.OnInit();
            //userData?.OnInitData();
        }

        private void Start()
        {
            UIManager.Instance.OpenUI<UIMainMenu>();
            LevelManager.Instance.OnReset();

            if (PlayerPrefs.GetInt("firstTime", 1) == 1)
            {
                UserData.Ins.OnResetData();
                PlayerPrefs.SetInt("firstTime", 0);
            }
        }

        public void OnGameStart()
        {
            LevelManager.Instance.OnInit();
            UIManager.Instance.OpenUI<UIPlay>();
            CameraFollower.Instance.ChangeState(CameraFollower.State.Gameplay);
        }
    }
}