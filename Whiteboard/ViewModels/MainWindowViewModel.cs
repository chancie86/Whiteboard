using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Media;
using WpfUtils;
using WpfUtils.ViewModels;

namespace chancies.Whiteboard.ViewModels
{
    public partial class MainWindowViewModel
        : WindowBaseViewModel
    {
        #region Events
        public event InkColourChangedHandler InkColourChanged;
        public event InputModeChangedHandler InputModeChanged;
        #endregion

        private readonly ObservableCollection<ClientViewModel> _clients;
        private readonly ObservableCollection<ColourViewModel> _availableInkColours;
        private ColourViewModel _selectedInkColour;
        private bool _isInkModeSelected;
        private bool _isErasePointModeSelected;
        private bool _isEraseStrokeModeSelected;

        private SynchronizationContext _uiSynchronizationContext;
        
        public MainWindowViewModel(ConnectViewModel connection)
        {
            Title = "Whiteboard";

            Connection = connection;

            _uiSynchronizationContext = SynchronizationContext.Current;

            IsInkModeSelected = true;

            _clients = new ObservableCollection<ClientViewModel>();
            _clients.Add(ClientViewModel.Current);

            _availableInkColours = new ObservableCollection<ColourViewModel>(
                new []
                {
                    new ColourViewModel(Colors.Black),
                    new ColourViewModel(Colors.White),
                    new ColourViewModel(Colors.Red),
                    new ColourViewModel(Colors.Orange),
                    new ColourViewModel(Colors.Green),
                    new ColourViewModel(Colors.Blue),
                    new ColourViewModel(Colors.Yellow),
                    new ColourViewModel(Colors.Purple)
                });

            RegisterOnPropertyChangedHandler(() => SelectedInkColour, RaiseInkColourChanged);
            RegisterOnPropertyChangedHandler(() => IsInkModeSelected, RaiseInputModeChanged);
            RegisterOnPropertyChangedHandler(() => IsErasePointModeSelected, RaiseInputModeChanged);
            RegisterOnPropertyChangedHandler(() => IsEraseStrokeModeSelected, RaiseInputModeChanged);

            SendStrokesCommand = new SimpleParameterCommand(SendStrokesCommandExecute);
        }

        public ObservableCollection<ClientViewModel> Clients
        {
            get { return _clients; }
        }

        public ObservableCollection<ColourViewModel> AvailableInkColours { get { return _availableInkColours; } }

        public ColourViewModel SelectedInkColour
        {
            get { return _selectedInkColour; }
            set { OnPropertyChanged(ref _selectedInkColour, value, () => SelectedInkColour); }
        }

        public bool IsInkModeSelected
        {
            get { return _isInkModeSelected; }
            set { OnPropertyChanged(ref _isInkModeSelected, value, () => IsInkModeSelected); }
        }

        public bool IsErasePointModeSelected
        {
            get { return _isErasePointModeSelected; }
            set { OnPropertyChanged(ref _isErasePointModeSelected, value, () => IsErasePointModeSelected); }
        }

        public bool IsEraseStrokeModeSelected
        {
            get { return _isEraseStrokeModeSelected; }
            set { OnPropertyChanged(ref _isEraseStrokeModeSelected, value, () => IsEraseStrokeModeSelected); }
        }

        private ConnectViewModel Connection { get; set; }

        private InputMode GetInputMode()
        {
            if (IsInkModeSelected)
                return InputMode.Ink;
            
            if (IsErasePointModeSelected)
                return InputMode.ErasePoint;
            
            return InputMode.EraseStroke;
        }

        private void RaiseInputModeChanged()
        {
            if (InputModeChanged != null)
            {
                InputModeChanged(this, new InputModeChangedEventArgs(GetInputMode()));
            }
        }

        private void RaiseInkColourChanged()
        {
            if (InkColourChanged != null)
            {
                InkColourChanged(this, new InkColourChangedEventArgs(SelectedInkColour.Colour));
            }
        }
    }

    #region InkChanged Event
    public delegate void InkColourChangedHandler(object sender, InkColourChangedEventArgs args);

    public class InkColourChangedEventArgs
        : EventArgs
    {
        public InkColourChangedEventArgs(Color newColour)
        {
            NewColour = newColour;
        }

        public Color NewColour { get; private set; }
    }
    #endregion

    #region InputMode Event
    public delegate void InputModeChangedHandler(object sender, InputModeChangedEventArgs args);

    public class InputModeChangedEventArgs
        : EventArgs
    {
        public InputModeChangedEventArgs(InputMode newMode)
        {
            NewMode = newMode;
        }

        public InputMode NewMode { get; private set; }
    }
    #endregion
}
