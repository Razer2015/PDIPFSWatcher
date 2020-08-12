using System;

namespace GT.Shared
{
    public class PDIPFSFileAccessEventArgs : EventArgs
    {
        public string PDIPFS { get; set; }
        public string HRPath { get; set; }
        public uint NodeIndex { get; set; }

        public long Offset { get; set; }
        public int Size { get; set; }
        public ulong IrpPtr { get; set; }
    }
}
