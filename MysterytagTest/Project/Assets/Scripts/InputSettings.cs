using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MysterytagTest
{
    [CreateAssetMenu(fileName = "InputSettings", menuName = "Create InpSettings")]
    public class InputSettings : ScriptableObject
    {

        public KeyCode UpMoveBttn = KeyCode.UpArrow;
        public KeyCode DownMoveBttn = KeyCode.DownArrow;
        public KeyCode RightMoveBttn = KeyCode.RightArrow;
        public KeyCode LeftMoveBttn = KeyCode.LeftArrow;

        public KeyCode FireBttn = KeyCode.Space;
    }
}
