using System.Collections;
using UnityEngine;
using Zenject;

namespace MysterytagTest
{
    public class GameController : MonoBehaviour
    {
        #region Public properties

        public bool IsStarted { get; private set; }

        public int Score { get; private set; }

        public int ScorePerLevel { get; private set; }

        #endregion

        #region Injects

        [Inject]
        private Player.PlayerFactory _playerFactory;

        [Inject]
        private Asteroid.Pool _asteroidPool;

        [Inject]
        private GameSettings _gameSettings;

        [Inject]
        private AsteroidGenerator _asteroidGenerator;

        [Inject]
        private LevelController _levelController;

        [Inject]
        private AsteroidDamageSignal _damageSignal;

        [Inject]
        private UIController _uiController;

        [Inject]
        private WeaponController _weaponController;

        #endregion

        #region Private fields

        private Player _player;

        private UIPanel _gameHUDPanel;
        private UIPanel _gameStartPanel;

        #endregion

        #region Interface

        public void PlayGame()
        {
            ResetGame();
            StartCoroutine(PlayLevelRoutine());
        }

        public void StopGame()
        {
            EndLevel();

            if (_gameStartPanel != null)
                _gameStartPanel.Open();

        }

        public void SetEndLevel()
        {
            EndLevel();

            if (ScorePerLevel < _levelController.LevelData.WinScorePerLevel)
            {
                StartCoroutine(LooseGameRoutine());
            }
            else if (_levelController.Level == _levelController.MaxLevel)
            {
                StartCoroutine(WinGameRoutine());
            }
            else
            {
                StartCoroutine(NextLevelPlayRoutine());
            }
        }

        #endregion

        #region Utils

        private void Start()
        {

            IsStarted = true;

            _damageSignal.Listen(OnAsteroidDamage);

            CreatePlayer();

            _gameHUDPanel = _uiController.GetPanelById("HUDPanel");

            _gameStartPanel = _uiController.GetPanelById("StartGamePanel");
        }

        private void OnDestroy()
        {
            _damageSignal.Unlisten(OnAsteroidDamage);
        }

        private void CreatePlayer()
        {
           _player =  _playerFactory.Create(_gameSettings.PlayerSpeed);
           _player.Hide();
        }

        private void OnAsteroidDamage(int damage)
        {
            Score += damage;
            ScorePerLevel += damage;
           
            CheckLevelComplete();
        }

        private void ResetGame()
        {
            StopAllCoroutines();
            _levelController.Reset();
            _weaponController.Reset();
            Score = 0;
            ScorePerLevel = 0;
        }

        private void EndLevel()
        {
            IsStarted = false;
            _player.Hide();
            _asteroidGenerator.StopGenerate();
            _asteroidGenerator.DespawnAll();

            if (_gameHUDPanel != null)
                _gameHUDPanel.Close();
        }

        private void CheckLevelComplete()
        {
            if (ScorePerLevel >= _levelController.LevelData.WinScorePerLevel)
            {
                SetEndLevel();
            }
        }

        private IEnumerator PlayLevelRoutine()
        {
            ScorePerLevel = 0;
            _weaponController.CheckWeaponUpdate(Score);

            var startLevelPanel = _uiController.GetPanelById("StartLevelPanel");

            if (startLevelPanel != null)
            {
                startLevelPanel.Open();

                while (startLevelPanel.IsOpened) yield return null;
            }

            if (_gameHUDPanel != null)
                _gameHUDPanel.Open();

            IsStarted = true;
            _player.Show();
            _asteroidGenerator.StartGenerate();

        }

        private IEnumerator WinGameRoutine()
        {
            var winPanel = _uiController.GetPanelById("WonPanel");

            if (winPanel != null)
            {
                winPanel.Open();

                while (winPanel.IsOpened) yield return null;
                
            }

            if (_gameStartPanel != null)
                _gameStartPanel.Open();
        }

        private IEnumerator LooseGameRoutine()
        {
            var loosePanel = _uiController.GetPanelById("LoosePanel");

            if (loosePanel != null)
            {
                loosePanel.Open();

                while (loosePanel.IsOpened) yield return null;
            }

            ResetGame();

            if (_gameStartPanel != null)
                _gameStartPanel.Open();
        }

        private IEnumerator NextLevelPlayRoutine()
        {
            var panel = _uiController.GetPanelById("WonLevelPanel");

            if (panel != null)
            {
                panel.Open();

                while (panel.IsOpened) yield return null;

                yield return new WaitForSeconds(0.5f);
            }
            _levelController.SetNextLevel();
             yield return StartCoroutine(PlayLevelRoutine());
        }

        #endregion
    }
}
