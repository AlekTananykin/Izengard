using System.Collections.Generic;
using UnityEngine;
using ResurseSystem;
using System;
using Data;

namespace BuildingSystem
{ 
    [System.Serializable]
    [CreateAssetMenu(fileName = "Global Buildings Models ", menuName = "Buildings/Global Buildings Models", order = 1)]
    public class GlobalBuildingsModels : ScriptableObject
    {
        #region ��������� ����� ������
        [Header("Main Building Configs")]
        [SerializeField]
        private MainBuildingModel _mainBuildingModelBase;
        [Header("Warehouse building Configs")]
        [SerializeField]
        private WareHouseBuildModel _warehouseBuildingModelBase;
        [Header("Resurse Produce Buildings Configs")]
        [SerializeField]
        private List<ResurseProduceBuildingModel> _resurseProduceBuildingModelsBase;
        [Header("Item Produce Buildings Configs")]
        [SerializeField]
        private List<ProduceItemBuildingModel> _itemProduceBuildingModelsBase;
        [Header("Resurse Market Buildings Configs")]
        [SerializeField]
        private List<ResurseMarkeBuildingModel> _resurseMarketBuildingModelsBase;
        [Header("Item Market Building Configs")]
        [SerializeField]
        private List<ItemMarketBuildingModel> _itemsMarketBuildingModelsBase;
        [Header("House Configs")]
        [SerializeField]
        private HouseBuildingModel _house;
        #endregion

        #region ���� ����������� ������ ������
        
        private List<BuildingModel> _ActiveBuildings;
        private List<BuildingModel> _NeedResursesBuildings;
        private List<BuildingModel> _BuildingsUnderConstraction;        
        private List<WareHouseBuildModel> _StockBuildings;        
        private List<ResurseProduceBuildingModel> _ProduceResurseBuildings;
        private List<ProduceItemBuildingModel> _ProduceItemBuildings;
        private List<MarketBuildingModel<ScriptableObject>> _MarketsList;
        private List<IProduceWorkers> _ProduceWorkerBuildings;        
        private int _globalMaxWorkersValue;
        [Header("Outpost Parameters")]
        [SerializeField]
        private OutpostParametersData _outpostParametersData;

        public Action<int> ChangeMaxWorkerValue;
        public Action<WareHouseBuildModel> StockBorn;
        #endregion
        public GlobalBuildingsModels()
        {
            _ActiveBuildings = new List<BuildingModel>();            
            _NeedResursesBuildings = new List<BuildingModel>();
            _BuildingsUnderConstraction = new List<BuildingModel>();
            _ProduceResurseBuildings = new List<ResurseProduceBuildingModel>();
            _ProduceItemBuildings = new List<ProduceItemBuildingModel>();
            _StockBuildings = new List<WareHouseBuildModel>();
            _ProduceWorkerBuildings = new List<IProduceWorkers>();
            _MarketsList = new List<MarketBuildingModel<ScriptableObject>>();
            _globalMaxWorkersValue = 0;
            ChangeMaxWorkerValue?.Invoke(_globalMaxWorkersValue);

        }
        #region ������ � �����
        public List<BuildingModel> GetActiveBuildings()
        {
            return _ActiveBuildings;
        }
        public List<BuildingModel> GetBuildingsUnderConstraction()
        {
            return _BuildingsUnderConstraction;
        }
        public List<BuildingModel> GetNeedResursesBuildings()
        {
            return _NeedResursesBuildings;
        }
        public List<ResurseProduceBuildingModel> GetProduceResurseBuildings()
        {
            return _ProduceResurseBuildings;
        }
        public List<ProduceItemBuildingModel> GetProduceItemBuildings()
        {
            return _ProduceItemBuildings;
        }
        public List<IProduceWorkers> GetAllHouse()
        {
            return _ProduceWorkerBuildings;
        }
        public List<MarketBuildingModel<ScriptableObject>> GetAllMarkets()
        {
            return _MarketsList;
        }
        public int GetMaxWorkerCount()
        {
            return _globalMaxWorkersValue;
        }
        #endregion
        #region Set base model to building metods

