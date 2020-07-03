using Assets.Scripts.Managers.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePage : IUIElement
{
    private string ITEMS_PATH = Application.dataPath + "Resources/ScriptableObjects/Environment";

    private GameObject _selfPage;
    private Button _buttonShop,
                   _buttonGrid;

    private bool isActive;

    private IUIManager _uiManager;
    private ICameraManager _cameraManager;

    public void Dispose()
    {
    }

    public void Hide()
    {
        isActive = false;
        _selfPage.SetActive(false);
        _cameraManager.IsMoved = false;
        _cameraManager.IsRaycastable = false;
    }

    public void Init()
    {
        _uiManager = GameClient.Get<IUIManager>();
        _cameraManager = GameClient.Get<ICameraManager>();
        var prefab = Resources.Load<GameObject>("Prefabs/UI/Pages/GamePage");
        _selfPage = MonoBehaviour.Instantiate(prefab) as GameObject;
        _selfPage.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _buttonGrid = _selfPage.transform.Find("ButtonGrid").GetComponent<Button>();
        _buttonShop = _selfPage.transform.Find("ButtonShop").GetComponent<Button>();
        _buttonGrid.onClick.AddListener(GridOnClickHandler);
        _buttonShop.onClick.AddListener(ShopOnClickHandler);
    }

    public void Show(object[] data)
    {
        _selfPage.SetActive(true);
        isActive = true;
        _cameraManager.IsMoved = true;
        _cameraManager.IsRaycastable = true;
    }

    public void Update()
    {
    }

    private void GridOnClickHandler()
    {
        _uiManager.Camera.GetComponent<gridOverlay>().FlipBoolShowGrid();
    }

    private void ShopOnClickHandler()
    {
        _uiManager.SetPage<ShopPage>(true);
    }
}
