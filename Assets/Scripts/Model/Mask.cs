using Model.Interfaces;

namespace Model
{
    public class Mask : IReadOnlyMask
    {
        // mask parameters
        public int Radius { get; }
        public int NCols { get; }
        public int NRows { get; }

        public int Length { get; }

        private int[,] items;

        private int _applicationCounter;

        public Mask(int radius)
        {
            Radius = radius;
            NCols = radius * 2 + 1;
            NRows = NCols;

            Length = NRows * NCols;

            items = new int[NRows, NCols];
        }

        // Indexer
        public int this[int i, int j]
        {
            get { return items[i, j]; }
            set { items[i, j] = value; }
        }

        public void ApplySum(int[,] matrix, int x, int y, bool subtract)
        {
            int sign = subtract ? -1 : 1;

            // Find relative left corner position
            int iMinMatrix = x - Radius;
            int jMinMatrix = y - Radius;

            // Current Matrix indexes
            int iMatrix;
            int jMatrix;

            for (int i = 0; i < NRows; i++)
            {
                for (int j = 0; j < NCols; j++)
                {
                    iMatrix = iMinMatrix + i;
                    jMatrix = jMinMatrix + j;

                    // Check if index is in range
                    if (iMatrix >= 0 && iMatrix < matrix.GetLength(0) && jMatrix >= 0 && jMatrix < matrix.GetLength(1))
                    {
                        matrix[iMatrix, jMatrix] += items[i, j] * sign;
                    }
                }
            }
            
            _applicationCounter += subtract? -1 : 1;
        }

        public int GetApplicationsCount()
        {
            return _applicationCounter;
        }

        public void UndoApplication()
        {
            _applicationCounter++;
        }

        public int[] Flatten()
        {
            int[] values = new int[9];

            int index = 0;
            for (int i = 0; i < NRows; i++)
            {
                for (int j = 0; j < NCols; j++)
                {
                    values[index] = -items[i, j];
                    index++;
                }
            }
        
            return values;
        }

        public override string ToString()
        {
            string s = "";
            for (int x = 0; x < NCols; x++)
            {
                for (int y = 0; y < NRows; y++)
                {
                    s += "[" + items[x, y] + "] ";
                }

                s += '\n';
            }

            return s;
        }
    }
}