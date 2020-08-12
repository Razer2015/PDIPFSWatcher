using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PDIPFSWatcher.Models;

namespace PDIPFSWatcher.Tracing
{
    public class TraceManager : IDisposable
    {
        TraceEventSession _kernelSession;
        KernelTraceEventParser _kernelParser;

        TraceEventSession _customSession;
        //ClrTraceEventParser _clrParser;

        Thread _processingThread;

        public event Action<TraceEvent, EventType> EventTrace;

        public TraceManager()
        {
            TraceEventSession.SetDebugPrivilege();

            _handlers = new Dictionary<EventType, Action<TraceEvent>>
            {
                {EventType.ProcessStart, obj => HandleEvent(obj, EventType.ProcessStart)},
            };
        }

        public void Dispose()
        {
            _kernelSession.Dispose();
        }

        public void Start(IEnumerable<EventType> types)
        {
            if (EventTrace == null)
                throw new InvalidOperationException("Must register for event notifications");

            _kernelSession =
                new TraceEventSession(KernelTraceEventParser.KernelSessionName,
                    TraceEventSessionOptions.NoRestartOnCreate)
                {
                    BufferSizeMB = 128,
                    //CpuSampleIntervalMSec = 10,
                };
            var keywords = KernelTraceEventParser.Keywords.All;

            _processingThread = new Thread(() =>
            {
                _kernelSession.EnableKernelProvider(keywords);
                _kernelParser = new KernelTraceEventParser(_kernelSession.Source);
                SetupCallbacks(types);
                _kernelSession.Source.Process();
            });
            _processingThread.Priority = ThreadPriority.Lowest;
            _processingThread.IsBackground = true;
            _processingThread.Start();
        }

        private void Parser_All(TraceEvent obj)
        {
            HandleEvent(obj, EventType.Custom);
        }

        public TraceEventFilter Filter { get; set; }

        public void Stop()
        {
            _kernelSession.Flush();
            _kernelSession.Stop();
            _customSession?.Stop();
        }

        void HandleEvent(TraceEvent evt, EventType type)
        {
            var include = Filter?.EvaluateEvent(evt);
            if (include == null || include == FilterRuleResult.Include)
            {
                EventTrace(evt.Clone(), type);
            }
        }

        Dictionary<EventType, Action<TraceEvent>> _handlers;

        void SetupCallback(EventType type)
        {
            switch (type)
            {
                case EventType.FileRead:
                    _kernelParser.FileIORead += obj => HandleEvent(obj, EventType.FileRead);
                    break;

                case EventType.FileWrite:
                    _kernelParser.FileIOWrite += obj => HandleEvent(obj, EventType.FileWrite);
                    break;

                case EventType.FileRename:
                    _kernelParser.FileIORename += obj => HandleEvent(obj, EventType.FileRename);
                    break;

                case EventType.FileCreate:
                    _kernelParser.FileIOCreate += obj => HandleEvent(obj, EventType.FileCreate);
                    break;

                case EventType.FileClose:
                    _kernelParser.FileIOClose += obj => HandleEvent(obj, EventType.FileClose);
                    break;

                case EventType.FileFlush:
                    _kernelParser.FileIOFlush += obj => HandleEvent(obj, EventType.FileFlush);
                    break;

                case EventType.FileDelete:
                    _kernelParser.FileIOFileDelete += obj => HandleEvent(obj, EventType.FileDelete);
                    break;

                case EventType.DiskRead:
                    _kernelParser.DiskIORead += obj => HandleEvent(obj, EventType.DiskRead);
                    break;

                case EventType.DiskWrite:
                    _kernelParser.DiskIOWrite += obj => HandleEvent(obj, EventType.DiskWrite);
                    break;

                case EventType.FileMapDCStart:
                    _kernelParser.FileIOMapFileDCStart += obj => HandleEvent(obj, EventType.FileMapDCStart);
                    break;

                case EventType.FileMapDCStop:
                    _kernelParser.FileIOMapFileDCStop += obj => HandleEvent(obj, EventType.FileMapDCStop);
                    break;

                case EventType.FileMap:
                    _kernelParser.FileIOMapFile += obj => HandleEvent(obj, EventType.FileMap);
                    break;

                case EventType.FileUnmap:
                    _kernelParser.FileIOUnmapFile += obj => HandleEvent(obj, EventType.FileUnmap);
                    break;
            }
        }

        private void SetupCallbacks(IEnumerable<EventType> types, bool add = true)
        {
            foreach (var type in types)
            {
                SetupCallback(type);
            }
        }
    }
}
