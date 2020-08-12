using GT.Shared.Logging;

namespace GT.TOC.Core.Volume
{
    public class GT6VolumeFile : GT5VolumeFile
    {
        public GT6VolumeFile(string basePath, ILogWriter logger = null) : base(basePath, logger) { }

        public override (string Seed, uint[] Key) KeySet => ("PISCINAS-323419048",
            new uint[] {0xAA1B6A59, 0xE70B6FB3, 0x62DC6095, 0x6A594A25});
    }
}
