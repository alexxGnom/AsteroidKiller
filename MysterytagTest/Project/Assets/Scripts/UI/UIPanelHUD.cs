using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MysterytagTest
{
    public class UIPanelHUD : UIPanel
    {
        #region Unity properties

        [SerializeField]
        private Text _currLevelScore;

        [SerializeField]
        private Text _totalScore;

        [SerializeField]
        private Text _needScore;

        #endregion

        #region Injects

        [Inject]
        private GameController _gameController;

        [Inject]
        private LevelController _levelController;

        [Inject]
        private AsteroidDamageSignal _damageSignal;

        #endregion

        #region Interface

        public override void Open()
        {
            base.Open();
            _needScore.text = string.Format("Need : {0}", _levelController.LevelData.WinScorePerLevel);
            UpdateScore();

            _damageSignal.Listen(DamageListener);
        }

        public override void Close()
        {
            base.Close();
            _damageSignal.Unlisten(DamageListener);
        }

        public void StopGame()
        {
            _gameController.StopGame();
        }

        #endregion

        #region Utils

        private void UpdateScore()
        {
            _totalScore.text = string.Format("Total Score : {0}", _gameController.Score);
            _currLevelScore.text = string.Format("Score : {0}", _gameController.ScorePerLevel);
        }

        private void DamageListener(int temp)
        {
            UpdateScore();
        }

        #endregion
    }
}
