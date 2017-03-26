using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Runtime.InteropServices;
using System.IO;

using UsbHidDriver;

namespace HID
{
    public struct HIDINFO
    {
        public string path;
        public short PID;
        public short VID;
        public string serialString;
    };

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        HidDriver myHidDevice = new HidDriver();

        HidAttribute[] hids = null;

        private void AddHidList(HidAttribute[] hids)
        {
            hidTreeView.BeginUpdate();
            hidTreeView.Nodes.Clear();
            hidTreeView.Nodes.Add("HID Device List");
            for(int i = 0; i < hids.Length; i++)
            {
                hidTreeView.Nodes[0].Nodes.Add(hids[i].devicePath);
                hidTreeView.Nodes[0].Nodes[i].Nodes.Add(hids[i].venderID.ToString("X4"));
                hidTreeView.Nodes[0].Nodes[i].Nodes.Add(hids[i].productID.ToString("X4"));
                hidTreeView.Nodes[0].Nodes[i].Nodes.Add(hids[i].serial);
            }
            hidTreeView.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hids = HidDriver.GetHidDevice();
            AddHidList(hids);

        }

        private void Open_Click(object sender, EventArgs e)
        {
            //int i = listView1.SelectedItems[0].Index;
            //myHidDevice.OpenDevice(hid[i].venderID, hid[i].productID, hid[i].serial);
            //myHidDevice.DataReceivedHandle += new EventHandler<DataReceivedArgs>(OnDataRecieved);
             //timer1.Start();
        }

        void OnDataRecieved(object sender, DataReceivedArgs e)
        {
            uint t = e.Packet[1];
            //t = t * 256 + e.Packet[2];
            //t = t * 256 + e.Packet[3];
            //t = t * 256 + e.Packet[4];
            textBox1.Text = t.ToString();
            //dataGridView1.Rows.Add(t);
        }

        private void SetTrue_Click(object sender, EventArgs e)
        {
            // 构建发送数据包
            byte[] sendReport = new byte[65];

            sendReport[0] = 0;      // report id
            sendReport[1] = 2;
            sendReport[2] = 0;

            if (myHidDevice.Write(sendReport) == false)
                MessageBox.Show("Send Failed");
        }

        private void SetFalse_Click(object sender, EventArgs e)
        {
            // 构建发送数据包
            byte[] sendReport = new byte[65];

            sendReport[0] = 0;
            sendReport[1] = 2;
            sendReport[2] = 1;
            if (myHidDevice.Write(sendReport) == false)
                MessageBox.Show("Send Failed");
        }

        byte led = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (led == 0)
                led = 1;
            else
                led = 0;
            // 构建发送数据包
            byte[] sendReport = new byte[65];

            sendReport[0] = 0;
            sendReport[1] = led;

            if (myHidDevice.Write(sendReport) == false)
                MessageBox.Show("Send Failed");
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myHidDevice.DataReceivedHandle -= OnDataRecieved;
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void hidTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == hidTreeView.TopNode)
            {
                HidDriver myHid = new HidDriver();
                myHid.OpenDevice(e.Node.Text);
                textBox1.Text = myHid.GetDeviceCapabilities();
            }
            else
                textBox1.Text = "";

        }
    }
}
