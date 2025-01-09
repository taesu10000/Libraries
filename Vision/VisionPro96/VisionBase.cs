using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;

using Domino.Gen2.Common.Vision.Camera;
using Domino.Gen2.Common.Vision.Cognex;
using Domino.Gen2.Common.Vision.File;
using Domino.Gen2.Common.Vision.Insepction;

namespace Domino.Gen2.VisionPro96V2
{
    public class VisionBase : IVisionBase
    {
        private CogFrameGrabbers _frameGrabbers;
        private ICognexInspection _inspection;
        private IDominoDataIO _dataIO;
        private readonly List<IDominoCamera> _cameraList = new List<IDominoCamera>();

        public VisionBase()
        {
            InitFrameGrabbers();
            InitInspection();
            InitDataIO();
        }

        private void InitDataIO()
        {
            _dataIO = new DominoDataIO();
        }

        private void InitInspection()
        {
            _inspection = new DominoInspection();
        }

        private void InitFrameGrabbers()
        {
            _frameGrabbers = new CogFrameGrabbers();
            foreach (ICogFrameGrabber item in _frameGrabbers)
            {
                _cameraList.Add(new DominoCamera(item));
            }
        }

        public void Close()
        {
            _cameraList.ForEach(x => x.Dispose());
            _frameGrabbers.Dispose();
        }

        public IDominoCamera GetCamera(string serial)
        {
            return _cameraList.FirstOrDefault(x => x.SerialNumber == serial);
        }

        public List<IDominoCamera> GetCameraList()
        {
            return _cameraList;
        }

        public IDominoInspection GetInspection()
        {
            return _inspection;
        }

        public IDominoDataIO GetDataIO()
        {
            return _dataIO;
        }
    }
}