        /// <summary>
        /// Set building model to current building
        /// </summary>
        /// <param name="type">type of building</param>
        /// <param name="model">model in building</param>
        public BuildingModel GetModelToBuilding(TypeOfBuildings type)
        {

            switch (type)
            {
                case TypeOfBuildings.House:
                    return new HouseBuildingModel(_house);
                case TypeOfBuildings.TextileFabrick:
                case TypeOfBuildings.RuneWorkshop:
                case TypeOfBuildings.Stable :
                case TypeOfBuildings.Foundry:
                    var tempResProdModel = FindResurseProduceBuildingModel(type);
                    if (tempResProdModel != null)
                    {
                        return new ResurseProduceBuildingModel(tempResProdModel);
                    }
                    return null;
                case TypeOfBuildings.MainBuilding:
                    return new MainBuildingModel(_mainBuildingModelBase);
                    
                case TypeOfBuildings.Warehouse:
                    return new WareHouseBuildModel(_warehouseBuildingModelBase);
                    
                case TypeOfBuildings.Forge | TypeOfBuildings.ArmorForge | TypeOfBuildings.WeaponForge:
                    var tempItemProdModel = FindItemProduceBuildingModel(type);
                    if (tempItemProdModel != null)
                    {
                        return new ProduceItemBuildingModel(tempItemProdModel);
                    }
                    return null;
                case TypeOfBuildings.TrainingField:
                    return null;
                case TypeOfBuildings.ItemMarket:
                    var itemMarketModel = FindItemMarketBuildingModel(type);
                    if (itemMarketModel != null)
                    {
                        return new ItemMarketBuildingModel(itemMarketModel);
                    }
                    return null;
                case TypeOfBuildings.ResurseMarket:
                    var resMarketModel = FindResurseMarketBuildingModel(type);
                    if (resMarketModel != null)
                    {
                        return new ResurseMarkeBuildingModel(resMarketModel);
                    }
                    return null;
                default:
                    return null;

            }
        }
        /// <summary>
        /// Find and return needed base building model from List of Produce resurse building models
        /// </summary>
        /// <param name="type">type of building</param>
        /// <returns></returns>
        public ResurseProduceBuildingModel FindResurseProduceBuildingModel(TypeOfBuildings type)
        {
            foreach (ResurseProduceBuildingModel model in _resurseProduceBuildingModelsBase)
            {
                if (model.BuildingTypeID == (int)type)
                {
                    return model;
                }
            }
            return null;
        }
        /// <summary>
        /// Find and return needed base building model from List of Produce items building models
        /// </summary>
        /// <param name="type">type of building</param>
        /// <returns></returns>
        public ProduceItemBuildingModel FindItemProduceBuildingModel(TypeOfBuildings type)
        {
            foreach (ProduceItemBuildingModel model in _itemProduceBuildingModelsBase)
            {
                if (model.BuildingTypeID == (int)type)
                {
                    return model;
                }
            }
            return null;
        }
        /// <summary>
        /// Find and return needed base building model from List of items market building models
        /// </summary>
        /// <param name="type">type of building</param>
        /// <returns></returns>
        public ItemMarketBuildingModel FindItemMarketBuildingModel(TypeOfBuildings type)
        {
            foreach (ItemMarketBuildingModel model in _itemsMarketBuildingModelsBase)
            {
                if (model.BuildingTypeID == (int)type)
                {
                    return model;
                }
            }
            return null;
        }
        /// <summary>
        /// Find and return needed base building model from List of resurse market building models
        /// </summary>
        /// <param name="type">type of building</param>
        /// <returns></returns>
        public ResurseMarkeBuildingModel FindResurseMarketBuildingModel(TypeOfBuildings type)
        {
            foreach (ResurseMarkeBuildingModel model in _resurseMarketBuildingModelsBase)
            {
                if (model.BuildingTypeID == (int)type)
                {
                    return model;
                }
            }
            return null;
        }
        #endregion
        #region ������ �������������� ������ ������

