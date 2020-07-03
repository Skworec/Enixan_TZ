using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.UI.Popups;

namespace Assets.Scripts.Managers
{
    class BuildManager : ISystem, IBuildManager
    {
        private Vector3 _touchStart;
        private Dictionary<int, BuildingView> _buildings;
        private BUILD_STATE _buildState;
        private Transform _parent;

        private int _chosenBuldingId;

        private ICameraManager _cameraManager;
        private IUIManager _uiManager;
        private IPlaneManager _planeManager;
        private ISettingsManager _settingsManager;
        public bool flag;

        public BUILD_STATE BuildState
        {
            get { return _buildState; }
            set { _buildState = value; }
        }
        public Dictionary<int, BuildingView> BuildingViews
        {
            get { return _buildings; }
            set { _buildings = value; }
        }

        public void BuildABuilding(Vector2 planePoint, Building building)
        {
            if (IsCanBuildOnPoint(building, planePoint) && BuildState == BUILD_STATE.Idle)
            {
                BuildingViews.Add(BuildingViews.Count, new BuildingView(building, planePoint, _parent));
                for (int i = (int)planePoint.x; i < planePoint.x + building.Size.x; i++)
                    for (int j = (int)planePoint.y; j < planePoint.y + building.Size.y; j++)
                        _planeManager.Tiles[i + _settingsManager.PlaneOffset][j + _settingsManager.PlaneOffset].IsFilled = true;
                _chosenBuldingId = BuildingViews.Count - 1;
                BuildState = BUILD_STATE.BuildingChosen;
            }
            else
            {
                Debug.LogWarning("You cannot built this here");
            }
        }

        public void Dispose()
        {
            
        }

        public void Init()
        {
            flag = true;
            BuildingViews = new Dictionary<int, BuildingView>();
            BuildState = BUILD_STATE.Idle;
            _cameraManager = GameClient.Get<ICameraManager>();
            _uiManager = GameClient.Get<IUIManager>();
            _planeManager = GameClient.Get<IPlaneManager>();
            _settingsManager = GameClient.Get<ISettingsManager>();
            _parent = _planeManager.Plane.transform.Find("Buildings");
        }

        public void MoveBuilding(Vector2 worldPoint)
        {
            BuildingView building = BuildingViews[_chosenBuldingId];
            worldPoint = GridSnap(worldPoint);
            worldPoint.x = Mathf.Clamp(worldPoint.x, 0, _planeManager.Tiles.Count - 1);

            if (IsCanBuildOnPoint(building.scriptObj, worldPoint))
            {
                for (int i = (int)building.PointOnPlane.x; i < building.PointOnPlane.x + building.scriptObj.Size.x; i++)
                    for (int j = (int)building.PointOnPlane.y; j < building.PointOnPlane.y + building.scriptObj.Size.y; j++)
                        _planeManager.Tiles[i + _settingsManager.PlaneOffset][j + _settingsManager.PlaneOffset].IsFilled = false;

                building.SelfTransform.position = new Vector3(worldPoint.x, 0, worldPoint.y);
                building.PointOnPlane = worldPoint;
            }
        }

        public void PlaceChoosenBuilding()
        {
            BuildingView building = BuildingViews[_chosenBuldingId];
            for (int i = (int)building.PointOnPlane.x; i < building.PointOnPlane.x + building.scriptObj.Size.x; i++)
                for (int j = (int)building.PointOnPlane.y; j < building.PointOnPlane.y + building.scriptObj.Size.y; j++)
                    _planeManager.Tiles[i + _settingsManager.PlaneOffset][j + _settingsManager.PlaneOffset].IsFilled = true;
        }

        public void RemoveBuilding()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            Vector3 worldPos = Vector3.zero;
            if (Input.GetMouseButtonDown(1))
            {
                flag = !flag;
                _cameraManager.IsMoved = flag;
            }
            if (Input.GetMouseButtonDown(0))
            {
                _touchStart = _cameraManager.GetWorldPosition(Input.mousePosition, 0) - Vector3.one / 2;
            }
            if (Input.GetMouseButton(0))
            {
                worldPos = _cameraManager.GetWorldPosition(Input.mousePosition, 0) - Vector3.one / 2;
            }
            switch (BuildState)
            {
                case BUILD_STATE.Idle:
                    {
                        _uiManager.HidePopup<BuildingInfoPopup>();
                        int i = IsHitedBuilding(new Vector2(_touchStart.x, _touchStart.z));
                        if (i != -1)    //Клик на здание
                        {
                            ChooseBuilding(i);
                        }
                        break;
                    }
                case BUILD_STATE.BuildingChosen:
                    {
                        bool canBeInstalled = false;
                        _cameraManager.IsMoved = true;
                        int id = IsHitedBuilding(new Vector2(_touchStart.x, _touchStart.z));
                        canBeInstalled = IsCanBuildOnPoint(BuildingViews[_chosenBuldingId].scriptObj, BuildingViews[_chosenBuldingId].PointOnPlane);
                        if (canBeInstalled)
                        {
                            PlaceChoosenBuilding();
                            if (id == -1) //Клик на незанятое пространство
                            {
                                _chosenBuldingId = -1;
                                BuildState = BUILD_STATE.Idle;
                                break;
                            }
                            else if (id != _chosenBuldingId) //Клик на другое здание
                            {
                                ChooseBuilding(id);
                            }
                        }
                        if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
                        {
                            Debug.Log("Tuta");
                            BuildState = BUILD_STATE.MovingBuilding;
                        }
                        break;
                    }
                case BUILD_STATE.MovingBuilding:
                    {
                        _cameraManager.IsMoved = false;
                        if (Input.GetMouseButton(0))
                        {
                            worldPos = _cameraManager.GetWorldPosition(Input.mousePosition, 0) - Vector3.one / 2;
                            MoveBuilding(new Vector2(worldPos.x, worldPos.z));
                        }
                        else //Отпустили кнопку
                        {
                            BuildState = BUILD_STATE.BuildingChosen;
                        }

                        break;
                    }
            }
        }

        private void ChooseBuilding(int i)
        {
            _chosenBuldingId = i;
            BuildState = BUILD_STATE.BuildingChosen;
            _uiManager.GetPopup<BuildingInfoPopup>().Show(BuildingViews[i].scriptObj.Description);
        }

        private Vector2 GridSnap(Vector2 worldPoint)
        {
            return new Vector2(Mathf.Round(worldPoint.x), Mathf.Round(worldPoint.y));
        }

        private int IsHitedBuilding(Vector2 point)
        {
            int result = -1;
            for (int i = 0; i < BuildingViews.Count; i++)
            {
                if (BuildingViews[i].IsHoldOnPoint(GridSnap(point)))
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        private bool IsCanBuildOnPoint(Building scriptObj, Vector2 point)
        {
            int offset = _settingsManager.PlaneOffset;
            for (int i = (int)(point.x); i < (int)(point.x + scriptObj.Size.x); i++)
            {
                for (int j = (int)(point.y); j < (int)(point.y + scriptObj.Size.y); j++)
                    if (!(_planeManager.Tiles[i + offset][j + offset].IsCanBuiltHere && !_planeManager.Tiles[i + offset][j + offset].IsFilled))
                        return false;
            }
            return true;
        }
    }
}
