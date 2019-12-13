using WpfUtils.ViewModels;

namespace chancies.Whiteboard.ViewModels
{
    public class ErrorViewModel
        : WindowBaseViewModel
    {
        public ErrorViewModel(string errorMessage)
        {
            Title = "Whiteboard";
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; private set; }
    }
}
