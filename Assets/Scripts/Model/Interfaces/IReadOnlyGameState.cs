using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;

namespace Model.Interfaces
{
    public interface IReadOnlyGameState
    {
        public int GetMaxScore();
        public int GetCurrentScore();
        public int GetCurrentRound();
        public float GetTimeLeft();

        public int[,] GetCurrentMatrix();
        public int GetMatrixValueAt(int index);
        
        public List<IReadOnlyMask> GetMasksList();
        public IReadOnlyMask GetMaskById(int maskId);

        public IReadOnlyMask GetActiveMask();
        public int GetActiveMaskIndex();
        
    }
}