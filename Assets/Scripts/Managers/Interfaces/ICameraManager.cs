using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
    interface ICameraManager
    {
        bool IsRaycastable { get; set; }
        bool IsMoved { get; set; }
        void MoveCamera(Vector3 position);
        void SetCameraRotation(Vector3 angles);
        void Zoom(float multiplier);
        Vector3 GetWorldPosition(Vector3 screenPosition, float z);
    }
}
