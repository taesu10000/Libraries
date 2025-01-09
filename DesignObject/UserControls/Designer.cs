using DesignObject.Controls;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DesignObject
{
    public partial class Designer : UserControl
    {
        new string MousePosition;
        public Designer()
        {
            InitializeComponent();
            Load += Designer_Load;
            Disposed += Designer_Disposed;
            DesignerEvent.ButtonPushEventHandler += ButtonEvent_ButtonEventHandler;
            DesignerEvent.Invalidate += ((sender, e) => Invalidate());
            DesignerEvent.CreateTable += DesignerContextMenu_CreateTableHandler;
            DesignerEvent.NewPaper += DesignerEvent_NewPaper;
            DesignerEvent.Redo += DesignerEvent_Redo;
            DesignerEvent.Undo += DesignerEvent_Undo;
			DesignerEvent.FocusDesigner += DesignerEvent_FocusDesigner;
			ucPalette.MouseMove += UcPalette_MouseMove;
		}
		private void DesignerEvent_FocusDesigner(object sender, EventArgs e)
		{
			this.ActiveControl = ucPalette;
		}
		private void DesignerEvent_Undo(object sender, EventArgs e)
        {
            ucPalette.Undo();
        }

        private void DesignerEvent_Redo(object sender, EventArgs e)
        {
            ucPalette.Redo();
        }

        private void DesignerEvent_NewPaper(object sender, Paper e)
        {
            initializeRuler(e);

            vScrollBar1.Minimum = 0;
            vScrollBar1.Maximum = Convert.ToInt32(e.SizePx.Height + 200.0f);
            //vScrollBar1.LargeChange = Math.Round(ucPalette.ClientRectangle.Height / vScrollBar1.Maximum, 2);
            hScrollBar1.Minimum = 0;
            hScrollBar1.Maximum = Convert.ToInt32(e.SizePx.Width + 100.0f);
            //hScrollBar1.LargeChange = hScrollBar1.Maximum / ucPalette.ClientRectangle.Width;
        }

        private void Designer_Disposed(object sender, EventArgs e)
        {
            DesignerVariables.Instance.DesignObjManager = null;
            DesignerEvent.Dispose();
        }

        private void DesignerContextMenu_CreateTableHandler(object sender, TableProp e)
        {
            DesignerVariables.Instance.DesignObjManager.TableObjs.Add(new TableObj(e.Rect, e.Col, e.Row));
            Invalidate();
        }

        private void UcPalette_MouseMove(object sender, MouseEventArgs e)
        {
            MousePosition = string.Format("X : {0} Y : {1} ", e.Location.GetZoomedPoint().X, e.Location.GetZoomedPoint().Y);
            //if (DesignerVariables.Instance.DocumentManager.SelectedMainObj.Count == 1)
            //    MousePosition = string.Format("{0} Selected Object Pos : {1}", MousePosition, DesignerVariables.Instance.DocumentManager.SelectedMainObj[0].Location);

            toolStripStatusLabel1.Text = MousePosition;
        }

        private void ButtonEvent_ButtonEventHandler(object sender, ButtonEventArgs e)
        {
            switch (e.EnButtons)
            {
                case enButtons.NewDocument:
                    var newDocArgs = e as NewDocumentArgs;
                    DesignerVariables.Instance.NewDocument(newDocArgs);
                    break;
                case enButtons.Save:
                    {
                        var loadSave = e as PathArgs;
                        DesignerEvent.OnSave(loadSave);
                    }
                    break;
                case enButtons.Load:
                    {
                        var loadSave = e as PathArgs;
                        DesignerEvent.OnLoad(loadSave);
                    }
                    break;
                case enButtons.NewTable:
                    Draw(enDraw.DrawTable);
                    break;
                case enButtons.TextBox:
                    Draw(enDraw.DrawMessageBox);
                    break;
                case enButtons.Barcode:
                    Draw(enDraw.DrawBarcode);
                    break;
                case enButtons.Ellipse:
                    Draw(enDraw.DrawCircle);
                    break;
                case enButtons.Rectangle:
                    Draw(enDraw.DrawRect);
                    break;
                case enButtons.Line:
                    Draw(enDraw.DrawLine);
                    break;
                case enButtons.Image:
                    {
						Draw(enDraw.DrawImage);
                    }
                    break;
                case enButtons.Print:
                    DesignerEvent.OnPopUpPrinterSetting();
                    break;
                case enButtons.PaperSize:
                    DesignerEvent.OnPopUpPaperSetting(DesignerVariables.Instance.DesignObjManager.Paper);
                    break;
                case enButtons.AlignTop:
                    ucPalette.SetObjectAalign(enAlign.Top);
                    break;
                case enButtons.AlignBottom:
                    ucPalette.SetObjectAalign(enAlign.Bottom);
                    break;
                case enButtons.AlignLeft:
                    ucPalette.SetObjectAalign(enAlign.Left);
                    break;
                case enButtons.AlignRight:
                    ucPalette.SetObjectAalign(enAlign.Right);
                    break;
                case enButtons.Copy:
                    DesignerEvent.OnCopy();
                    break;
                case enButtons.Paste:
                    DesignerEvent.OnPaste();
                    Invalidate();
                    break;
                case enButtons.Cut:
                    DesignerEvent.OnCut();
                    Invalidate();
                    break;
                default:
                    break;
            }
        }
        private void Designer_Load(object sender, EventArgs e)
        {
            this.Size = new Size(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            ucPalette.Size = new Size(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
			DesignerEvent.OnFocusDesigner();
		}

        public void Draw(enDraw draw, object param = null)
        {
            ucPalette.SetMode(enMode.Draw, true);
            ucPalette.SetDrawMode(draw, true);
        }
        public new void Invalidate()
        {
            ucPalette.Invalidate();
            InvalidateRuler();
        }

        #region CustomUI
        public void BarcodeItemManager()
        {

        }
        #endregion
        #region Scroll
        public void InitializeScrollBar()
        {
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
			switch (ModifierKeys)
			{
				case Keys.Control:
					{
						if (e.Delta > 0)
							DesignerVariables.Instance.Paper.ZoomRatio += 0.2f;
						else
							DesignerVariables.Instance.Paper.ZoomRatio -= 0.2f;
						ucPalette.Invalidate();
						InvalidateRuler();
					}
					break;
				default:
					{
						int lines = (e.Delta / 10) * SystemInformation.MouseWheelScrollLines;
						float val = vScrollBar1.Value - lines;
						if (val >= vScrollBar1.Minimum && val <= vScrollBar1.Maximum)
						{
							vScrollBar1.Value -= lines;
						}
						base.OnMouseWheel(e);
					}
					break;
			}
		}
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            vScrollBar1.Value = e.NewValue;
            Refresh();
        }
        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            float val = vScrollBar1.Value * -1;
            ucPalette.Paper.WheelOffetY = val;
            ucPalette.Invalidate();
            InvalidateRuler();
        }
        public void initializeRuler(Paper paper)
        {
            HRuler.InitializeRuler(paper);
            VRuler.InitializeRuler(paper);
        }
        public void InvalidateRuler()
        {
            HRuler.InvalidateRuler(ucPalette.Paper);
            VRuler.InvalidateRuler(ucPalette.Paper);
        }
        #endregion

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            hScrollBar1.Value = e.NewValue;
            Refresh();
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            float val = hScrollBar1.Value * -1;
            ucPalette.Paper.WheelOffetX = val;
            ucPalette.Invalidate();
            InvalidateRuler();
        }
    }
}
