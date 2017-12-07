using UnityEngine;

namespace MysterytagTest
{
    public class UIPanel : MonoBehaviour
    {
        #region Unity properties

        [SerializeField]
        private string _id;

        #endregion

        #region Public properties

        public string Id { get { return _id; }}

        public bool IsOpened { get; protected set; }

        #endregion

        #region Interface

        public virtual  void Open()
        {
            gameObject.SetActive(true);
            IsOpened = true;
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
            IsOpened = false;
        }

        #endregion
    }
}
