using System.Windows.Media;
using WpfUtils;

namespace chancies.Whiteboard.ViewModels
{
    public class ColourViewModel
        : BaseViewModel
    {
        public ColourViewModel()
            : this(Colors.Black)
        {
        }

        public ColourViewModel(Color colour)
        {
            Colour = colour;
            Brush = new SolidColorBrush(colour);
        }

        public Color Colour { get; set; }
        public Brush Brush { get; set; }
    }
}
