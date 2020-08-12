using GT.Shared.Logging;

namespace GT.TOC.Core.Volume
{
    public class GT5USVolumeFile : GT5VolumeFile
    {
        public GT5USVolumeFile(string basePath, ILogWriter logger = null) : base(basePath, logger) { }

        public override (string Seed, uint[] Key) KeySet => ("PATAGONIAN-22798263",
            new uint[] { 0x5A1A59E5, 0x4D3546AB, 0xF30AF68B, 0x89F08D0D });
    }
}
