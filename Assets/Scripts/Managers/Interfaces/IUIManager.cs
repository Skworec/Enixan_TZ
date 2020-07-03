using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Assets.Scripts.Managers.Interfaces
{
    public interface IUIManager
    {
        Camera Camera { get; }
        Canvas Canvas { get; }
        EventSystem EventSystem { get; }
        IUIElement CurrentPage { get; set; }

        T GetPopup<T>() where T : IUIPopup;

        void SetPage<T>(bool hideAll = false, object[] data = null) where T : IUIElement;
        void HideUI();
        void DrawPopup<T>(object message = null, bool setMainPriority = false) where T : IUIPopup;
        void HidePopup<T>() where T : IUIPopup;
    }
}