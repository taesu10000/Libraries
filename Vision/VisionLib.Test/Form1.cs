using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Common.Vision.Cognex;
using Common.Vision.Camera;
using VisionPro96;

using VisionLib.TestLib;
using Common.Vision.Insepction;
using Common.Vision.File;

namespace VisionLib.Test
{
    public partial class Form1 : Form
    {
        private readonly IVisionBase _visionBase;
        private readonly IDominoDataIO _dataIO;
        private readonly ICognexInspection _cognexInspection;
        private ICognexDisplay _display;
        private IDominoCamera _currentCamera;
        private List<IDominoCamera> _cameraList;
        private bool _isInsepction;
        private bool _isSaveOriginalImage;
        private bool _isSaveOverlayImage;
        private bool _isLive;

        public Form1()
        {
            InitializeComponent();
            //_visionBase = VisionBase.Instance;
            _visionBase = new VisionBase();
            _dataIO = _visionBase.GetDataIO();
            var inspection = _visionBase.GetInspection();
            inspection.OnInspectionComplete += Inspection_OnInspectionComplete;
            _cognexInspection = inspection as ICognexInspection;
            InitControl();
        }


        private void InitControl()
        {
            //카메라 시리얼 리스트 가져오기
            _cameraList = _visionBase.GetCameraList();
            foreach (var camera in _cameraList)
            {
                camera.OnConnected += CurrentCamera_OnConnected;
                camera.OnDisconnected += CurrentCamera_OnDisconnected;
                cmbCameraSerialList.Items.Add(camera.SerialNumber);
            }                    

            cmbSaveImageFormat.DataSource = Enum.GetValues(typeof(SaveImageFormatType));
            _display = cogRecordDisplayControl1;
        }

        private void CurrentCamera_OnConnected(object sender, EventArgs e)
        {
            if (sender is IDominoCamera camera)
            {
                camera.AcqFifoComplete += Camera_AcqFifoComplete;
                UpdateCameraParams(camera);
                Console.WriteLine($"Camera {camera.SerialNumber} connected");
            }
        }

        private void Camera_AcqFifoComplete(object sender, IDominoImage e)
        {
            if (_isInsepction)
            {
                _cognexInspection.Run(e);
            }
            else
                _display.UpdateDisplay(e);
        }

        private void Inspection_OnInspectionComplete(object sender, IDominoInspectionResult e)
        {
            _display.UpdateRecord(e);
            if (_isSaveOriginalImage) _dataIO.SaveOriginalImage(e.OriginalImage);
            if (_isSaveOverlayImage) _dataIO.SaveOverlayImage(e.OverlayImage);
        }

        private void CurrentCamera_OnDisconnected(object sender, EventArgs e)
        {
            if (sender is IDominoCamera camera)
            {
                if (camera.SerialNumber.Equals(cmbCameraSerialList.SelectedItem.ToString()))
                    UpdateCameraParams(camera);
                Console.WriteLine($"Camera {camera.SerialNumber} disconnected");
            }
        }

        private void UpdateCameraParams(IDominoCamera camera)
        {
            UpdateCameraControl(camera.IsConnected);
            var cameraParameter = camera?.CameraParameter;
            if (cameraParameter != null)
            {
                txbExposure.Text = cameraParameter.Exposure.ToString();
                txbBrightness.Text = cameraParameter.Brightness.ToString();
                txbContrast.Text = cameraParameter.Contrast.ToString();
                txbTriggerInterval.Text = cameraParameter.LiveTriggerInterval.ToString();
            }
        }

        private void UpdateCameraControl(bool isConnected)
        {
            grbCameraAction.Enabled = isConnected;
            grbCameraParams.Enabled = isConnected;
            btnConnect.Enabled = !isConnected;
            btnDisconnect.Enabled = isConnected;
        }

        private void BtnStartAcquire_Click(object sender, EventArgs e)
        {
            _currentCamera?.OneShot();
        }

        private void BtnLive_Click(object sender, EventArgs e)
        {
            if (!_isLive)
            {
                _currentCamera?.Live();
                _isLive = true;
            }
        }

