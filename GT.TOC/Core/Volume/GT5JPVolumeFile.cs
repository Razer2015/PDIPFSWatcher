using GT.Shared.Logging;

namespace GT.TOC.Core.Volume
{
    public class GT5JPVolumeFile : GT5VolumeFile
    {
        public GT5JPVolumeFile(string basePath, ILogWriter logger = null) : base(basePath, logger) { }

        public override (string Seed, uint[] Key) KeySet => ("SAHARA-568201135",
            new uint[] { 0xD770A27B, 0x2114AABD, 0xDD8C423D, 0x54690651 });
    }
}
