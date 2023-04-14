using BuildingSystem;
using System.Collections.Generic;
using Code;
using Code.TileSystem;
using Controllers.BuildBuildingsUI;
using ResourceSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Views.BuildBuildingsUI
{
    public class BuildingsUIView : MonoBehaviour
    {
        [field: SerializeField] public Button BuyDefender { get; private set; } // add Nikolay Vasilev
        [field: SerializeField] public Button CloseMenuButton { get; private set; }
        [field: SerializeField] public Button PrefabButtonClear { get; private set; }
        [field: SerializeField] public Button PrefabButtonInfo { get; private set; }
        [field: SerializeField] public Transform[] Windows { get; private set; }
        [field: SerializeField] public Transform BuildButtonsHolder { get; set; }
        [field: SerializeField] public Transform ByBuildButtonsHolder { get; set; }

        [SerializeField] private Button _buyPrefabButton;
        public Dictionary<BuildingConfig, Button> ButtonsInMenu = new Dictionary<BuildingConfig, Button>();
        public List<BuildingConfig> ButtonsBuy = new List<BuildingConfig>();
        public List<Button> DestroyButtonsBuy = new List<Button>();


        public void Init(List<BuildingConfig> models)
        {
            foreach (var building in models)
            {
                var button = Instantiate(_buyPrefabButton, BuildButtonsHolder);
                ButtonsInMenu.Add(building, button);
                CreateButtonUI(building, button);
            }
        }

        public void CreateBuildingInfo(BuildingConfig config)
        {
            var button = Instantiate(PrefabButtonInfo, ByBuildButtonsHolder);
            var view = button.GetComponent<BuildingUIInfo>();
            view.Icon.sprite = config.Icon;
            view.Type.text = config.BuildingType.ToString();
            view.UnitsBusy.text = "0/5 Units";
            DestroyButtonsBuy.Add(button);

        }

        public void Deinit()
        {
            foreach (var kvp in ButtonsInMenu)
            {
                Destroy(kvp.Value.gameObject);
            }
            ButtonsInMenu.Clear();
            
        }

        public void ClearButtonsUIBuy()
        {
            foreach (var kvp in DestroyButtonsBuy)
            {
                Destroy(kvp.gameObject);
            }
            DestroyButtonsBuy.Clear();
            ButtonsBuy.Clear();
        }

        private void CreateButtonUI(BuildingConfig buildingConfig, Button button)
        {
            var view = button.GetComponent<BuildButtonView>();
            if (view != null) 
            {
                view.BuildingName.text = buildingConfig.BuildingType.ToString();
                foreach (var cost in buildingConfig.BuildingCost)
                {
                    view.CostForBuildingsUI.text += cost.ResourceType + ":" + cost.Cost + " ";
                }
                view.Description.text = buildingConfig.Description;
                view.Icon.sprite = buildingConfig.Icon;
            }
            else
            {
                Debug.LogError("Button field is empty");
            }
        }
    }
}