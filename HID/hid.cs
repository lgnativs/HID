/**************************************************************************/
/**
* @file     hid.cs
* @brief    USB HID自定义设备通讯类
* @version  V2.0
* @date     4.23 2013
*
* @note
* Copyright (C) 2013 lgnativs(apple_eat@126.com). All rights reserved.
*
* @par
* 此文件实现对任意USB HID自定义设备的驱动类.可以提供HID设备的打开,关闭,读写等功能.
 * 系统默认:
* HID输入报告符字长为64,Report ID = 0
* HID输出报告符字长为64,Report ID = 0
 
* This file can be freely distributed 
*
* @par
* THIS SOFTWARE IS PROVIDED "AS IS".  NO WARRANTIES, WHETHER EXPRESS, IMPLIED
* OR STATUTORY, INCLUDING, BUT NOT LIMITED TO, IMPLIED WARRANTIES OF
* MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE APPLY TO THIS SOFTWARE.
* ARM SHALL NOT, IN ANY CIRCUMSTANCES, BE LIABLE FOR SPECIAL, INCIDENTAL, OR
* CONSEQUENTIAL DAMAGES, FOR ANY REASON WHATSOEVER.
*
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.ComponentModel;

namespace UsbHidDriver
{
    public class DataReceivedArgs : EventArgs
    {
        public DataReceivedArgs(byte[] p)
        {
            packet = p;
        }

        private byte[] packet;

        public byte[] Packet
        {
            get { return packet; }
        }
    }

    public partial class HidDriver : object
    {
        #region 属性
        /// <summary>
        /// 指示是否已经打开HID设备
        /// </summary>
        private bool isOpened = false;

        public bool opened
        {
            get { return isOpened; }
        }

        /// <summary>
        /// 保存指定了Vid,Pid的HID设备属性信息的数组
        /// </summary>
        private List<HidAttribute> myHidDevice = null;

        /// <summary>
        /// 以读访问权限打开的HID设备句柄
        /// </summary>
        private IntPtr hidReadHandle = IntPtr.Zero;

        /// <summary>
        /// 以写访问权限打开的HID设备句柄
        /// </summary>
        private IntPtr hidWriteHandle = IntPtr.Zero;

        /// <summary>
        /// 设备的属性
        /// </summary>
        private HIDP_CAPS capabilitices = new HIDP_CAPS();
        #endregion

        /// <summary>
        ///  构造函数
        /// </summary>
        public HidDriver()
        {

        }

        static public HidAttribute[] GetHidDevice()
        {
            List<HidAttribute> hidAttributeList = new List<HidAttribute>();

            // 获取HID接口的全局GUID
            Guid hidGuid = Guid.Empty;
            HidD_GetHidGuid(out hidGuid);

            // 获得HID接口设备信息集
            IntPtr hidInfoSet = SetupDiGetClassDevs(ref hidGuid, 0, IntPtr.Zero, DIGCF.DIGCF_PRESENT | DIGCF.DIGCF_DEVICEINTERFACE);

            // 如果信息集不为空,则枚举其中的每一个接口信息
            if (hidInfoSet != null)
            {
                SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
                deviceInterfaceData.cbSize = Marshal.SizeOf(deviceInterfaceData);
                uint memberIndex = 0;
                for (memberIndex = 0; memberIndex < 64; memberIndex++)
                {
                    if (SetupDiEnumDeviceInterfaces(hidInfoSet, IntPtr.Zero, ref hidGuid, memberIndex, ref deviceInterfaceData))
                    {
                        // 获取接口的详细信息
                        // 第一传递为空的缓冲区,会导致读取错误,但可以获得接口信息需求的缓冲区长度
                        uint requiredSize = 0;
                        SetupDiGetDeviceInterfaceDetail(hidInfoSet, ref deviceInterfaceData, IntPtr.Zero, 0, out requiredSize, IntPtr.Zero);

                        // 分配存放接口详细信息的非托管缓冲区
                        IntPtr buffer = Marshal.AllocHGlobal((int)requiredSize);
                        SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                        deviceInterfaceDetailData.cbSize = Marshal.SizeOf(deviceInterfaceDetailData);

                        // 将结构体内容的部分拷贝进非托管缓冲区内
                        Marshal.StructureToPtr(deviceInterfaceDetailData, buffer, false);

                        // 第二次读取,获得接口的详细信息;
                        if (SetupDiGetDeviceInterfaceDetail(hidInfoSet, ref deviceInterfaceData, buffer, requiredSize, out requiredSize, IntPtr.Zero))
                        {
                            // 将非托管缓冲区内容封装到托管结构体内
                            //Marshal.PtrToStructure(buffer, deviceInterfaceDetailData);

                            // 偏移一个cbSize(4字节),是设备接口路径字符串的开始地址;
                            String devicePath = Marshal.PtrToStringAuto(buffer + 4);

                            // 打开此接口,以检查是否是指定的hid设备
                            IntPtr hid = CreateFile(devicePath, FILE_ACCESS_MASK.GENERIC_READ | FILE_ACCESS_MASK.GENERIC_WRITE,
                                FILE_SHARE_MODE.FILE_SHARE_READ, 0, FILE_CREATION_DISPOSITON.OPEN_EXISTING, 0, 0);

                            // 设备打开成功, 检查打开的HID设备是否是指定的
                            if (hid != null)
                            {  
                                HIDD_ATTRIBUTES hidAttributes = new HIDD_ATTRIBUTES();
                                hidAttributes.Size = Marshal.SizeOf(hidAttributes);
                                if (HidD_GetAttributes(hid, out hidAttributes))
                                {
                                    StringBuilder deviceSerialString = new StringBuilder(128); ;
                                    string serial = null;
                                    if (HidD_GetSerialNumberString(hid, deviceSerialString, deviceSerialString.Capacity))
                                    {
                                        serial = deviceSerialString.ToString();
                                    }
                                    HidAttribute n = new HidAttribute(hidAttributes.VendorID, hidAttributes.ProductID, hidAttributes.VersionNumber, serial, devicePath);
                                    hidAttributeList.Add(n);
                                }
                                
                                // 关闭打开的HID设备
                                CloseHandle(hid);
                            }
                        }
                        // 释放分配的非托管缓冲区
                        Marshal.FreeHGlobal(buffer);
                    }
                }
            }
            return hidAttributeList.ToArray();
        }

        /// <summary>
        /// 查找指定Vid,Pid的HID设备的个数
        /// </summary>
        /// <param name="vID">设备的vID</param>
        /// <param name="pID">设备的pID</param>
        /// <returns></returns>
        public int GetDeviceNumber(short vID, short pID)
        {
            HidAttribute[] hidDevices = GetHidDevice();
            foreach (HidAttribute hid in hidDevices)
            {
                if (hid.venderID == vID && hid.productID == pID)
                {
                    myHidDevice.Add(hid);
                }
            }
            return myHidDevice.Count;
        }


        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool OpenDevice(int index)
        {
            bool ret = false;
            if (index < myHidDevice.Count && isOpened == false)
            {
                ret = CreateHandle(myHidDevice[index].devicePath);
            }
            return ret;
        }

        /// <summary>
        /// 打开指定的vid,pid,serial USB HID设备
        /// </summary>
        /// <param name="vid">供应商ID</param>
        /// <param name="pid">产品ID</param>
        /// <param name="serial">产品的序列号</param>
        /// <returns></returns>
        public bool OpenDevice(UInt16 vid, UInt16 pid, string serial)
        {
            if (isOpened == true)
                return false;

            HidAttribute[] hidDevices = GetHidDevice();
            bool ret = false;
            foreach (HidAttribute hid in hidDevices)
            {
                if (hid.venderID == vid && hid.productID == pid && hid.serial == serial)
                {
                    ret = CreateHandle(hid.devicePath);
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// 按指定的设备路径打开设备
        /// </summary>
        /// <param name="devicePath"></param>
        /// <returns></returns>
        public bool OpenDevice(string devicePath)
        {
            if (isOpened)
                return false;
            return CreateHandle(devicePath);
        }

        public string GetDeviceCapabilities()
        {
            string strCaps = "";
            IntPtr pPreparsedData = new IntPtr();
            if (HidD_GetPreparsedData(hidWriteHandle, ref pPreparsedData))
            {
                if (HidP_GetCaps(pPreparsedData, ref capabilitices))
                {
                    strCaps += String.Format("Usage = {0},\r\n", capabilitices.Usage);
                    strCaps += String.Format("UsagePage = {0},\r\n", capabilitices.UsagePage);
                    strCaps += String.Format("InputReportByteLength = {0},\r\n", capabilitices.InputReportByteLength);
                    strCaps += String.Format("OutputReportByteLength = {0},\r\n", capabilitices.OutputReportByteLength);
                    strCaps += String.Format("FeatureReportByteLength = {0},\r\n", capabilitices.FeatureReportByteLength);
                    strCaps += String.Format("NumberLinkCollectionNodes = {0},\r\n", capabilitices.NumberLinkCollectionNodes);
                    strCaps += String.Format("NumberInputButtonCaps = {0},\r\n", capabilitices.NumberInputButtonCaps);
                    strCaps += String.Format("NumberInputValueCaps = {0},\r\n", capabilitices.NumberInputValueCaps);
                    strCaps += String.Format("NumberInputDataIndices = {0},\r\n", capabilitices.NumberOutputButtonCaps);
                    strCaps += String.Format("NumberOutputButtonCaps = {0},\r\n", capabilitices.NumberOutputValueCaps);
                    strCaps += String.Format("NumberOutputValueCaps = {0},\r\n", capabilitices.NumberOutputValueCaps);
                    strCaps += String.Format("NumberOutputDataIndices = {0},\r\n", capabilitices.NumberOutputDataIndices);
                    strCaps += String.Format("NumberFeatureButtonCaps = {0},\r\n", capabilitices.NumberInputButtonCaps);
                    strCaps += String.Format("NumberFeatureValueCaps = {0},\r\n", capabilitices.NumberFeatureValueCaps);
                    strCaps += String.Format("NumberFeatureDataIndices = {0},\r\n", capabilitices.NumberFeatureDataIndices);
                }
                HidD_FreePreparsedData(pPreparsedData);
            }
            return strCaps;
        }

        private bool CreateHandle(string lpFileName)
        {
            bool ret = false;

            if (isOpened == false)
            {
                // 以读访问打开此HID设备;
                hidReadHandle = CreateFile(lpFileName, FILE_ACCESS_MASK.GENERIC_READ,
                    FILE_SHARE_MODE.FILE_SHARE_WRITE | FILE_SHARE_MODE.FILE_SHARE_READ, 0, FILE_CREATION_DISPOSITON.OPEN_EXISTING, 0, 0);

                // 以写访问打开此HID设备
                hidWriteHandle = CreateFile(lpFileName, FILE_ACCESS_MASK.GENERIC_WRITE,
                    FILE_SHARE_MODE.FILE_SHARE_WRITE | FILE_SHARE_MODE.FILE_SHARE_READ, 0, FILE_CREATION_DISPOSITON.OPEN_EXISTING, 0, 0);

                if (hidReadHandle != null && hidWriteHandle != null)
                {
                    StartBackgroundRead();
                    isOpened = true;
                    ret = true;
                }
            }
            return ret;
        }

        //public bool Write(byte[] outPacket)
        //{
        //    if (isOpened)
        //    {
        //        uint bytesWritten = 0;
        //        bool ret = false;
        //        ret = WriteFile(hidWriteHandle, outPacket, (uint)outPacket.Length, ref bytesWritten, IntPtr.Zero);
        //        return ret;
        //    }
        //    return false;
        //}
        public bool Write(byte[] outPacket)
        {
            if (isOpened)
            {
                uint bytesWritten = 0;
                bool ret = false;
                //IntPtr pBuff = Marshal.AllocHGlobal(outPacket.Length);
                //Marshal.Copy(outPacket, 0, pBuff, outPacket.Length);
                //ret = HidD_SetOutputReport(hidWriteHandle, pBuff, (ulong)outPacket.Length);
                ret = WriteFile(hidWriteHandle, outPacket, (uint)outPacket.Length, ref bytesWritten, IntPtr.Zero);
                return ret;
            }
            return false;
        }

        public bool Read(ref byte[] inPacket)
        {
            if (isOpened)
            {
                uint bytesRead = 0;
                inPacket[0] = 0;                      // 默认Report ID;        
                return ReadFile(hidReadHandle, inPacket, (uint)inPacket.Length, ref bytesRead, IntPtr.Zero);
            }
            return false;
        }

        ~HidDriver()
        {
            if (isOpened)
            {
                CloseHandle(hidReadHandle);
                CloseHandle(hidWriteHandle);
            }
        }

        #region 多线程读
        private void StartBackgroundRead()
        {
            BackgroundWorker readWork = new BackgroundWorker();
            readWork.WorkerReportsProgress = true;
            readWork.ProgressChanged += new ProgressChangedEventHandler(readWork_ProgressChanged);
            readWork.RunWorkerCompleted += new RunWorkerCompletedEventHandler(readWork_RunWorkerCompleted);
            readWork.DoWork += new DoWorkEventHandler(readWork_DoWork);
            readWork.RunWorkerAsync();
        }

        byte[] inPacket = new byte[65];

        void readWork_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (isOpened)
            {
                while (true)
                {
                    uint bytesRead = 0;
                    inPacket[0] = 0;                      // 默认Report ID;        
                    if (ReadFile(hidReadHandle, inPacket, (uint)inPacket.Length, ref bytesRead, IntPtr.Zero))
                    {
                        worker.ReportProgress(1);
                    }
                }
            }
        }

        void readWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<DataReceivedArgs> DataReceivedHandle;

        void readWork_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DataReceivedArgs i = new DataReceivedArgs(inPacket);
            if (DataReceivedHandle != null)
            {
                DataReceivedHandle(this, i);
            }
        }
        #endregion

        //#region 设备连接移除处理
        //private IntPtr RegisterHidNotification(

        //#endregion
    }
}
