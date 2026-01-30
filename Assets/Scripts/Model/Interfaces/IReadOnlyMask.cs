namespace Model.Interfaces
{
    public interface IReadOnlyMask
    {
        public int this[int i, int j]
        {
            get;
        }

        public int GetApplicationsCount();
        public int[] Flatten();
    }
}