using System;
using System.Collections.Generic;
using Code.TileSystem;
using Controllers.BuildBuildingsUI;
using ResourceSystem;
using UnityEditor;
using UnityEngine.UI;
using Views.BuildBuildingsUI;
using Object = UnityEngine.Object;

namespace Code.TileSystem
{
    public class TileUIController : IDisposable, IOnController, IOnUpdate
    {
        private TileList _list;
        private TileView _view;
        private TileUIView _uiView;
        private BaseCenterText _centerText;
        private BuildingsUIView _buildingsUIView;
        private BuildGenerator _generator;
        private GlobalResorceStock _stock;
        private List<BuildingConfig> BuildingConfigs;
        private BuildBuildings _buildBuildings;
        private int _currentlvl;
        private int _eightQuantity;
        private int _units;

        public TileConfig Config;
        public TileList List => _list;
        public BaseCenterText CenterText => _centerText;
        public int CurrentLVL => _currentlvl;
        public TileView View => _view;
        public BuildingsUIView BuildingsUIView => _buildingsUIView;
        
        public TileUIController(TileList tileList, TileUIView uiView, BaseCenterText centerText, BuildingsUIView buildingsUIView, 
            BuildGenerator buildGenerator, GlobalResorceStock stock)
        {
            _centerText = centerText;
            _list = tileList;
            _uiView = uiView;
            _generator = buildGenerator;
            _stock = stock;
            _buildingsUIView = buildingsUIView;
            
            OpenMenu(false);
        }

        /// <summary>
        /// Загрузка информации о тайле
        /// </summary>
        public void LoadInfo(TileView view)
        {
            _uiView.Upgrade.onClick.RemoveAllListeners();
            _view = view;
            view.CreateButtonsUIBuy(this);
            _uiView.Upgrade.onClick.AddListener(() => view.LVLUp(this));
            UpdateInfo(view.TileConfig);
            ADDBuildUI(view.CurrBuildingConfigs);
        }
        /// <summary>
        /// Загрузка всей информации на тайл
        /// </summary>
        public void UpdateInfo(TileConfig config)
        {
            _uiView.LvlText.text = config.TileLvl.GetHashCode().ToString() + " LVL";
            _eightQuantity = _view.EightQuantity;
            _units = config.MaxUnits;
            _currentlvl = config.TileLvl.GetHashCode();
            _uiView.UnitMax.text = "0/"+ config.MaxUnits + " Units";
            _uiView.Icon.sprite = config.IconTile;
        }
        public void ADDBuildUI(List<BuildingConfig> configs)
        {
            _buildingsUIView.Deinit();
            BuildingConfigs = configs;
            UpdateBuildings();
            foreach (var kvp in _buildingsUIView.ButtonsInMenu)
            {
                kvp.Value.onClick.AddListener((() => _view.CreateButtonsUIBuy(this)));
            }
        }
        
        public void UpdateBuildings()
        {
            _buildBuildings = new BuildBuildings(BuildingConfigs, _generator);
            _buildingsUIView.Init(BuildingConfigs);
            

            foreach (var kvp in _buildingsUIView.ButtonsInMenu)
            {
                foreach (var config in BuildingConfigs)
                {
                    kvp.Value.onClick.AddListener(() => CheckingForAResource(_stock.GlobalResStock, _centerText, config, kvp.Key, _buildBuildings));
                }
                

            }
        }
        /// <summary>
        /// Проверяет на наличие ресурса если он есть ставим здание.
        /// </summary>
        public void CheckingForAResource(ResurseStock stock, BaseCenterText CenterText, BuildingConfig config, BuildingConfig buildingConfig, BuildBuildings buildings)
        {
            if (config == buildingConfig)
            {
                foreach (var cost in config.BuildingCost)
                {
                    var t = stock.HoldersInStock.Find(x => x.ObjectInHolder.ResourceType == cost.ResourceType);
                    if (t.CurrentValue >= cost.Cost)
                    {
                        t.CurrentValue -= cost.Cost;
                        buildings.BuildBuilding1(buildingConfig);
                        _buildingsUIView.ButtonsBuy.Add(buildingConfig);
                    }
                    else
                    {
                        CenterText.NotificationUI("you do not have enough resources to buy", 1000);
                    }

                
                }
            }
        }

        public void LevelCheck()
        {
            if (CurrentLVL > _buildingsUIView.DestroyButtonsBuy.Count)
            {
                _buildingsUIView.PrefabButtonClear.gameObject.SetActive(true);
            }
            else
            {
                _buildingsUIView.PrefabButtonClear.gameObject.SetActive(false);
            }
        }

        private void OnOpenMenuButton() => OpenMenu(true);

        private void OnCloseMenuButton()
        {
            OpenMenu(false);
        }

        public void OpenMenu(bool isOpen)
        {
            foreach (var window in _buildingsUIView.Windows) window.gameObject.SetActive(isOpen);
            _buildingsUIView.CloseMenuButton.gameObject.SetActive(isOpen);
            if (isOpen == false)
            {
                _buildingsUIView.ClearButtonsUIBuy();
            }
        }
        
        
        #region Other

        /// <summary>
        /// Этот метод для того чтобы взять юнита из тайла
        /// </summary>
        /// <param name="EightQuantity">кол юнитов</param>
        public void hiringUnits(int EightQuantity)
        {
            if (EightQuantity <= _units)
            {
                _eightQuantity += EightQuantity;
                _uiView.UnitMax.text = _eightQuantity.ToString() + "/"+ _units.ToString() + " Units";
            }
            else
            {
                _centerText.NotificationUI("you have hired the maximum number of units", 2000);
            }
            
        }

        /// <summary>
        /// Метод для того чтобы вернуть юнита для найма
        /// </summary>
        /// <param name="EightQuantity">кол юнитов</param>
        public void RemoveFromHiringUnits(int EightQuantity)
        {
            if (_eightQuantity > 0)
            {
                _eightQuantity -= EightQuantity;
                _uiView.UnitMax.text = _eightQuantity.ToString() + "/"+ _units.ToString() + " Units";
            }
        }

        public void Dispose()
        {
            foreach (var kvp in _buildingsUIView.ButtonsInMenu)
                kvp.Value.onClick.RemoveAllListeners();
            _buildingsUIView.CloseMenuButton.onClick.RemoveAllListeners();
            _buildingsUIView.Deinit();
            _uiView.Upgrade.onClick.RemoveAllListeners();
        }

        public void OnUpdate(float deltaTime)
        {
            LevelCheck();
        }

        #endregion
    }
}