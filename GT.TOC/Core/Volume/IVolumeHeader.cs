namespace GT.TOC.Core.Volume
{
    public interface IVolumeHeader
    {
        uint Seed { get; set; }
        uint Size { get; set; }
        uint RealSize { get; set; }

        bool Read(EndianBinReader reader);
    }
}
