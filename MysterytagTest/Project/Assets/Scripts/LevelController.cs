using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MysterytagTest
{
    public class LevelController
    {

        #region Public properties

        public LevelData LevelData
        {
            get { return _gameSettings.LevelsData[_level]; }
        }

        public int Level
        {
            get { return _level; }
        }

        public int MaxLevel
        {
            get { return _gameSettings.LevelsData.Length - 1; }
        }

        #endregion

        #region Injects

        [Inject]
        private GameSettings _gameSettings;

        #endregion

        #region Private fields

        private int _level = 0;

        #endregion

        #region Interface

        public void SetNextLevel()
        {
            _level = Mathf.Min(_level + 1, MaxLevel);
        }

        public void Reset()
        {
            _level = 0;
        }

        #endregion

    }
}
