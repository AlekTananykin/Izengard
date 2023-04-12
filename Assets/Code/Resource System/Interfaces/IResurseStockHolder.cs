using ResourceSystem;

namespace BuildingSystem
{ 
    public interface IResurseStockHolder
    {
        public ResourceStock ThisBuildingStock { get; }

        public void AddResurseInStock(IResurseHolder holder);

        public IResurseHolder GetResursesInStock(IResurse resurse, float value);
    }
}
