using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;

using Common.Vision.Camera;
using Common.Vision.Cognex;
using Common.Vision.Insepction;
using VisionPro96V2.Models;

namespace VisionPro96V2
{
    internal class DominoInspection : ICognexInspection
    {
        private CogToolBlock _toolBlock;


        #region Variables
        private const string BARCODE_MAX_COUNT = "BarcodeMaxCount";
        private const string DECODED_STRING = "DecodedString";
        private const string INPUT_IMAGE = "InputImage";
        #endregion

        #region Properties
        public int MaxBarcodeReadingCount { get; set; } = 1;

        #endregion

        #region Constructor

        public DominoInspection()
        {
            _toolBlock = new CogToolBlock();
            _toolBlock.Inputs.Add(new CogToolBlockTerminal(INPUT_IMAGE, typeof(ICogImage)));
            _toolBlock.Inputs.Add(new CogToolBlockTerminal(BARCODE_MAX_COUNT, typeof(int)));
            _toolBlock.Outputs.Add(new CogToolBlockTerminal(DECODED_STRING, typeof(string)));
            InitToolBlock(_toolBlock);
        }

        #endregion

        #region Finalizer

        #endregion

        #region Methods

        public Control GetToolBlockControl()
        {
            if (_toolBlock == null)
            {
                throw new Exception("ToolBlock is null");
            }
            return new CogToolBlockEditV2 { Subject = _toolBlock };
        }

        public void LoadVppFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                var vppObject = CogSerializer.LoadObjectFromFile(fileName);
                if (vppObject is CogToolBlock tb)
                {
                    _toolBlock.Dispose();
                    _toolBlock.Ran -= ToolBlock_Ran;
                    _toolBlock = tb;
                    InitToolBlock(_toolBlock);
                }
                else
                {
                    throw new InvalidOperationException("Invalid VPP file");
                }
            }
            else
            {
                throw new FileNotFoundException("File not found", fileName);
            }
        }

        private void InitToolBlock(CogToolBlock _toolBlock)
        {
            var maxBarcodeCount = _toolBlock.Inputs[BARCODE_MAX_COUNT];
            if (maxBarcodeCount != null)
            {

            }
            _toolBlock.Ran += ToolBlock_Ran;
        }

        private void ToolBlock_Ran(object sender, EventArgs e)
        {
            if (sender is CogToolBlock tb)
            {
                var result = new DominoInspectionResult();
                var cogImage = tb.Inputs["InputImage"].Value as ICogImage;
                result.OriginalImage = new DominoImage(cogImage);
                result.Record = GetRecord(tb);
                OnInspectionComplete?.Invoke(this, result);
            }
        }

        private ICogRecord GetRecord(CogToolBlock cogToolBlock)
        {
            if (cogToolBlock != null && cogToolBlock.CreateLastRunRecord().SubRecords.Count > 0)
            {
                return cogToolBlock.CreateLastRunRecord().SubRecords[0];
            }
            else
            {
                //에러 처리
                return null;
            }
        }

        public void Run(IDominoImage dominoImage)
        {
            var currentImage = dominoImage as ICognexImage;
            if (!(dominoImage is ICognexImage))
            {
                throw new InvalidOperationException("Invalid image type");
            }
            if (!_toolBlock.Inputs.Contains("InputImage"))
            {
                throw new InvalidOperationException("ToolBlock does not contain InputImage");
                
            }

            _toolBlock.Inputs["InputImage"].Value = currentImage?.CogImage;
            _toolBlock.Run();
        }

        #endregion

        #region Events

        public event EventHandler<string> OnVppFromFileLoaded;
        public event EventHandler<IDominoInspectionResult> OnInspectionComplete;

        #endregion




    }
}
