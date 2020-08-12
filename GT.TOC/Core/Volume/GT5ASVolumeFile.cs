using GT.Shared.Logging;

namespace GT.TOC.Core.Volume
{
    public class GT5ASVolumeFile : GT5VolumeFile
    {
        public GT5ASVolumeFile(string basePath, ILogWriter logger = null) : base(basePath, logger) { }

        public override (string Seed, uint[] Key) KeySet => ("TAKLAMAKAN-63706075",
            new uint[] { 0x93783D89, 0x33D56FB5, 0xE7701B43, 0x6032AD93 });
    }
}
