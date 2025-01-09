using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Common.Vision.Camera
{
    public interface IDominoCamera : IDisposable
    {
        string SerialNumber { get; }
        ICameraParameter CameraParameter { get; }
        bool IsConnected { get; }
        void Live();
        void OneShot();
        void Stop();
        void Connect();
        void Disconnect();

        event EventHandler OnConnected;
        event EventHandler OnDisconnected;

        event EventHandler<IDominoImage> AcqFifoComplete;
    }
}
