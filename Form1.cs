using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;

namespace ShutdownAfter
{
    public partial class Form1 : Form
    {
        IntPtr watchingWindow = IntPtr.Zero;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            foreach (KeyValuePair<IntPtr, string> window in OpenWindowGetter.GetOpenWindows())
            {
                var item = new ListViewItem(window.Value);
                item.Tag = window.Key;

                if (watchingWindow == window.Key)
                {
                    item.Font = new Font(item.Font, FontStyle.Bold);
                }

                listView1.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                watchingWindow = (IntPtr) listView1.SelectedItems[0].Tag;
            }
            else
            {
                watchingWindow = IntPtr.Zero;
            }
            button1_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                OpenWindowGetter.MakeTopMost((IntPtr)listView1.SelectedItems[0].Tag);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (watchingWindow != IntPtr.Zero)
            {
                var shutdown = true;
                foreach (KeyValuePair<IntPtr, string> window in OpenWindowGetter.GetOpenWindows())
                {
                    if (watchingWindow == window.Key)
                    {
                        shutdown = false;
                        break;
                    }
                }

                if (shutdown)
                {
                    watchingWindow = IntPtr.Zero;
                    Process.Start("shutdown", "/s /t 0");
                }
            }
        }

    }
}
