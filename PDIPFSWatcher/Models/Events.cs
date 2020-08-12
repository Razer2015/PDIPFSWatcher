using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIPFSWatcher.Models
{
    public enum EventType
    {
        None,
        ProcessStart = 100, ProcessStop, ProcessDCStart, ProcessDCStop,
        ThreadStart = 200, ThreadStop, ThreadDCStart, ThreadDCStop,
        MemoryAlloc = 300, MemoryFree, HeapRangeCreate, HeapRangeDestory, HeapRangeReserve, VirtualAllocDCStart,
        VirtualAllocDCStop,
        RegistryOpenKey = 400, RegistryQueryValue, RegistrySetValue, RegistryCreateKey,
        RegistryCloseKey, RegistryEnumerateKey, RegistryEnumerateValues, RegistryFlush,
        RegistryDeleteKey, RegistryDeleteValue, RegistryQueryMultipleValues,
        AlpcSendMessage = 500, AlpcReceiveMessage, AlpcWaitForReply, ALPCWaitForNewMessage,
        ModuleLoad = 600, ModuleUnload, ModuleDCLoad, ModuleDCUnload,
        FileRead = 700, FileWrite, FileCreate, FileRename, FileDelete, FileQueryInfo, FileClose, FileFlush,
        FileMapDCStart, FileMapDCStop, FileMap, FileUnmap,
        DiskRead = 800, DiskWrite, DriverMajorFunctionCall, DriverMajorFunctionReturn, DriverCompletionRoutine,
        TcpIpReceive = 900, TcpIpSend, TcpIpConnect, TcpIpDisconnect, TcpIpAccept,
        MemoryMemInfo = 950, MemoryInMemory, MemorySystemMemoryInfo, MemoryInMemoryActive, ProcessMemoryInfo,
        Custom = 0x10000000
    }
}
