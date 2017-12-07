using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MysterytagTest
{
    public class UIStartLevelPanel : UIPanelAutoHided
    {
        #region Unity properties

        [SerializeField]
        private Text _levelTitle;

        [SerializeField]
        private Text _weaponLevelTitle;

        #endregion

        #region Injects

        [Inject]
        private LevelController _levelController;

        [Inject]
        private WeaponController _weaponController;

        #endregion

        #region Private fields

        private int _lastWeaponLevel = 0;

        #endregion

        #region Interface

        public override void Open()
        {
            base.Open();

            _levelTitle.text = string.Format("Start level: {0}", _levelController.Level + 1);


            if (_weaponController.Level > _lastWeaponLevel)
            {
                _lastWeaponLevel = _weaponController.Level;
                _weaponLevelTitle.text = string.Format("You have new weapon level: {0}", _weaponController.Level + 1);
                _weaponLevelTitle.gameObject.SetActive(true);
            }
            else
            {
                _weaponLevelTitle.gameObject.SetActive(false);
            }
        }

        #endregion
 
    }
}

