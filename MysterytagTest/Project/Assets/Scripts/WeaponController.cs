using Zenject;

namespace MysterytagTest
{
    public class WeaponController
    {
        #region Public properties

        public int Level
        {
            get { return _level; }
        }

        public WeaponData Weapon
        {
            get { return _gameSettings.WeaponsData[_level]; }
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

        public void Reset()
        {
            _level = 0;
        }

        public void CheckWeaponUpdate(int score)
        {
            if (score >= Weapon.ScoreToUpdate)
            {
                _level++;
            }
        }

        #endregion
    }
}
