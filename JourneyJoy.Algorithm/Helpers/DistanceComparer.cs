namespace JJAlgorithm.Helpers
{
    public class DistanceComparer : IComparer<int>
    {
        public int[] AdjustmentMatrixRow { get; set; }
        public DistanceComparer(int[] m)
        {
            AdjustmentMatrixRow = m;
        }

        public int Compare(int x, int y) => AdjustmentMatrixRow[x] - AdjustmentMatrixRow[y];
    }
}
