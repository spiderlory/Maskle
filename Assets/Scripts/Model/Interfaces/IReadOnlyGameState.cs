using System.Collections.Generic;

namespace Model.Interfaces
{
    public interface IReadOnlyGameState
    {
        public int GetMaxScore();
        public int GetCurrentScore();
        public int GetCurrentRound();
        public int GetTimeLeft();

        public int[,] GetCurrentMatrix();
        public int GetMatrixValueAt(int index);
        
        public List<IReadOnlyMask> GetMasksList();
        public IReadOnlyMask GetMaskById(int maskId);

        public IReadOnlyMask GetActiveMask();
        public int GetActiveMaskIndex();
        
    }
}