using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Assets.Scripts.Managers.Interfaces;

namespace Assets.Scripts.Managers
{
    public class PlaneManager : IPlaneManager, ISystem
    {
        private GameObject _plane;
        private List<List<Tile>> _tiles;

        public GameObject Plane
        {
            get
            {
                return _plane;
            }
            set
            {
                _plane = value;
            }
        }
        public List<List<Tile>> Tiles
        {
            get
            {
                return _tiles;
            }
            set
            {
                _tiles = value;
            }
        }

        public GameObject GrassPrefab;
        [SerializeField]
        public List<Building> EnvironmentObjects;

        private ISettingsManager _dataManager;
        private IUIManager _uiManager;


        public void Init()
        {
            _dataManager = GameClient.Get<ISettingsManager>();
            _uiManager = GameClient.Get<IUIManager>();
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Plane");
            Plane = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity);

            GrassPrefab = Resources.Load<GameObject>("Prefabs/Tile");
            EnvironmentObjects = LoadEnvironment("ScriptableObjects/Environment");
            GeneratePlane(_dataManager.PlaneSize + _dataManager.PlaneOffset * 2, _dataManager.TileScale);
            GenerateEnvironment();
        }
        public void Update()
        {
        }

        public void Dispose()
        {
            Tiles.Clear();
            EnvironmentObjects.Clear();
        }

        public void GeneratePlane(int size, float scale)
        {
            Tiles = new List<List<Tile>>();
            for (int i = 0; i < size; i++)
            {
                Tiles.Add(new List<Tile>());
                for (int j = 0; j < size; j++)
                {
                    Tiles[i].Add(new Tile(GrassPrefab, new Vector3(i - _dataManager.PlaneOffset, 0, j - _dataManager.PlaneOffset), Quaternion.identity, GameObject.Find("Tiles").transform));
                }
            }
            Plane.transform.localScale *= scale;
        }

        public void GenerateEnvironment()
        {
            int size = _dataManager.PlaneSize + _dataManager.PlaneOffset * 2;
            for (int i = _dataManager.PlaneOffset; i < size - _dataManager.PlaneOffset; i++)
            {
                for (int j = _dataManager.PlaneOffset; j < size - _dataManager.PlaneOffset; j++)
                {
                    Tiles[i][j].IsFilled = true;
                }
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if ((i < _dataManager.PlaneOffset && i >= 0 || i >= size - _dataManager.PlaneOffset && i < size) || (j < _dataManager.PlaneOffset && j >= 0 || j >= size - _dataManager.PlaneOffset && j < size))
                    {
                        if (Random.Range(0, 1f) < _dataManager.DensityOfEnvironment)
                        {
                            PlaceOnPlane(new Vector2(i - _dataManager.PlaneOffset, j - _dataManager.PlaneOffset), EnvironmentObjects[Random.Range(0, EnvironmentObjects.Count)], GameObject.Find("Environment").transform);
                        }
                    }
                }
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if ((i < _dataManager.PlaneOffset && i >= 0 || i >= size - _dataManager.PlaneOffset && i < size) || (j < _dataManager.PlaneOffset && j >= 0 || j >= size - _dataManager.PlaneOffset && j < size))
                    {
                        Tiles[i][j].IsFilled = true;
                        Tiles[i][j].IsCanBuiltHere = false;
                    }
                    else
                        Tiles[i][j].IsFilled = false;
                }
            }
        }

        private List<Building> LoadEnvironment(string path)
        {
            List<Building> result = new List<Building>();
            int itemCount;
            string[] itemsPathsStr = Directory.GetFiles(Application.dataPath + "/Resources/" + path);
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
                for (int i = 0; i < itemCount; i++)
                {
                    result.Add(Resources.Load<Building>(itemsPaths[i]));
                }
            }
            return result;
        }

        public bool PlaceOnPlane(Vector2 coordinates, Building building, Transform parent)
        {
            int offset = _dataManager.PlaneOffset;
            bool isFilled = false;
            for (int i = (int)coordinates.x; i < (int)coordinates.x + building.Size.x; i++)
                for (int j = (int)coordinates.y; j < (int)coordinates.y + building.Size.y; j++)
                {
                    if (!(j + offset < Tiles[0].Count) || !(i + offset < Tiles.Count))
                    {
                        isFilled = true;
                        continue;
                    }
                    if (Tiles[i + offset][j + offset].IsFilled || !Tiles[i + offset][j + offset].IsCanBuiltHere)
                        isFilled = true;
                }
            if (!isFilled)
            {
                Vector3 _coordinates = new Vector3(coordinates.x, 0, coordinates.y) * _dataManager.TileScale;
                MonoBehaviour.Instantiate(building.Prefab, _coordinates, Quaternion.identity, parent);
                for (int i = (int)coordinates.x; i < (int)coordinates.x + building.Size.x && i + offset < Tiles.Count; i++)
                    for (int j = (int)coordinates.y; j < (int)coordinates.y + building.Size.y && j + offset < Tiles[0].Count; j++)
                    {
                        Tiles[i + offset][j + offset].IsFilled = true;
                        Tiles[i + offset][j + offset].Building = building;
                    }
            }
            return !isFilled;
        }

        public void RemoveFromPlane(Building building)
        {
            MonoBehaviour.Destroy(building);
            /*for (int i = (int)_coordinates.x; i < building.Size.x; i++)
                for (int j = (int)_coordinates.y; j < building.Size.y; j++)
                {
                    Tiles[i][j].IsFilled = false;
                    Tiles[i][j].Building = null;
                }*/
        }
    }
}
