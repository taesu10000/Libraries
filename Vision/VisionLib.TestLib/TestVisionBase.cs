using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Cognex.VisionPro;

using Domino.Gen2.Common.Vision.Cognex;
using Domino.Gen2.Common.Vision.Camera;
using Domino.Gen2.Common.Vision.File;
using Domino.Gen2.Common.Vision.Insepction;

namespace VisionLib.TestLib
{
    public class TestVisionBase : IVisionBase
    {
        private readonly List<IDominoCamera> _cameraList = new List<IDominoCamera>();
        public TestVisionBase()
        {
            var camera1 = new TestCamera("TestCamera1");
            _cameraList.Add(camera1);
            var camera2 = new TestCamera("TestCamera2");
            _cameraList.Add(camera2);
        }

        public void Close()
        {
            foreach (var item in _cameraList)
            {
                item.Disconnect();
            }
        }

        public IDominoCamera GetCamera(string serial)
        {
            return _cameraList.FirstOrDefault(x => x.SerialNumber == serial);
        }

        public List<IDominoCamera> GetCameraList()
        {
            return _cameraList;
        }

        public IDominoDataIO GetDataIO()
        {
            throw new NotImplementedException();
        }

        public IDominoInspection GetInspection()
        {
            throw new NotImplementedException();
        }
    }
}
