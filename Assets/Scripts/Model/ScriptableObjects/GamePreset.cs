using UnityEngine;

namespace Model
{

    [CreateAssetMenu(fileName = "GamePreset", menuName = "Scriptable Objects/GamePreset")]
    public class GamePreset : ScriptableObject
    {
        [Min(0)]
        public int rows = 3;
        [Min(0)]
        public int cols = 3;
        
        public int nMasks = 3;
        public int nMasksApplication = 3;
        
        public int maskRadius = 1;
        public int min = -1;
        public int max = 1;
        public int count = 4;
    }
}