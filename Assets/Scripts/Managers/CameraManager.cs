using Assets.Scripts.Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CameraManager : ISystem, ICameraManager
    {
        private bool _isRaycastable;
        private bool _isMoved;
        private Vector3 _touchStart;

        private Camera _camera;

        private ISettingsManager _dataManager;
        private IUIManager _uiManager;

        public bool IsRaycastable
        {
            get
            {
                return _isRaycastable;
            }
            set
            {
                _isRaycastable = value;
            }
        }
        public bool IsMoved
        {
            get
            {
                return _isMoved;
            }
            set
            {
                _isMoved = value;
            }
        }

        public void Dispose()
        {
        }

        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            _dataManager = GameClient.Get<ISettingsManager>();
            _dataManager.ZoomOutMin *= _dataManager.TileScale;
            _dataManager.ZoomOutMax *= _dataManager.TileScale;
            _dataManager.MaxCamPos *= _dataManager.TileScale;
            IsMoved = true;
            IsRaycastable = true;
            _camera = _uiManager.Camera;
            SetCameraRotation(new Vector3(30, 45, 0));
            float startPosition = _dataManager.PlaneSize * _dataManager.TileScale / 2;
            MoveCamera(new Vector3(startPosition, 15, startPosition));
        }

        public void MoveCamera(Vector3 position)
        {
            if (_isMoved)
            {
                Vector3 newPosition = new Vector3(Mathf.Clamp(position.x, _dataManager.MinCamPos, _dataManager.MaxCamPos), position.y, Mathf.Clamp(position.z, _dataManager.MinCamPos, _dataManager.MaxCamPos));
                _camera.transform.position = newPosition;
            }
        }

        public void SetCameraRotation(Vector3 angles)
        {
                Quaternion q = new Quaternion();
                q.eulerAngles = angles;
                _camera.transform.rotation = q;
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _touchStart = GetWorldPosition(Input.mousePosition, 0);
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                Zoom(difference * _dataManager.ZoomFactor);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 direction = _touchStart - GetWorldPosition(Input.mousePosition, 0);
                direction.y = 0;
                MoveCamera(_camera.transform.position + direction);
            }
            Zoom(Input.GetAxis("Mouse ScrollWheel") * 10);
        }

        public void Zoom(float increment)
        {
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - increment, _dataManager.ZoomOutMin, _dataManager.ZoomOutMax);
        }

        public Vector3 GetWorldPosition(Vector3 screenPosition, float z)
        {

            if (_isRaycastable)
            {
                Ray mousePos = _camera.ScreenPointToRay(screenPosition);
                Plane ground = new Plane(Vector3.up, new Vector3(0, 0, z));
                float distance;
                ground.Raycast(mousePos, out distance);
                return mousePos.GetPoint(distance);
            }
            else return new Vector3(-100, -100, -100);
        }
    }
}
