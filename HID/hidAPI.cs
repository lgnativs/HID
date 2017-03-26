/**************************************************************************/
/**
* @file     hidAPI.cs
* @brief    USB HID设备通讯类所使用的C/C++函数的C#调用封装
* @version  V2.0
* @date     4.23 2013
*
* @note
* Copyright (C) 2013 lgnativs(apple_eat@126.com). All rights reserved.
*
* @par
 
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


namespace UsbHidDriver
{
    public class HidAttribute : Object
    {
        public UInt16 venderID;
        public UInt16 productID;
        public string serial;
        public UInt16 version;
        public string devicePath;


        public HidAttribute()
        {
        }

        public HidAttribute(UInt16 vID, UInt16 pID, UInt16 ver, string serialString, string path)
        {
            venderID = vID;
            productID = pID;
            version = ver;
            devicePath = path;
            serial = serialString;
        }
    }

    public partial class HidDriver : object
    {
        #region 系统API函数

        #region 查询计算机中设备,并枚举特定设备/接口的win API

        /// <summary>
        /// An SP_DEVINFO_DATA structure defines a device instance that is a member of a device information set.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct SP_DEVINFO_DATA
        {
            /// <summary>
            /// The size, in bytes, of the SP_DEVINFO_DATA structure.
            /// </summary>
            int cbSize;

            /// <summary>
            /// The GUID of the device's setup class.
            /// </summary>
            Guid classGuid;

            /// <summary>
            /// An opaque handle to the device instance (also known as a handle to the devnode).
            /// </summary>
            int devInst;

            /// <summary>
            /// Reserved. For internal use only.
            /// </summary>
            int reserved;
        }

        /// <summary>
        ///     A variable of type DWORD that specifies control options that 
        ///  filter the device information elements that are added to the device information set.
        ///  This parameter can be a bitwise OR of one or more of the following flags. 
        /// </summary>
        enum DIGCF : uint
        {
            /// <summary>
            /// Return a list of installed devices for the specified device setup classes or device interface classes.
            /// </summary>
            DIGCF_DEFAULT = 0x1,

            /// <summary>
            /// Return devices that support device interfaces for the specified device interface classes. 
            /// This flag must be set in the Flags parameter if the Enumerator parameter specifies a device instance ID.
            /// </summary>
            DIGCF_PRESENT = 0x2,

            /// <summary>
            /// Return only the device that is associated with the system default device interface,
            /// if one is set, for the specified device interface classes.
            /// </summary>
            DIGCF_ALLCLASSES = 0x4,

            /// <summary>
            /// Return only devices that are currently present.
            /// </summary>
            DIGCF_PROFILE = 0x8,

            /// <summary>
            /// Return only devices that are a part of the current hardware profile.
            /// </summary>
            DIGCF_DEVICEINTERFACE = 0x10
        }

        /// <summary>
        /// An SP_DEVICE_INTERFACE_DATA structure defines a device interface in a device information set.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            /// <summary>
            /// The size, in bytes, of the SP_DEVICE_INTERFACE_DATA structure.
            /// </summary>
            public Int32 cbSize;

            /// <summary>
            /// The GUID for the class to which the device interface belongs.
            /// </summary>
            public Guid interfaceClassGuid;

            /// <summary>
            /// Can be one or more of the following:
            /// SPINT_ACTIVE
            ///     The interface is active (enabled).
            /// SPINT_DEFAULT
            ///     The interface is the default interface for the device class.
            /// SPINT_REMOVED    
            ///     The interface is removed.
            /// </summary>
            public Int32 flags;

            /// <summary>
            /// Reserved. Do not use.
            /// </summary>
            public Int32 reserved;
        }

        /// <summary>
        /// An SP_DEVICE_INTERFACE_DETAIL_DATA structure contains the path for a device interface.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            /// <summary>
            /// The size, in bytes, of the SP_DEVICE_INTERFACE_DETAIL_DATA structure. 
            /// </summary>
            public int cbSize;

            /// <summary>
            /// A NULL-terminated string(TCHAR) that contains the device interface path. devicePath为字符串的第一个字节;
            /// </summary>
            public short devicePath;
        }

        /// <summary>
        /// 获取一个设备信息集句柄,可以通过指定设备枚举类型和选项控制出现在集合中的项目
        /// The SetupDiGetClassDevs function returns a handle to a device information set 
        /// that contains requested device information elements for a local computer.
        /// </summary>
        /// 
        /// <param name="ClassGuid">A pointer to the GUID for a device setup class or a device interface class. 
        /// This pointer is optional and can be NULL. </param>class
        /// 
        /// <param name="Enumerator">A pointer to a NULL-terminated string that specifies An identifier (ID) of a Plug and Play (PnP) enumerator.
        /// or A PnP device instance ID. When specifying a PnP device instance ID, DIGCF_DEVICEINTERFACE must be set in the Flags parameter.</param>
        /// 
        /// <param name="HwndParent">A handle to the top-level window to be used for a user interface that is associated with installing a device instance in the device information set. 
        /// This handle is optional and can be NULL.</param>
        /// 
        /// <param name="Flags"> DIGCF that specifies control options that filter the device information elements that are added to the device information set. </param>
        /// 
        /// <returns>If the operation succeeds, SetupDiGetClassDevs returns a handle to a device information set 
        /// that contains all installed devices that matched the supplied parameters. If the operation fails, the function returns INVALID_HANDLE_VALUE. To get extended error information, call GetLastError.</returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, uint Enumerator, IntPtr HwndParent, DIGCF Flags);



        /// <summary>
        /// 枚举设备信息集中的包含的接口类列表中的一个接口信息
        /// The SetupDiEnumDeviceInterfaces function enumerates the device interfaces that are contained in a device information set.
        /// </summary>
        /// <param name="DeviceInfoSet">A pointer to a device information set that contains the device interfaces for which to return information.</param>
        /// <param name="DeviceInfoData">A pointer to an SP_DEVINFO_DATA structure that specifies a device information element in DeviceInfoSet. </param>
        /// <param name="interfaceClassGuid">A pointer to a GUID that specifies the device interface class for the requested interface.</param>
        /// <param name="memberIndex">A zero-based index into the list of interfaces in the device information set. </param>
        /// <param name="deviceInterfaceData">A pointer to a caller-allocated buffer 
        /// that contains, on successful return, a completed SP_DEVICE_INTERFACE_DATA structure
        /// that identifies an interface that meets the search parameters. </param>
        /// <returns></returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, ref Guid interfaceClassGuid, UInt32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);


        /// <summary>
        /// 获取接口的详细信息. 调用两次 第1次调用,返回需要的缓冲区长度;第2次调用,传入要求大小的缓冲区,以获取数据 
        /// The SetupDiGetDeviceInterfaceDetail function returns details about a device interface.
        /// </summary>
        /// <param name="deviceInfoSet">A pointer to the device information set that contains the interface for which to retrieve details.</param>
        /// <param name="deviceInterfaceData">A pointer to an SP_DEVICE_INTERFACE_DATA structure that specifies the interface in DeviceInfoSet for which to retrieve details.</param>
        /// <param name="deviceInterfaceDetailData">A pointer to an SP_DEVICE_INTERFACE_DETAIL_DATA structure to receive information about the specified interface. </param>
        /// <param name="deviceInterfaceDetailDataSize">The size of the DeviceInterfaceDetailData buffer. </param>
        /// <param name="requiredSize">A pointer to a variable of type DWORD that receives the required size of the DeviceInterfaceDetailData buffer. </param>
        /// <param name="deviceInfoData">A pointer to a buffer that receives information about the device that supports the requested interface.</param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData,
            uint deviceInterfaceDetailDataSize, out uint requiredSize, IntPtr deviceInfoData);
        #endregion

        #region 文件操作 win API
        /// <summary>
        /// 文件打开模式
        /// The access to the object, which can be summarized as read, write, both, or neither (zero).
        /// </summary>
        enum FILE_ACCESS_MASK : uint
        {
            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000,
            GENERIC_EXECUTE = 0x20000000,
        }

        /// <summary>
        /// 文件共享模式
        /// The sharing mode of an object, which can be read, write, both, delete, all of these, or none: 0, 
        /// </summary>
        enum FILE_SHARE_MODE : uint
        {
            FILE_SHARE_DELETE = 0x00000004,
            FILE_SHARE_READ = 0x00000001,
            FILE_SHARE_WRITE = 0x00000002,
            FILE_SHARE_NONE = 0x00000000,
        }

        /// <summary>
        /// An action to take on files that exist and do not exist. 
        /// </summary>
        enum FILE_CREATION_DISPOSITON : uint
        {
            CREATE_ALWAYS = 2,
            CREATE_NEW = 1,
            OPEN_ALWAYS = 4,
            OPEN_EXISTING = 3,
            TRUNCATE_EXISTING = 5,
        }

        enum FILE_FLAG_ATTRIBUTES : uint
        {
            FILE_ATTRIBUTE_ARCHIVE = 0x20,                          // The file should be archived. Applications use this attribute to mark files for backup or removal.
            FILE_ATTRIBUTE_ENCRYPTED = 0x4000,                 // The file or directory is encrypted. For a file, this means that all data in the file is encrypted. 
                                                                                             // For a directory, this means that encryption is the default for newly created files and subdirectories
            FILE_ATTRIBUTE_HIDDEN = 0x2,                            // The file is hidden. Do not include it in an ordinary directory listing.
            FILE_ATTRIBUTE_NORMAL = 0x80,                        // The file does not have other attributes set. This attribute is valid only if used alone.
            FILE_FLAG_OVERLAPPED = 0x40000000,               // The file or device is being opened or created for asynchronous I/O.
        }

        /// <summary>
        /// 创建文件,用以打开一个端口/文件/接口
        /// Creates or opens a file, file stream, or directory as a transacted operation.
        /// </summary>
        /// <param name="lpFileName">he name of an object to be created or opened.</param>
        /// <param name="dwDesiredAccess">The access to the object, which can be summarized as read, write, both, or neither (zero). </param>
        /// <param name="dwShareMode">The sharing mode of an object, which can be read, write, both, delete, all of these, or none</param>
        /// <param name="lpSecurityAttributes">The parameter can be NULL.</param>
        /// <param name="dwCreationDisposition">An action to take on files that exist and do not exist. </param>
        /// <param name="dwFlagsAndAttributes">The file attributes and flags.set 0</param>
        /// <param name="hTemplateFile"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(
            string lpFileName,                                        // file name
            FILE_ACCESS_MASK dwDesiredAccess,        // access mode
            FILE_SHARE_MODE dwShareMode,              // share mode
            uint lpSecurityAttributes,                             // SD
            FILE_CREATION_DISPOSITON dwCreationDisposition,                 // how to create
            FILE_FLAG_ATTRIBUTES dwFlagsAndAttributes,                           // file attributes
            uint hTemplateFile                                          // handle to template file
            );

        /// <summary>
        /// 关闭一个文件/句柄对象
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        static extern int CloseHandle(IntPtr hObject);


        /// <summary>
        /// 文件读操作
        /// </summary>
        /// <param name="hFile"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="nNumberOfBytesToRead"></param>
        /// <param name="lpNumberOfBytesRead"></param>
        /// <param name="lpOverlapped"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern bool ReadFile
            (
                IntPtr hFile,
                byte[] lpBuffer,
                uint nNumberOfBytesToRead,
                ref uint lpNumberOfBytesRead,
                IntPtr lpOverlapped
            );

        /// <summary>
        /// 文件写操作
        /// </summary>
        /// <param name="hFile"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="nNumberOfBytesToWrite"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <param name="lpOverlapped"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern bool WriteFile
            (
                IntPtr hFile,
                byte[] lpBuffer,
                uint nNumberOfBytesToWrite,
                ref uint lpNumberOfBytesWritten,
                IntPtr lpOverlapped
            );

        /// <summary>
        /// Cancels all pending input and output (I/O) operations that are issued by the calling thread for the specified file. 
        /// </summary>
        /// <param name="hFile"></param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero (0). </returns>
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CancelIo(IntPtr hFile);

        #endregion

        #region HID操作的API

        /// <summary>
        /// The HIDD_ATTRIBUTES structure contains vendor information about a HIDClass device
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct HIDD_ATTRIBUTES
        {
            public Int32 Size;
            public UInt16 VendorID;
            public UInt16 ProductID;
            public UInt16 VersionNumber;
        }

        /// <summary>
        /// The HIDP_CAPS structure contains information about a top-level collection's capability.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct HIDP_CAPS
        {
            internal Int16 Usage;
            internal Int16 UsagePage;
            internal Int16 InputReportByteLength;
            internal Int16 OutputReportByteLength;
            internal Int16 FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            internal Int16[] Reserved;
            internal Int16 NumberLinkCollectionNodes;
            internal Int16 NumberInputButtonCaps;
            internal Int16 NumberInputValueCaps;
            internal Int16 NumberInputDataIndices;
            internal Int16 NumberOutputButtonCaps;
            internal Int16 NumberOutputValueCaps;
            internal Int16 NumberOutputDataIndices;
            internal Int16 NumberFeatureButtonCaps;
            internal Int16 NumberFeatureValueCaps;
            internal Int16 NumberFeatureDataIndices;
        }

        /// <summary>
        /// The HidD_GetHidGuid routine returns the device interfaceGUID for HIDClass devices.
        /// </summary>
        /// <param name="HidGuid">Pointer to a caller-allocated GUID buffer that the routine uses to return the device interface GUID for HIDClass devices.</param>
        [DllImport("hid.dll")]
        static extern void HidD_GetHidGuid(out Guid HidGuid);

        /// <summary>
        /// 获取HID设备的属性信息
        /// The HidD_GetAttributes routine returns the attributes of a specified top-level collection.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="Attributes">Pointer to a caller-allocated HIDD_ATTRIBUTES structure that returns the attributes of the collection specified by HidDeviceObject.</param>
        /// <returns></returns>
        [DllImport("hid.dll")]
        static extern Boolean HidD_GetAttributes(IntPtr HidDeviceObject, out HIDD_ATTRIBUTES Attributes);

        /// <summary>
        /// 获取HID设备的序列号字符串
        /// The HidD_GetSerialNumberString routine returns the embedded string of a top-level collection that identifies the serial number of the collection's physical device.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="Buffer">Pointer to a caller-allocated buffer that the routine uses to return the requested serial number string. </param>
        /// <param name="BufferLength">Specifies the length, in bytes, of a caller-allocated buffer provided at Buffer. </param>
        /// <returns></returns>
        [DllImport("hid.dll", CharSet = CharSet.Auto)]
        static extern Boolean HidD_GetSerialNumberString(IntPtr HidDeviceObject, StringBuilder Buffer, int BufferLength);

        /// <summary>
        /// The HidD_FlushQueue routine deletes all pending input reports in a top-level collection's input queue.
        /// </summary>
        /// <param name="HidDeviceObject"></param>
        /// <returns></returns>
        [DllImport("hid.dll")]
        static extern Boolean HidD_FlushQueue(IntPtr HidDeviceObject);

        /// <summary>
        /// The HidD_GetPreparsedData routine returns a top-level collection's preparsed data.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="PreparsedData">Pointer to the address of a routine-allocated buffer that contains a collection's preparsed data in a _HIDP_PREPARSED_DATA structure.</param>
        /// <returns></returns>
        [DllImport("hid.dll")]
        static extern Boolean HidD_GetPreparsedData(IntPtr HidDeviceObject, ref IntPtr PreparsedData);

        /// <summary>
        /// The HidD_FreePreparsedData routine releases the resources that the HID class driver allocated to hold a top-level collection's preparsed data.
        /// </summary>
        /// <param name="PreparsedData">Pointer to the buffer, returned by HidD_GetPreparsedData, that is freed.</param>
        /// <returns></returns>
        [DllImport("hid.dll")]
        static extern Boolean HidD_FreePreparsedData(IntPtr PreparsedData);

        /// <summary>
        /// The HidP_GetCaps routine returns a top-level collection's HIDP_CAPS structure.
        /// </summary>
        /// <param name="PreparsedData">Pointer to a top-level collection's preparsed data.</param>
        /// <param name="Capabilities">Pointer to a caller-allocated buffer 
        /// that the routine uses to return a collection's HIDP_CAPS structure.</param>
        /// <returns></returns>
        [DllImport("hid.dll")]
        static extern Boolean HidP_GetCaps(IntPtr PreparsedData, ref HIDP_CAPS Capabilities);

        /// <summary>
        /// The HidD_GetInputReport routine returns an input reports from a top-level collection.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="ReportBuffer">Pointer to a caller-allocated input report buffer 
        /// that the caller uses to specify a HID report ID and HidD_GetInputReport uses to return the specified input report.</param>
        /// <param name="ReportBufferLength">Specifies the size, in bytes, of the report buffer. 
        /// The report buffer must be large enough to hold the input report -- excluding its report ID, if report IDs are used -- plus one additional byte that specifies a nonzero report ID or zero.</param>
        /// <returns>If HidD_SetOutputReport succeeds, it returns TRUE; otherwise, it returns FALSE.</returns>
        [DllImport("hid.dll")]
        static extern Boolean HidD_GetInputReport(IntPtr HidDeviceObject, out IntPtr ReportBuffer, ulong ReportBufferLength);


        /// <summary>
        /// he HidD_SetOutputReport routine sends an output report to a top-level collection.
        /// </summary>
        /// <param name="HidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="ReportBuffer">Pointer to a caller-allocated output report buffer that the caller uses to specify a report ID.</param>
        /// <param name="ReportBufferLength">Specifies the size, in bytes, of the report buffer. 
        /// The report buffer must be large enough to hold the output report -- excluding its report ID, if report IDs are used -- plus one additional byte that specifies a nonzero report ID or zero.</param>
        /// <returns>If HidD_SetOutputReport succeeds, it returns TRUE; otherwise, it returns FALSE.</returns>
        [DllImport("hid.dll")]
        static extern Boolean HidD_SetOutputReport(IntPtr HidDeviceObject, IntPtr ReportBuffer, ulong ReportBufferLength);

        #endregion

        #region 注册设备PnP通知 API
        /// <summary>
        /// The device type, which determines the event-specific information that follows the first three members.
        /// </summary>
        enum DBCH_DEVICETYPE : uint
        {
            /// <summary>
            /// Class of devices.This structure is a DEV_BROADCAST_DEVICEINTERFACE structure.
            /// </summary>
            DBT_DEVTYP_DEVICEINTERFACE = 0x00000005,

            /// <summary>
            /// File system handle. This structure is a DEV_BROADCAST_HANDLE structure.
            /// </summary>
            DBT_DEVTYP_HANDLE = 0x00000006,

            /// <summary>
            /// OEM- or IHV-defined device type. This structure is a DEV_BROADCAST_OEM structure.
            /// </summary>
            DBT_DEVTYP_OEM = 0x00000000,

            /// <summary>
            /// Port device (serial or parallel). This structure is a DEV_BROADCAST_PORT structure.
            /// </summary>
            DBT_DEVTYP_PORT = 0x00000003,

            /// <summary>
            /// Logical volume. This structure is a DEV_BROADCAST_VOLUME structure.
            /// </summary>
            DBT_DEVTYP_VOLUME = 0x00000002,
        }

        /// <summary>
        /// Serves as a standard header for information related to a device event reported through the WM_DEVICECHANGE message.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct DEV_BROADCAST_HDR
        {
            /// <summary>
            /// The size of this structure, in bytes.
            /// </summary>
            public uint dbcc_size;

            /// <summary>
            /// The device type,which determines the event-specific information that follows the first three members.
            /// </summary>
            public DBCH_DEVICETYPE dbcc_devicetype;

            /// <summary>
            /// Reserved; do not use.
            /// </summary>
            public uint dbcc_reserved;
        }

        /// <summary>
        /// Contains information about a class of devices.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        struct DEV_BROADCAST_DEVICEINTERFACE
        {
            /// <summary>
            /// The size of this structure, in bytes. This is the size of the members plus the actual length of the dbcc_name string 
            /// </summary>
            public uint dbcc_size;

            /// <summary>
            /// Set to DBT_DEVTYP_DEVICEINTERFACE.
            /// </summary>
            public DBCH_DEVICETYPE dbcc_devicetype;

            /// <summary>
            /// Reserved; do not use.
            /// </summary>
            public uint dbcc_reserved;

            /// <summary>
            /// The GUID for the interface device class.
            /// </summary>
            public Guid dbcc_classguid;

            /// <summary>
            /// A null-terminated string that specifies the name of the device.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
            public string dbcc_name;
        }

        /// <summary>
        /// Registers the device or type of device for which a window will receive notifications.
        /// </summary>
        /// <param name="hRecipient">A handle to the window or service that will receive device events for the devices specified in the NotificationFilter parameter. </param>
        /// <param name="NotificationFilter">A pointer to a block of data that specifies the type of device for which notifications should be sent. </param>
        /// <param name="Flags">This parameter can be one of the following values.</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, uint Flags);

        /// <summary>
        /// Closes the specified device notification handle.
        /// </summary>
        /// <param name="Handle">Device notification handle returned by the RegisterDeviceNotification function.</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        static extern Boolean UnregisterDeviceNotification(IntPtr Handle);

        #endregion

        #endregion

    }
}

