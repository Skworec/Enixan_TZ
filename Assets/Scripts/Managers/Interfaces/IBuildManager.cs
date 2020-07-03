using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface IBuildManager
    {
        BUILD_STATE BuildState { get; set; }
        Dictionary<int, BuildingView> BuildingViews { get; set; }
        void BuildABuilding(Vector2 planePoint, Building building);
        void MoveBuilding(Vector2 worldPoint);
        void RemoveBuilding();
    }

    public class BuildingView
    {
        public Transform SelfTransform;
        private Vector2 _pointOnPlane;


        public Building scriptObj;
        public Vector2 PointOnPlane
        {
            get
            {
                return _pointOnPlane;
            }
            set
            {

                _pointOnPlane = value;
            }
        }

        public BuildingView(Building building, Vector2 point, Transform parent)
        {
            scriptObj = building;
            PointOnPlane = point;
            Vector3 p = new Vector3(point.x, 0, point.y);
            SelfTransform = MonoBehaviour.Instantiate(scriptObj.Prefab, p, Quaternion.identity, parent).transform;
        }

        public bool IsHoldOnPoint(Vector2 point)
        {
            for (int i = (int)(PointOnPlane.x); i < (int)(PointOnPlane.x + scriptObj.Size.x); i++)
            {
                for (int j = (int)(PointOnPlane.y); j < (int)(PointOnPlane.y + scriptObj.Size.y); j++)
                    if (point.x == i && point.y == j)
                        return true;
            }
            return false;
        }
    }

    public enum BUILD_STATE
    {
        Idle,
        BuildingChosen,
        MovingBuilding
    }
}
