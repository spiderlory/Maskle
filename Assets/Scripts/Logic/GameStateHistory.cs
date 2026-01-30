using System.Collections.Generic;
using Model;

namespace Logic
{
    public class GameStateHistory
    {
        private Stack<int[,]> _matrixHistory;
        private Stack<Mask> _masksList;

        public GameStateHistory()
        {
            _matrixHistory = new Stack<int[,]>();
            _masksList = new Stack<Mask>();
        }

        public void PushMatrix(int[,] matrix)
        {
            int[,] copyMatrix = matrix.Clone() as int[,];
            _matrixHistory.Push(copyMatrix);
        }

        public void PushMask(Mask mask)
        {
            _masksList.Push(mask);
        }

        public int[,] PopMatrix()
        {
            return _matrixHistory.Pop();
        }

        public Mask PopMask()
        {
            return _masksList.Pop();
        }

        public int[,] PeekMatrix()
        {
            return _matrixHistory.Peek();
        }

        public Mask PeakMask()
        {
            return _masksList.Peek();
        }

        public int GetMatrixCount()
        {
            return _matrixHistory.Count;
        }

        public int GetMasksCount()
        {
            return _masksList.Count;
        }

        public void ClearHistory()
        {
            _matrixHistory.Clear();
            _masksList.Clear();
        }
    }
}