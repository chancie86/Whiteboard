using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using chancies.Whiteboard.ViewModels;
using WpfUtils;

namespace chancies.Whiteboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [DataTemplated(typeof(MainWindowViewModel))]
    public partial class MainWindowView
    {
        public MainWindowView()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        private MainWindowViewModel ViewModel { get; set; }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs ea)
        {
            var view = sender as MainWindowView;
            if (view == null)
                return;

            var viewModel = ea.OldValue as MainWindowViewModel;
            if (viewModel != null)
            {
                viewModel.InkColourChanged -= OnInkColourChanged;
                viewModel.InputModeChanged -= OnInputModeChanged;
                viewModel.StrokesUpdated -= OnStrokesUpdated;
            }

            viewModel = ea.NewValue as MainWindowViewModel;
            if (viewModel != null)
            {
                viewModel.InkColourChanged += OnInkColourChanged;
                viewModel.InputModeChanged += OnInputModeChanged;
                viewModel.StrokesUpdated += OnStrokesUpdated;
            }

            ViewModel = viewModel;
        }

        private void OnStrokesUpdated(object sender, StrokesUpdatedEventArgs args)
        {
            InkCanv.Strokes = args.Strokes;
        }

        private void OnInputModeChanged(object sender, InputModeChangedEventArgs args)
        {
            switch (args.NewMode)
            {
                case InputMode.Ink:
                    InkCanv.EditingMode = InkCanvasEditingMode.Ink;
                    break;
                case InputMode.ErasePoint:
                    InkCanv.EditingMode = InkCanvasEditingMode.EraseByPoint;
                    break;
                case InputMode.EraseStroke:
                    InkCanv.EditingMode = InkCanvasEditingMode.EraseByStroke;
                    break;
                case InputMode.Select:
                    InkCanv.EditingMode = InkCanvasEditingMode.Select;
                    break;
            }
        }

        private void OnInkColourChanged(object sender, InkColourChangedEventArgs args)
        {
            InkCanv.DefaultDrawingAttributes.Color = args.NewColour;
        }

        private void InkCanvStrokeErased(object sender, RoutedEventArgs e)
        {
            SyncStrokes();
        }

        private void InkCanvStrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            SyncStrokes();
        }

        private void InkCanvStrokeErasing(object sender, InkCanvasStrokeErasingEventArgs e)
        {
            SyncStrokes();
        }

        private void SyncStrokes()
        {
            try
            {
                var memoryStream = new MemoryStream();
                InkCanv.Strokes.Save(memoryStream);
                memoryStream.Flush();

                ViewModel.SendStrokesCommand.Execute(memoryStream);
            }
            catch (Exception ex)
            {
                Utilities.ShowModal(new ErrorViewModel(ex.Message));
            }
        }
    }
}