        private void BtnStopLive_Click(object sender, EventArgs e)
        {
            _currentCamera?.Stop();
            _isLive = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _visionBase.Close();
            Task.Delay(1000).Wait();
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            _isInsepction = true;
            _currentCamera?.OneShot();
        }

        private void BtnLoadImageFromFile_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _display?.LoadImageFromFile(dlg.FileName);
                    //if (!result)
                    //{
                    //    // 파일을 못 불러왔을 때 로그
                    //}
                }
            }
        }

        private void BtnLoadVppFromFile_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _cognexInspection?.LoadVppFromFile(dlg.FileName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private void BtnOpenVpp_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new Form();
                var vppControl = _cognexInspection?.GetToolBlockControl();
                if (vppControl != null)
                {
                    vppControl.Dock = DockStyle.Fill;
                    form.Controls.Add(vppControl);
                    form.Show();
                }
                else
                {
                    // 없을 경우 에러 추가
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void BtnContinueAcquire_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txbTriggerInterval.Text, out int interval))
            {
                _isInsepction = true;
                _currentCamera.CameraParameter.LiveTriggerInterval = interval;
                _currentCamera?.Live();
            }
            else
            {
                // 에러 추가
            }
        }

        private void BtnSetOverlayFolderPath_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _dataIO.SaveOverlayImageFolderPath = dlg.SelectedPath;
                    lblSetOverlayImageFolderPath.Text = dlg.SelectedPath;
                }
            }
        }

        private void BtnSetOriginFolderPath_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _dataIO.SaveOriginalImageFolderPath = dlg.SelectedPath;
                    lblSetOriginImageFolderPath.Text = dlg.SelectedPath;
                }
            }
        }

        private void ChbSaveOrgImage_CheckedChanged(object sender, EventArgs e)
        {
            _isSaveOriginalImage = chbSaveOrgImage.Checked;
        }

        private void ChbSaveOverlayImage_CheckedChanged(object sender, EventArgs e)
        {
            _isSaveOverlayImage = chbSaveOverlayImage.Checked;
        }

        private void CmbSaveImageFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSaveImageFormat.SelectedItem is SaveImageFormatType format && _currentCamera != null)
            {
                _dataIO.SaveImageFormat = format;
            }
        }

        private void TxbNumberToFind_TextChanged(object sender, EventArgs e)
        {
            _cognexInspection.MaxBarcodeReadingCount = int.Parse(txbNumberToFind.Text);
        }

        private void TxbTriggerInterval_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txbTriggerInterval.Text, out int interval))
            {
                _currentCamera.CameraParameter.LiveTriggerInterval = interval;
            }
        }

        private void BtnSetExposure_Click(object sender, EventArgs e)
        {
            _currentCamera.CameraParameter.Exposure = double.Parse(txbExposure.Text);
        }

        private void BtnSetBrightness_Click(object sender, EventArgs e)
        {
            _currentCamera.CameraParameter.Brightness = double.Parse(txbBrightness.Text);
        }

        private void BtnSetContrast_Click(object sender, EventArgs e)
        {
            _currentCamera.CameraParameter.Contrast = double.Parse(txbContrast.Text);
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            var serial = cmbCameraSerialList.SelectedItem.ToString();
            var selectedCamera = _visionBase.GetCamera(serial);
            
            if (selectedCamera == null)
            {
                // 에러 추가
                return;
            }
            selectedCamera.Connect();
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            var serial = cmbCameraSerialList.SelectedItem.ToString();
            var selectedCamera = _visionBase.GetCamera(serial);
            selectedCamera.Disconnect();
        }

        private void CmbCameraSerialList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCameraSerial = cmbCameraSerialList.SelectedItem.ToString();
            var selectedCamera = _visionBase.GetCamera(selectedCameraSerial);
            UpdateCameraControl(selectedCamera.IsConnected);
            // Connected라면 카메라 파라미터 업데이트
            if (selectedCamera.IsConnected) UpdateCameraParams(selectedCamera);
            _currentCamera = selectedCamera;
        }

        private void BtnContinueStop_Click(object sender, EventArgs e)
        {
            _isInsepction = false;
            _currentCamera?.Stop();
        }

    }
}
