using Assets.Scripts.Managers.Interfaces;

namespace Assets.Scripts.Managers
{
    public class SettingsManager : ISystem, ISettingsManager
    {
        private float _minCamPos;
        private float _maxCamPos;
        private float _zoomOutMin;
        private float _zoomOutMax;
        private float _zoomFactor;

        private int _planeOffset;
        private int _planeSize;
        private float _tileScale;
        private float _densityOfEnvironment;

        public float MinCamPos
        {
            get
            {
                return _minCamPos;
            }
            set
            {
                _minCamPos = value;
            }
        }
        public float MaxCamPos
        {
            get
            {
                return _maxCamPos;
            }
            set
            {
                _maxCamPos = value;
            }
        }
        public float ZoomOutMin
        {
            get
            {
                return _zoomOutMin;
            }
            set
            {
                _zoomOutMin = value;
            }
        }
        public float ZoomOutMax
        {
            get
            {
                return _zoomOutMax;
            }
            set
            {
                _zoomOutMax = value;
            }
        }
        public float ZoomFactor
        {
            get
            {
                return _zoomFactor;
            }
            set
            {
                _zoomFactor = value;
            }
        }


        public int PlaneSize 
        { 
            get 
            { 
                return _planeSize; 
            } 
            set 
            { 
                _planeSize = value; 
            } 
        }
        public float TileScale
        {
            get
            {
                return _tileScale;
            }
            set
            {
                _tileScale = value;
            }
        }
        public int PlaneOffset
        {
            get
            {
                return _planeOffset;
            }
            set
            {
                _planeOffset = value;
            }
        }
        public float DensityOfEnvironment
        {
            get
            {
                return _densityOfEnvironment;
            }
            set
            {
                _densityOfEnvironment = value;
            }
        }

        public void Init()
        {
            MinCamPos = -17;
            MaxCamPos = 11;
            ZoomOutMin = 10f;
            ZoomOutMax = 30f;
            ZoomFactor = 0.01f;
            DensityOfEnvironment = 0.4f;

            PlaneOffset = 10;
            PlaneSize = 30;
            TileScale = 1f;
        }

        public void Dispose()
        {
        }

        public void Update()
        {
        }
    }
}
