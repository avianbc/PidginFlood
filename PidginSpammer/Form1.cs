using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace aim
{
    public partial class Form1 : Form
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        static extern short VkKeyScan(char ch);

        public const int WM_CHAR = 0x0102;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int VK_RETURN = 0x0D;
        public const int VK_SHIFT = 0x10;
        public const uint MAPVK_VK_TO_CHAR = 0x02;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process[] proc = Process.GetProcessesByName("pidgin");

            if (proc.Length == 0 || proc[0] == null)
            {
                MessageBox.Show("Process not found. Is pidgin running?", "Aborting");
                return;
            }

            if (textBox1.Text == String.Empty)
                return;

            foreach (string s in textBox1.Lines)
            {
                foreach (char c in s)
                {
                    IntPtr val = new IntPtr(VkKeyScan(c));

                    PostMessage(proc[0].MainWindowHandle, WM_KEYDOWN, val, new IntPtr(0));
                    PostMessage(proc[0].MainWindowHandle, WM_CHAR, val, new IntPtr(0));
                    PostMessage(proc[0].MainWindowHandle, WM_KEYUP, val, new IntPtr(0));
                }

                PostMessage(proc[0].MainWindowHandle, WM_KEYDOWN, new IntPtr(VK_RETURN), new IntPtr(0));
                PostMessage(proc[0].MainWindowHandle, WM_KEYUP, new IntPtr(VK_RETURN), new IntPtr(0));
            }
        }
    }
}
