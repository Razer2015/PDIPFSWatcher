namespace GT.TOC.Core.Volume
{
    public interface IVolumeSegment
    {
        uint Size { get; set; }
        uint RealSize { get; set; }

        uint DataSize { get; set; }
        byte[] Data { get; set; }

        bool Read(EndianBinReader reader, uint size, uint realSize);
    }
}
