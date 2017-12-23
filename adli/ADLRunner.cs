using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace adli
{
    class ADLRunner : IDisposable
    {
        private List<GPU> _gpuList;
        private IntPtr _adapterBuffer;

        public ADLRunner()
        {
            var adlRet = -1;
            var numberOfAdapters = 0;

            //The second parameter is iEnumConnectedAdapters.
            //If its value is zero, the adapter information will be collected only for the adapters that are physically present and enabled in the system.
            //If the value is non-zero, information of all adapters that have ever been present (even currently away) in the system will be collected.
            if ((adlRet = ADL.ADL_Main_Control_Create(ADL.ADL_Main_Memory_Alloc, 0)) == ADL.ADL_SUCCESS)
            {
                ADL.ADL_Adapter_NumberOfAdapters_Get(ref numberOfAdapters);
                var adapterInfoArray = new ADLAdapterInfoArray();

                var size = Marshal.SizeOf(adapterInfoArray);
                _adapterBuffer = Marshal.AllocCoTaskMem(size);
                Marshal.StructureToPtr(adapterInfoArray, _adapterBuffer, false);
                if ((adlRet = ADL.ADL_Adapter_AdapterInfo_Get(_adapterBuffer, size)) == ADL.ADL_SUCCESS)
                {
                    adapterInfoArray = (ADLAdapterInfoArray)Marshal.PtrToStructure(_adapterBuffer, typeof(ADLAdapterInfoArray));
                    _gpuList = adapterInfoArray.ADLAdapterInfo.Take(numberOfAdapters).Select(adapter =>
                    {
                        var adapterId = 0;
                        ADL.ADL_Adapter_ID_Get(adapter.AdapterIndex, ref adapterId);
                        return new GPU
                        {
                            Id = adapterId,
                            Name = adapter.AdapterName,
                            Bus = adapter.BusNumber,
                            AdapterIndex = adapter.AdapterIndex
                        };
                    }).Where(adapter => adapter.Bus > 0)
                    .GroupBy(adapter => adapter.Id).Select(group => group.First()).ToList();
                }
            }
        }

        public void Run()
        {
            if (_gpuList != null)
            {
                var adlRet = -1;
                foreach (var gpu in _gpuList)
                {
                    var fanControl = new ADLODNFanControl();
                    var temp = 0;
                    if ((adlRet = ADL.ADL2_OverdriveN_FanControl_Get(IntPtr.Zero, gpu.AdapterIndex, ref fanControl)) == ADL.ADL_SUCCESS)
                    {
                        gpu.Fan = fanControl;
                    }
                    if ((adlRet = ADL.ADL2_OverdriveN_Temperature_Get(IntPtr.Zero, gpu.AdapterIndex, 1, ref temp)) == ADL.ADL_SUCCESS)
                    {
                        gpu.Temperature = temp;
                    }
                }
            }
        }

        public List<GPU> GetGPUList()
        {
            return _gpuList;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Marshal.FreeCoTaskMem(_adapterBuffer);
                }

                ADL.ADL_Main_Control_Destroy();

                disposedValue = true;
            }
        }

        //override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
         ~ADLRunner()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
