using System;
using System.Collections;
using System.Collections.Generic;
using SimpleShooty.Game;
using UnityEngine;

namespace SimpleShooty.Common
{
    public enum GameState
    {
        MainMenu = 0,
        Start,
        Won,
        Lost
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance => instance;
        private static GameManager instance;

        public PlayerController Player { get; private set; }
        public GameState CurrentGameState;

        public Action<GameState> OnStateUpdate;
        private void Awake()
        {
            if (Instance == null && Instance != this)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
        public void SetPlayer(PlayerController player)
        {
            this.Player = player;
        }
        public void UpdateState(GameState state)
        {
            CurrentGameState = state;
            OnStateUpdate?.Invoke(state);
        }
        public bool HasWon()
        {
            return CurrentGameState == GameState.Won;
        }
    }
}