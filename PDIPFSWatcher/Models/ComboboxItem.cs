using System.Linq;

namespace PDIPFSWatcher.Models
{
    public class ComboboxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return $"{(Text.Length > 20 ? $"{Text.Left(17)}..." : Text),-20} | {Value}";
        }
    }
}
