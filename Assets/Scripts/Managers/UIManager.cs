using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Interfaces;
using Assets.Scripts.UI.Popups;

namespace Assets.Scripts.Managers
{
    public class UIManager : IUIManager, ISystem
    {
        private List<IUIElement> _uiPages = new List<IUIElement>();
        private List<IUIPopup> _uiPopups = new List<IUIPopup>();

        public Camera Camera
        {
            get
            {
                return _camera;
            }
        }

        public Canvas Canvas
        {
            get
            {
                return _canvas;
            }
        }

        public EventSystem EventSystem
        {
            get
            {
                return _eventSystem;
            }
        }

        public IUIElement CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
            }
        }

        private IUIElement _currentPage;
        private Camera _camera;
        private Canvas _canvas;
        private EventSystem _eventSystem;

        public void Dispose()
        {
            foreach (var page in _uiPages)
            {
                page.Dispose();
            }

            foreach (var popup in _uiPopups)
            {
                popup.Dispose();
            }
        }

        public void Init()
        {
            _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

            _uiPages.Add(new ShopPage());
            _uiPages.Add(new GamePage());

            foreach (var page in _uiPages)
            {
                page.Init();
            }

            _uiPopups.Add(new BuildingInfoPopup());

            foreach (var popup in _uiPopups)
            {
                popup.Init();
            }
        }

        public void Update()
        {
            foreach (var page in _uiPages)
                page.Update();
            foreach (var popup in _uiPopups)
                popup.Update();
        }

        public void SetPage<T>(bool hideAll = false, object[] data = null) where T : IUIElement
        {
            if (hideAll)
            {
                foreach (var _page in _uiPages)
                {
                    _page.Hide();
                }
            }
            else
            {
                if (CurrentPage != null)
                    CurrentPage.Hide();
            }

            foreach (var _page in _uiPages)
            {
                if (_page is T)
                {
                    CurrentPage = _page;
                    break;
                }
            }

            if (CurrentPage != null)
                CurrentPage.Show(data);
            else
                Debug.LogError("CurrentPage == null");
        }

        public void HideUI()
        {
            foreach (var _page in _uiPages)
            {
                _page.Hide();
            }
        }

        public T GetPopup<T>() where T : IUIPopup
        {
            return (T)_uiPopups.Find(item => item is T);
        }

        public void DrawPopup<T>(object message = null, bool setMainPriority = false) where T : IUIPopup
        {
            IUIPopup popup = null;
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    popup = _popup;
                    break;
                }
            }

            if (setMainPriority)
                popup.SetMainPriority();

            if (message == null)
            {
                popup.Show();
            }
            else
            {
                popup.Show(message);
            }
        }

        public void HidePopup<T>() where T : IUIPopup
        {
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    _popup.Hide();
                    break;
                }
            }
        }
    }
}