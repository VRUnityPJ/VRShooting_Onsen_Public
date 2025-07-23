namespace Onogawa.Scripts.Point.interfaces
{
    public interface IPointHolder
    {
        public int Point { get; }
        public int RestFireflyPoint { get; }

        public void InitializePoint();
        public void AddRestFireflyPoint(int fireflyCount);
        public void AddPoint(int value);
        public void SubPoint(int value);
        public int GetTotalPoint();
        public Rank GetRank();
    }
}