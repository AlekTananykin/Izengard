using BuildingSystem;
using EquipmentSystem;
using ResourceSystem;
using UnityEngine;

public class WorkerView : UnitView
{
    [SerializeField]
    private ResourceHolder _Resholder;
    [SerializeField]
    private ItemСarrierHolder _Itemholder;
    [SerializeField]
    
    private float _currentMineTime;

    public void GetResurseOutOfHolder(WareHouseBuildModel model)
    {
        model.AddInStock(_Resholder);
    }
    public void GetItemOutOfHolder (WareHouseBuildModel model)
    {
        model.AddInStock(_Itemholder);
    }
    public void MineResurse (ResourceMine mine,float time)
    {
        _currentMineTime += time;
        if (_currentMineTime>=mine.ExtractionTime)
        {
            _Resholder = mine.MineResource();
            _currentMineTime = 0;
        }
    }
    public void GiveMeItem(BuildingModel building)
    {

    }
}
