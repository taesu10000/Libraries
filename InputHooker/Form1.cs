using System.Diagnostics;
using System.Runtime.InteropServices;

namespace InputHooker
{
    public partial class Form1 : Form
    {
        private WinHook winHook;

        public bool UseLog { get; private set; }

        public Form1()
        {
            InitializeComponent();


            winHook = new WinHook();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {

            winHook.UseKeyboaredHook = chkUseKeyboardHook.Checked;
            winHook.UseMouseHook = chkUseMouseHook.Checked;

            winHook.KeyboardEvent += WinHook_KeyboardEvent;
            winHook.MouseEvent += WinHook_MouseEvent;

            winHook.SetGlobalHook();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            winHook.ReleaseHook();
            winHook.KeyboardEvent -= WinHook_KeyboardEvent;
            winHook.MouseEvent -= WinHook_MouseEvent;
        }

        private int WinHook_KeyboardEvent(int nCode, nint wParam, nint IParam)
        {
            if (nCode >= 0)
            {
                // IParam(lParam)을 KBDLLHOOKSTRUCT로 변환
                KBDLLHOOKSTRUCT keyboardStruct = winHook.GetKeyboardStatus(IParam);
                var msg = $"{(Keys)keyboardStruct.vkCode} {(KeyboardFlag)keyboardStruct.flags == KeyboardFlag.WM_KEYDOWN}";
                msg = string.Format($"\r\n{DateTime.Now.ToString("[yy-MM-dd:fff]")} {msg}");
                richTextBox1.AppendText(msg);
            }
            return 0;
        }
        private int WinHook_MouseEvent(int nCode, nint wParam, nint IParam)
        {
            if (nCode >= 0)
            {
                // IParam을 MSLLHOOKSTRUCT로 변환
                MSLLHOOKSTRUCT mouseStruct = winHook.GetMouseStatus(IParam);

                // 좌표 정보 추출
                int x = mouseStruct.pt.x;
                int y = mouseStruct.pt.y;

                var coordinates = $"Mouse Coordinates: X={x}, Y={y}";
                toolStripStatusLabel1.Text = coordinates;
            }

            return 0;
        }

        private void chkUseKeyboardHook_CheckedChanged(object sender, EventArgs e)
        {
            winHook.UseKeyboaredHook = chkUseKeyboardHook.Checked;
        }

        private void chkUseMouseHook_CheckedChanged(object sender, EventArgs e)
        {
            winHook.UseMouseHook = chkUseMouseHook.Checked;
        }

        private void chkShowLog_CheckedChanged(object sender, EventArgs e)
        {
            UseLog = chkShowLog.Checked;
        }
    }
}