        /// <summary>
        /// ����� ��� ������� �������� �� ����������� ����� ��� �������������
        /// </summary>
        /// <param name="globalStock"></param>
        public void AddNeedForBuildResurse(GlobalResurseStock globalStock)
        {
            if (_NeedResursesBuildings!=null)
            { 
            foreach (BuildingModel model in _NeedResursesBuildings)
            {
                model.ThisBuildingCost.GetNeededResurse(globalStock.GlobalResStock);
            }
            CheckBuildingsCostPaid();
            }
        }
        public void AddBuildingInNeedResurseForBuildingList(BuildingModel building)
        {
            if (building.ThisBuildingCost.PricePaidFlag)
            {
                _ActiveBuildings.Add(building);
                CheckBuildingsModel(building);
            }
            else
            { 
                _NeedResursesBuildings.Add(building);
            }
        }
        /// <summary>
        /// ����� �������� ������ �� ����������� � �������� ��� ������������� � ���������� � ������ �� �������������
        /// </summary>
        /// <param name="building"></param>
        public void BuildingCostPaid(BuildingModel building)
        {
            _NeedResursesBuildings.Remove(building);
            StartBuildBuilding(building);
        }
        
        /// <summary>
        /// ����� ���������� ������ � ���� ���������� (� ������ ��� �������� ������ ���������� ������������� ������ �� ����� ���������� ������������� ������) 
        /// </summary>
        /// <param name="building"></param>
        public void StartBuildBuilding(BuildingModel building)
        {
            _BuildingsUnderConstraction.Add(building);
            building.ABuildingComplete += CheckBuildsComplete;
        }
      
        /// <summary>
        /// ����� �������� ������ �� ����������, ���������� � ������ �������� ������, �������� � ���� ����� �������� ������ ������
        /// </summary>
        /// <param name="building"></param>
        public void BuildingComplete(BuildingModel building)
        {
            _BuildingsUnderConstraction.Remove(building);            
            _ActiveBuildings.Add(building);
            CheckBuildingsModel(building);
        }
        
        /// <summary>
        /// ����� �������� ������ �� ��������, ��� �� �������� ����� �������� ������ � ������ ������
        /// </summary>
        /// <param name="building"></param>
        public void BuildingDestroy(BuildingModel building)
        {
            _ActiveBuildings.Remove(building);
            DeleteBuildingsModel(building);
        }
       
        /// <summary>
        /// ����� �������� ��������� ���������� ������������� ������ �� ������ ����������� � ��������.
        /// </summary>
        public void CheckBuildingsCostPaid()
        {
            foreach (BuildingModel building in _NeedResursesBuildings)
            {
                if (building.ThisBuildingCost.PricePaidFlag)
                {
                    BuildingCostPaid(building);                    
                }
            }
        }
        
        /// <summary>
        /// ����� ������� �� ������ ���������� ������������� ������, ������� ���������. �������� ����� ���������� ������ � ��������
        /// </summary>
        /// <param name="buildmodel"></param>
        public void CheckBuildsComplete(BuildingModel buildmodel)
        {
            
            buildmodel.ABuildingComplete -= CheckBuildsComplete;
            BuildingComplete(buildmodel);
                   
            
        }
        
        /// <summary>
        /// ����� �������� ������ ������ � ������������� �� �������� ������ ������ �� ���������������
        /// </summary>
        /// <param name="building"></param>
        public void CheckBuildingsModel(BuildingModel building)
        {            
            if (building is ProduceItemBuildingModel)
            {
                _ProduceItemBuildings.Add((ProduceItemBuildingModel)building);               
            }
            if (building is ResurseProduceBuildingModel)
            {
                _ProduceResurseBuildings.Add((ResurseProduceBuildingModel)building);                
            }
            if (building is WareHouseBuildModel)
            {
                var tempbuilding = (WareHouseBuildModel)building;
                _StockBuildings.Add(tempbuilding);
                StockBorn?.Invoke(tempbuilding);

            }
            if (building is IProduceWorkers)
            {
               
                _ProduceWorkerBuildings.Add((IProduceWorkers)building);
                _globalMaxWorkersValue += ((IProduceWorkers)building).CurrentWorkerValue;
                ChangeMaxWorkerValue?.Invoke(_globalMaxWorkersValue);
                //temporary solution (plug)
                _outpostParametersData.SetMaxCountOfWorkers(_globalMaxWorkersValue);
            }
            if (building is MarketBuildingModel<ScriptableObject>)
            {
                _MarketsList.Remove((MarketBuildingModel<ScriptableObject>)building);
            }

        }
        
