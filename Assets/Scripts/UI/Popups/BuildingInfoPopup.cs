using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popups
{
    class BuildingInfoPopup : IUIPopup
    {
        private GameObject _selfPopup;
        private Text _infoText;
        public void Dispose()
        {
        }

        public void Hide()
        {
            _selfPopup.SetActive(false);
        }

        public void Init()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/UI/Popups/BuildingInfoPopup");
            _selfPopup = MonoBehaviour.Instantiate(prefab);
            _selfPopup.transform.SetParent(GameObject.Find("Canvas").transform, false);
            _infoText = _selfPopup.GetComponent<Text>();
            Hide();
        }

        public void SetMainPriority()
        {
        }

        public void Show()
        {
            _selfPopup.SetActive(true);
        }

        public void Show(object data)
        {
            _selfPopup.SetActive(true);
            _infoText.text = (string)data;
        }

        public void Update()
        {
        }
    }
}
