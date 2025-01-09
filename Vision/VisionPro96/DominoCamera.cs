using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

using Cognex.VisionPro;

using Common.Vision.Camera;

namespace VisionPro96V2
{
    internal class DominoCamera : IDominoCamera, IDisposable
    {


        #region Variables

        private readonly ICogFrameGrabber _frameGrabber;
        private ICogAcqFifo _acqFifo;
        private bool _isLive;
        private int _imgCount;
        private bool disposedValue;

        #endregion

        #region Properties
        public string SerialNumber => _frameGrabber.SerialNumber;
        public ICameraParameter CameraParameter { get; private set; }
        public bool IsConnected { get; private set; }

        #endregion

        #region Constructor
        public DominoCamera(ICogFrameGrabber item)
        {
            _frameGrabber = item;
        }

        #endregion

        #region Finalizer
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }
                DisposeCamera();
                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        private void DisposeCamera()
        {
            if (_isLive)
                Stop();
            Task.Delay(100).Wait();
            _frameGrabber.Disconnect(true);
        }

        // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        ~DominoCamera()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods
        public void Connect()
        {
            try
            {
                _acqFifo = _frameGrabber.CreateAcqFifo(_frameGrabber.AvailableVideoFormats[0], CogAcqFifoPixelFormatConstants.Format8Grey, 0, true);
                _acqFifo.Complete += AcqFifo_Complete;
                _acqFifo.OwnedGigEVisionTransportParams.LatencyLevel = 0;
                InitParameter();
                IsConnected = true;
                OnConnected?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception)
            {
                // 에러 처리
            }

        }

        private void InitParameter()
        {
            CameraParameter = new DominoCameraParameter(_acqFifo);
            _acqFifo.OwnedExposureParams.Exposure = CameraParameter.Exposure;
            _acqFifo.OwnedBrightnessParams.Brightness = CameraParameter.Brightness;
            _acqFifo.OwnedContrastParams.Contrast = CameraParameter.Contrast;
        }

        private void AcqFifo_Complete(object sender, CogCompleteEventArgs e)
        {
            try
            {
                _acqFifo.GetFifoState(out int numPendingVal, out int numReadyVal, out bool busyVal);
                ICogImage currentImage;
                if (numReadyVal > 0)
                {
                    var acqInfo = new CogAcqInfo();
                    currentImage = (CogImage8Grey)_acqFifo.CompleteAcquireEx(acqInfo);
                    Task.Run(() =>
                    {
                        IDominoImage img = new DominoImage(currentImage);
                        AcqFifoComplete?.Invoke(this, img);
                    });
                    Task.Run(() =>
                    {
                        if (_isLive)
                        {
                            _acqFifo.Flush();
                            var delay = Convert.ToInt16(1000.0 / CameraParameter.LiveTriggerInterval);
                            Task.Delay(delay).Wait();
                            _acqFifo.StartAcquire();
                        }
                    });
                    _imgCount++;
                    if (_imgCount > 5)
                    {
                        GC.Collect();
                        _imgCount = 0;
                    }
                    Console.WriteLine("grabbed");
                }
                else
                    throw new Exception("Ready count is not greater than 0.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Disconnect()
        {
            IsConnected = false;
            OnDisconnected?.Invoke(this, EventArgs.Empty);
        }

        public void Live()
        {
            _isLive = true;
            OneShot();
        }

        public void OneShot()
        {
            _acqFifo.StartAcquire();
        }

        public void Stop()
        {
            _isLive = false;
        }
        #endregion

        #region Events

        public event EventHandler OnConnected;
        public event EventHandler OnDisconnected;
        public event EventHandler<IDominoImage> AcqFifoComplete;

        #endregion









        


    }
}
