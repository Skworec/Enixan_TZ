using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface IPlaneManager
    {
        GameObject Plane { get; set; }
        List<List<Tile>> Tiles { get; set; }
        void GeneratePlane(int size, float scale);
        void GenerateEnvironment();
    }

    public class Tile
    {
        public bool IsCanBuiltHere;
        private bool _isFilled;
        public GameObject gameObject;
        public Building Building;

        public bool IsFilled
        {
            get
            {
                return _isFilled;
            }
            set
            {
                _isFilled = value;
                gameObject.GetComponentInChildren<SpriteRenderer>().color = _isFilled ? Color.red : Color.white;
            }
        }

        public Tile(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, bool isFilled = false)
        {
            gameObject = MonoBehaviour.Instantiate(prefab, position, rotation, parent);
            IsFilled = isFilled;
            IsCanBuiltHere = true;
        }
    }
}
