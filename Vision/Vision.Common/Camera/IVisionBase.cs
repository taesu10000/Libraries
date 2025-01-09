using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common.Vision.File;
using Common.Vision.Insepction;

namespace Common.Vision.Camera
{
    public interface IVisionBase
    {

        void Close();
        IDominoCamera GetCamera(string serial);
        List<IDominoCamera> GetCameraList();
        IDominoDataIO GetDataIO();
        IDominoInspection GetInspection();
    }
}
