using UnityEngine;

namespace MysterytagTest
{
    public class UIPanelAutoHided : UIPanel
    {
        [SerializeField]
        private float _openedDuration;

        public override void Open()
        {
            base.Open();

            Invoke("Close", _openedDuration);
        }
    }
}