        /// <summary>
        /// ����� �������� ������ �� �������� ������ �� ��������������
        /// </summary>
        /// <param name="building"></param>
        public void DeleteBuildingsModel(BuildingModel building)
        {
            
            if (building is ProduceItemBuildingModel)
            {
                _ProduceItemBuildings.Remove((ProduceItemBuildingModel)building);                
            }
            if (building is ResurseProduceBuildingModel)
            {
                _ProduceResurseBuildings.Remove((ResurseProduceBuildingModel)building);
            }
            if (building is WareHouseBuildModel)
            {
                _StockBuildings.Remove((WareHouseBuildModel)building);
            }
            if (building is IProduceWorkers)
            {
                
                _ProduceWorkerBuildings.Remove((IProduceWorkers)building);
                _globalMaxWorkersValue -= ((IProduceWorkers)building).CurrentWorkerValue;
                ChangeMaxWorkerValue?.Invoke(_globalMaxWorkersValue);
            }
            if (building is MarketBuildingModel<ScriptableObject>)
            {
                _MarketsList.Remove((MarketBuildingModel<ScriptableObject>)building);
            }
        }
        
        /// <summary>
        /// ����� ������� � ��������� ������������� ���������� ���������� ������
        /// </summary>
        /// <returns></returns>
        public int GetGlobalMaxWorkerCount()
        {
            return _globalMaxWorkersValue;
        }
        /// <summary>
        /// ����� �������\����������� ������������ ���������
        /// </summary>
        /// <param name="Time">�����</param>
        public void StartGlobalProducing(float time,GlobalResurseStock stock)
        {
            if (_ProduceItemBuildings!=null&& _ProduceItemBuildings.Count>0)
            {
                foreach(ProduceItemBuildingModel building in _ProduceItemBuildings)
                {
                    building.StartProduce(time);
                    building.GetPaidForProducts(stock);
                }
            }
            if (_ProduceResurseBuildings != null && _ProduceResurseBuildings.Count > 0)
            {
                foreach (ResurseProduceBuildingModel building in _ProduceResurseBuildings)
                {
                    building.StartProduce(time);
                    building.GetPaidForProducts(stock);
                }
            }            
        }
       /// <summary>
       /// Starts all current builds process
       /// </summary>
       /// <param name="time"></param>
        public void StartGlobalBuild(float time)
        {
            foreach(BuildingModel building in _BuildingsUnderConstraction)
            {
                building.StartBuilding(time);
            }
        }
        #endregion
        
        /// <summary>
        /// ����� "������" ���������� ������ ��� ������ ����
        /// </summary>
        public void ResetGlobalBuildingModel()
        {
            _ActiveBuildings = new List<BuildingModel>();
            _NeedResursesBuildings = new List<BuildingModel>();
            _BuildingsUnderConstraction = new List<BuildingModel>();
            _ProduceResurseBuildings = new List<ResurseProduceBuildingModel>();
            _ProduceItemBuildings = new List<ProduceItemBuildingModel>();
            _ProduceWorkerBuildings = new List<IProduceWorkers>();
            _StockBuildings = new List<WareHouseBuildModel>();
            _MarketsList = new List<MarketBuildingModel<ScriptableObject>>();
            _globalMaxWorkersValue = 0;
            ChangeMaxWorkerValue?.Invoke(_globalMaxWorkersValue);
        }
    }
}
