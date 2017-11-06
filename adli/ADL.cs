using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace adli
{
    internal delegate IntPtr ADL_Main_Memory_Alloc(int size);

    [StructLayout(LayoutKind.Sequential)]
    internal struct ADLAdapterInfo
    {
        /// <summary>The size of the structure</summary>
        int Size;
        /// <summary> Adapter Index</summary>
        internal int AdapterIndex;
        /// <summary> Adapter UDID</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ADL.ADL_MAX_PATH)]
        internal string UDID;
        /// <summary> Adapter Bus Number</summary>
        internal int BusNumber;
        /// <summary> Adapter Driver Number</summary>
        internal int DriverNumber;
        /// <summary> Adapter Function Number</summary>
        internal int FunctionNumber;
        /// <summary> Adapter Vendor ID</summary>
        internal int VendorID;
        /// <summary> Adapter Adapter name</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ADL.ADL_MAX_PATH)]
        internal string AdapterName;
        /// <summary> Adapter Display name</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ADL.ADL_MAX_PATH)]
        internal string DisplayName;
        /// <summary> Adapter Present status</summary>
        internal int Present;
        /// <summary> Adapter Exist status</summary>
        internal int Exist;
        /// <summary> Adapter Driver Path</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ADL.ADL_MAX_PATH)]
        internal string DriverPath;
        /// <summary> Adapter Driver Ext Path</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ADL.ADL_MAX_PATH)]
        internal string DriverPathExt;
        /// <summary> Adapter PNP String</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ADL.ADL_MAX_PATH)]
        internal string PNPString;
        /// <summary> OS Display Index</summary>
        internal int OSDisplayIndex;
    }

    /// <summary> ADLAdapterInfo Array</summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ADLAdapterInfoArray
    {
        /// <summary> ADLAdapterInfo Array </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ADL.ADL_MAX_ADAPTERS)]
        internal ADLAdapterInfo[] ADLAdapterInfo;
    }
    /// <summary>
    /// Overdrive Fan control info
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ADLODNFanControl
    {
        internal int iMode;
        internal int iFanControlMode;
        internal int iCurrentFanSpeedMode;
        internal int iCurrentFanSpeed;
        internal int iTargetFanSpeed;
        internal int iTargetTemperature;
        internal int iMinPerformanceClock;
        internal int iMinFanLimit;
    }

    internal class ADL
    {
        const string Atiadlxx_FileName = "atiadlxx.dll";
        #region Internal Constant
        /// <summary> Define the maximum path</summary>
        internal const int ADL_MAX_PATH = 256;
        /// <summary> Define the maximum adapters</summary>
        internal const int ADL_MAX_ADAPTERS = 40 /* 150 */;
        /// <summary> Define the successful</summary>
        internal const int ADL_SUCCESS = 0;
        #endregion Internal Constant

        [DllImport(Atiadlxx_FileName, CallingConvention=CallingConvention.Cdecl)]
        internal static extern int ADL_Main_Control_Create(ADL_Main_Memory_Alloc callback, int enumConnectedAdapters);

        [DllImport(Atiadlxx_FileName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ADL_Main_Control_Destroy();

        [DllImport(Atiadlxx_FileName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ADL_Adapter_NumberOfAdapters_Get(ref int numAdapters);

        [DllImport(Atiadlxx_FileName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ADL_Adapter_AdapterInfo_Get(IntPtr info, int inputSize); 

        [DllImport(Atiadlxx_FileName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ADL_Adapter_ID_Get(int adapterIndex, ref int adapterId);

        [DllImport(Atiadlxx_FileName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ADL2_OverdriveN_FanControl_Get(IntPtr context, int adapterIndex, ref ADLODNFanControl fanControl);

        [DllImport(Atiadlxx_FileName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ADL2_OverdriveN_Temperature_Get(IntPtr context, int adapterIndex, int iTemperatureType, ref int iTemperature);

        public static IntPtr ADL_Main_Memory_Alloc(int size)
        {
            return Marshal.AllocCoTaskMem(size);
        }
    }
}
