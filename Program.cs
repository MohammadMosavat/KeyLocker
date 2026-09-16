using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace KeyLocker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new TrayAppContext());
        }
    }

    public class TrayAppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private ToolStripMenuItem toggleItem;
        private KeyboardHook hook;
        private bool locked = false;

        public TrayAppContext()
        {
            hook = new KeyboardHook();

            var menu = new ContextMenuStrip();
            toggleItem = new ToolStripMenuItem("Lock Keyboard", null, OnToggle);
            menu.Items.Add(toggleItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Exit", null, OnExit);

            trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Shield,
                Text = "KeyLocker - Unlocked",
                Visible = true,
                ContextMenuStrip = menu
            };

            // Left-click tray icon also toggles (quick access)
            trayIcon.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                    ToggleLock();
            };
        }

        private void OnToggle(object sender, EventArgs e) => ToggleLock();

        private void ToggleLock()
        {
            locked = !locked;

            if (locked)
            {
                hook.Install();
                trayIcon.Icon = SystemIcons.Error; // red-ish visual cue
                trayIcon.Text = "KeyLocker - LOCKED (click to unlock)";
                toggleItem.Text = "Unlock Keyboard";
                trayIcon.ShowBalloonTip(1500, "KeyLocker", "Keyboard is now LOCKED. Click tray icon to unlock.", ToolTipIcon.Info);
            }
            else
            {
                hook.Uninstall();
                trayIcon.Icon = SystemIcons.Shield;
                trayIcon.Text = "KeyLocker - Unlocked";
                toggleItem.Text = "Lock Keyboard";
                trayIcon.ShowBalloonTip(1500, "KeyLocker", "Keyboard unlocked.", ToolTipIcon.Info);
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            hook.Uninstall();
            trayIcon.Visible = false;
            Application.Exit();
        }
    }

    public class KeyboardHook
    {
        private const int WH_KEYBOARD_LL = 13;
        private IntPtr hookId = IntPtr.Zero;
        private LowLevelKeyboardProc proc;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public KeyboardHook()
        {
            proc = HookCallback;
        }

        public void Install()
        {
            if (hookId != IntPtr.Zero) return;
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                hookId = SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public void Uninstall()
        {
            if (hookId == IntPtr.Zero) return;
            UnhookWindowsHookEx(hookId);
            hookId = IntPtr.Zero;
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // Swallow every key event: return non-zero (1) instead of calling next hook.
            if (nCode >= 0)
            {
                return (IntPtr)1;
            }
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}