using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Managers.Interfaces;
using TMPro;

public class ShopItemView
{
    private static GameObject _prefab;

    private Image _itemImage;
    private Text _itemName;
    private Text _itemDescription;
    private Button _itemButton;
    private Button _descriptionButton;
    private Button _descriptionOutButton;

    public Transform transform;

    private IUIManager _uiManager;
    private ISettingsManager _settingsManager;
    private IBuildManager _buildManager;
    private Building _scriptObj;

    public ShopItemView(Transform content, Building scriptObj)
    {
        if (_prefab == null)
            _prefab = Resources.Load<GameObject>("Prefabs/UI/ShopItem");

        _uiManager = GameClient.Get<IUIManager>();
        _settingsManager = GameClient.Get<ISettingsManager>();
        _buildManager = GameClient.Get<IBuildManager>();

        transform = MonoBehaviour.Instantiate(_prefab, content).transform;
        _scriptObj = scriptObj;

        _itemImage = transform.Find("ItemImage").GetComponent<Image>();
        _itemName = transform.Find("ItemName").GetComponent<Text>();
        _itemDescription = transform.Find("DescriptionText").GetComponent<Text>();
        _itemButton = transform.Find("ItemImage").GetComponent<Button>();
        _descriptionButton = transform.Find("DescriptionButton").GetComponent<Button>();
        _descriptionOutButton = transform.Find("DescriptionText").GetComponent<Button>();

        _itemButton.onClick.AddListener(OnItemClickHandler);
        _descriptionButton.onClick.AddListener(DescriptionOnClickHandler);
        _descriptionOutButton.onClick.AddListener(DescriptionOutOnClickHandler);

        _itemImage.sprite = scriptObj.Icon;
        _itemName.text = scriptObj.BuildingName;
        _itemDescription.text = scriptObj.Description;
    }

    private void OnItemClickHandler()
    {
        _uiManager.SetPage<GamePage>(true);
        int center = (int)Mathf.Round(_settingsManager.PlaneSize / 2);
        Vector2 point = new Vector2(center, center);
        _buildManager.BuildABuilding(point, _scriptObj);
    }
    private void DescriptionOnClickHandler()
    {
        _itemDescription.gameObject.SetActive(true);
        _itemImage.gameObject.SetActive(false);
        _itemName.gameObject.SetActive(false);
        _descriptionButton.gameObject.SetActive(false);
    }
    private void DescriptionOutOnClickHandler()
    {
        _itemImage.gameObject.SetActive(true);
        _itemName.gameObject.SetActive(true);
        _descriptionButton.gameObject.SetActive(true);
        _itemDescription.gameObject.SetActive(false);
    }
}
