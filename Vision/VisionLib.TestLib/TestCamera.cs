using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Cognex.VisionPro;

using Domino.Gen2.Common.Vision.Camera;

namespace VisionLib.TestLib
{
    internal class TestCamera : IDominoCamera
    {
        private readonly List<Bitmap> _fileList = new List<Bitmap>();
        private int _imageIndex;
        private bool _isContinue;
        private bool _isLive;
        private bool disposedValue;

        public string SaveOriginalImageFolderPath { get; set; }
        public string SaveOverlayImageFolderPath { get; set; }
        public bool IsSaveOriginalImage { get; set; }
        public bool IsSaveOverlayImage { get; set; }
        public int MaxBarcodeReadingCount { get; set; }
        public SaveImageFormatType SaveImageFormat { get; set; }
        public string SerialNumber { get; }
        public ICameraParameter CameraParameter { get; }
        public bool IsConnected { get; private set; }

        public event EventHandler OnConnected;
        public event EventHandler OnDisconnected;
        public event EventHandler<IDominoImage> AcqFifoComplete;
        public event EventHandler<string> OnVppFromFileLoaded;
        public event EventHandler<Image> OnImageFromFileLoaded;
        public event EventHandler<string> OnOriginalImageFromFileSaved;
        public event EventHandler<string> OnOverlayImageFromFileSaved;

        private void SetTextCallback(string text) => Console.WriteLine(text);
        public TestCamera(string sn)
        {
            SerialNumber = sn;
            CameraParameter = new TestCameraParameter();
            InitImageBuffer();
        }

        private void InitImageBuffer()
        {
            var filesPath = Path.Combine(Environment.CurrentDirectory, "Images");
            if (!Directory.Exists(filesPath))
            {
                return;
            }
            _fileList.Clear();
            var files = Directory.GetFiles(filesPath);
            for (int i = 0; i < files.Length; i++)
            {
                var fileName = files[i];
                if (fileName.EndsWith(".bmp"))
                {
                    _fileList.Add(new Bitmap(fileName));
                }
            }
        }

        public void Connect()
        {
            if (!IsConnected)
            {
                IsConnected = true;
                OnConnected?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                SetTextCallback($"{SerialNumber} Already Connected");
            }
        }
        public void ContinueShot()
        {
            _isContinue = true;
            Task.Run(() =>
            {
                while(_isContinue)
                {
                    Grab();
                    SaveBitmap();
                    Task.Delay(1000 / CameraParameter.LiveTriggerInterval).Wait();
                }
            });
        }

        public Control GetToolBlockControl()
        {
            return null;
        }

        public void Inspection()
        {
            SetTextCallback($"Inspection Complete");
            SaveBitmap();
        }

        private void SaveBitmap()
        {
            if (IsSaveOriginalImage) SaveOriginalBitmap(SaveOriginalImageFolderPath + ".bmp");
            if (IsSaveOverlayImage) SaveOverlayBitmap(SaveOverlayImageFolderPath + ".bmp");
        }

        public void Live()
        {
            _isLive = true;
            Task.Run(() =>
            {
                while (_isLive)
                {
                    Grab();
                    Task.Delay(100).Wait();
                }
            });
            
        }

        public void OneShot()
        {
            Grab();
        }

        private void Grab()
        {
            AcqFifoComplete?.Invoke(this, new TestDominoImage { DominoImage = _fileList[_imageIndex % 3]});

            _imageIndex++;
            if (_imageIndex % 5 == 0) GC.Collect();
        }

        public void Stop()
        {
            _isLive = false;
        }

        public void LoadVppFromFile(string fileName)
        {
            OnVppFromFileLoaded?.Invoke(this, fileName);
        }

        public void SaveOriginalBitmap(string fileName)
        {
            SetTextCallback($"Original Bitmap {fileName} Saved");
        }

        public void Disconnect()
        {
            if (_isLive) Stop();
            if (_isContinue) ContinueStop();
            SetTextCallback($"{SerialNumber} Disconnected");
            IsConnected = false;
            OnDisconnected?.Invoke(this, EventArgs.Empty);
        }

        public void LoadImageFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                OnImageFromFileLoaded?.Invoke(this, new Bitmap(fileName));
            }
        }

        public void ContinueStop()
        {
            _isContinue = false;
        }

        public void SaveOverlayBitmap(string fileName)
        {
            SetTextCallback($"Overlay Bitmap {fileName} Saved");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~TestCamera()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
