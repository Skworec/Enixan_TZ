using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Managers.Interfaces
{
    public interface ISettingsManager
    {
        //Camera Settings
        float MinCamPos { get; set; }
        float MaxCamPos { get; set; }
        float ZoomOutMin { get; set; }
        float ZoomOutMax { get; set; }
        float ZoomFactor { get; set; }
        //


        //Plane Settings
        int PlaneOffset { get; set; }
        int PlaneSize { get; set; }
        float TileScale { get; set; }
        float DensityOfEnvironment { get; set; }
        //
    }
}
