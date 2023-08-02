using System;
using UnityEngine;

namespace _Scripts.Managers
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private GameState state;
        
        public event Action<GameState> OnStateChangedEvent;
        
        public event Action OnMenuStateEvent;
        public event Action OnGameStartedEvent;
        public event Action OnGameEndedEvent;
        public event Action OnWinEvent;
        public event Action OnLoseEvent;

        public bool IsPlaying { get; private set; }
        
        public GameState State
        {
            get => state;
            set
            {
                if (value == state) return;

                state = value;

                switch(value)
                {
                    case GameState.Menu:
                        SetMenuState();
                        break;

                    case GameState.Playing:
                        TryStartGame();
                        break;

                    case GameState.Win:
                        TryWin();
                        break;

                    case GameState.Lose:
                        TryLose();
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }

                OnStateChangedEvent?.Invoke(state);
            }
        }
        
        private void Start()
        {
            State = state;
        }

        private void SetMenuState()
        {
            IsPlaying = false;
            OnMenuStateEvent?.Invoke();
        }

        private void TryStartGame()
        {
            if (IsPlaying) return;

            IsPlaying = true;
            OnGameStartedEvent?.Invoke();
        }

        private void TryWin()
        {
            if (IsPlaying == false) return;
            EndGame();

            OnWinEvent?.Invoke();
        }


        private void TryLose()
        {
            if (IsPlaying == false) return;
            EndGame();

            OnLoseEvent?.Invoke();
        }

        private void EndGame()
        {
            IsPlaying = false;
            OnGameEndedEvent?.Invoke();
        }
    }
}