using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InputHooker
{
    public class WinHook
    {
        public event HookProc KeyboardEvent;
        public event HookProc MouseEvent;

        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr IParam);

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr IParam);

        private List<int> hookProcIds { get; set; }

        public WinHook()
        {
            hookProcIds = new List<int>();
        }

        public bool UseKeyboaredHook { get; set; }
        public bool UseMouseHook { get; set; }
        public void SetHook(Process pro)
        {
            var dllPtr = (IntPtr)WinHook.LoadLibrary("user32");
            int ptr = pro.Threads[0].Id;
            if (UseKeyboaredHook)
            {
                int keyboardHooKId = WinHook.SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL, KeyboardEvent, dllPtr, ptr);
                hookProcIds.Add(keyboardHooKId);
            }
            if (UseMouseHook)
            {
                int mouseHooKId = WinHook.SetWindowsHookEx((int)HookType.WH_MOUSE_LL, MouseEvent, dllPtr, ptr);
                hookProcIds.Add(mouseHooKId);
            }
        }
        public void SetGlobalHook()
        {
            var dllPtr = (IntPtr)WinHook.LoadLibrary("user32");
            if (UseKeyboaredHook)
            { 
                int keyboardHooKId = WinHook.SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL, KeyboardEvent, dllPtr, 0);
                hookProcIds.Add(keyboardHooKId);
            }
            if (UseMouseHook)
            { 
                int mouseHooKId = WinHook.SetWindowsHookEx((int)HookType.WH_MOUSE_LL, MouseEvent, dllPtr, 0);
                hookProcIds.Add(mouseHooKId);
            }
        }
        public void ReleaseHook()
        {
            for (int i = 0; i < hookProcIds.Count; i++)
            {
                if (WinHook.UnhookWindowsHookEx(hookProcIds[i]))
                    hookProcIds.RemoveAt(i);
            }
        }
        public KBDLLHOOKSTRUCT GetKeyboardStatus(nint IParam)
        {
            return Marshal.PtrToStructure<KBDLLHOOKSTRUCT>((IntPtr)IParam);
        }
        public MSLLHOOKSTRUCT GetMouseStatus(nint IParam)
        {
            return Marshal.PtrToStructure<MSLLHOOKSTRUCT>((IntPtr)IParam);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHOOKSTRUCT
    {
        public POINT pt;           // 마우스 좌표
        public uint mouseData;     // 마우스 휠 데이터
        public uint flags;         // 이벤트 플래그
        public uint time;          // 타임스탬프
        public IntPtr dwExtraInfo; // 추가 정보
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct KBDLLHOOKSTRUCT
    {
        public uint vkCode;      // 가상 키 코드 (예: A는 0x41, Ctrl은 0x11)
        public uint scanCode;    // 하드웨어 스캔 코드
        public uint flags;       // 키 이벤트의 상태 플래그 (예: 눌림, 시스템 키 등)
        public uint time;        // 이벤트 발생 시간 (타임스탬프)
        public IntPtr dwExtraInfo; // 추가 정보
    }
    public enum KeyboardFlag
    {
        WM_KEYDOWN = 0x00,
        WM_KEYUP = 0x01,
        WM_ALT = 0x10,
}
    public enum HookType : int
    {
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK = 1,
        WH_KEYBOARD = 2,
        WH_GETMESSAGE = 3,
        WH_CALLWNDPROC = 4,
        WH_CBT = 5,
        WH_SYSMSGFILTER = 6,
        WH_MOUSE = 7,
        WH_HARDWARE = 8,
        WH_DEDBUG = 9,
        WH_SHELL = 10,
        WH_FOREGROUNDIDLE = 11,
        WH_CALLWNDPROCRET = 12,
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14,
    }
    public enum MouseMessages
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WMRBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
    }

}
