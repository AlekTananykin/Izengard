using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.BuildBuildingsUI
{
    public class BuildingUIInfo : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _type;
        [SerializeField] private TMP_Text _unitsBusy;

        public Image Icon => _icon;
        public TMP_Text Type => _type;
        public TMP_Text UnitsBusy => _unitsBusy;
    }
}