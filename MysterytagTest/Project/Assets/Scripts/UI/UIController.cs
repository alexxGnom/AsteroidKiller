using UnityEngine;

namespace MysterytagTest
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private UIPanel[] _cashedPanels;

        private void Start()
        {
            GetPanelById("StartGamePanel").Open();
        }

        public UIPanel GetPanelById(string Id)
        {
            foreach (var panel in _cashedPanels)
            {
                if (panel.Id == Id)
                    return panel;
            }
            return null;
        }
    }
}
