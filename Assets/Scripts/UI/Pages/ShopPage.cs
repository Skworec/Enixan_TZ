using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Managers.Interfaces;

public class ShopPage : IUIElement
{
    private string ITEMS_PATH = Application.dataPath + "/Resources/ScriptableObjects/Environment";

    private GameObject _selfPage;
    private Button _buttonClose;
    private Text _textDescription;
    private GameObject _contentPrefab;
    private Transform _content;
    private List<ShopItemView> shopItemViews;

    private IUIManager _uiManager;
    private ICameraManager _cameraManager;

    public void Dispose()
    {
        shopItemViews.Clear();
    }

    public void Hide()
    {
        _selfPage.SetActive(false);
    }

    public void Init()
    {
        shopItemViews = new List<ShopItemView>();

        _uiManager = GameClient.Get<IUIManager>();
        _cameraManager = GameClient.Get<ICameraManager>();
        var prefab = Resources.Load<GameObject>("Prefabs/UI/Pages/ShopPage");
        _contentPrefab = Resources.Load<GameObject>("Prefabs/UI/ShopUnit");
        _selfPage = MonoBehaviour.Instantiate(prefab);
        _selfPage.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _textDescription = _selfPage.transform.Find("Header/Text").GetComponent<Text>();
        _buttonClose = _selfPage.transform.Find("ButtonClose").GetComponent<Button>();
        _content = _selfPage.transform.Find("Scroll View/Content");
        _buttonClose.onClick.AddListener(CloseOnClickHandler);
        FillContent(ReadItems());
        Hide();
    }

    public void Show(object[] data)
    {
        _selfPage.SetActive(true);
    }

    public Building[] ReadItems()
    {
        int itemCount;
        Building[] shopItemModels = new Building[0]; 
        string[] itemsPathsStr = Directory.GetFiles(ITEMS_PATH);
        List<string> itemsPaths = new List<string>();
        if (itemsPathsStr.Length != 0)
        {
            itemCount = itemsPathsStr.Length;
            string substr = Application.dataPath + "/Resources/";
            for (int i = 0; i < itemCount; i++)
            {
                if (itemsPathsStr[i].Contains(".meta"))
                    continue;
                int n = itemsPathsStr[i].IndexOf(substr);
                itemsPaths.Add(itemsPathsStr[i].Remove(n, substr.Length).Replace("\\", "/").Replace(".asset", ""));
            }
            itemCount = itemsPaths.Count;
            shopItemModels = new Building[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                shopItemModels[i] = Resources.Load<Building>(itemsPaths[i]);
            }
        }
        return shopItemModels;
    }

    public void FillContent(Building[] buildings)
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            shopItemViews.Add(new ShopItemView(_content, buildings[i]));
        }
    }

    public void Update()
    {
    }

    private void CloseOnClickHandler()
    {
        _uiManager.SetPage<GamePage>(true);
    }
}
