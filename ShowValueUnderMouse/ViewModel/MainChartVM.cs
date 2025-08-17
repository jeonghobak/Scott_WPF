using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.WPF;
using ShowValueUnderMouse.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShowValueUnderMouse.ViewModel
{
    public class MainChartVM : INotifyPropertyChanged
    {   
        public MainChartVM()
        {
            PlotControl = new WpfPlot();
            GenerateCommand = new RelayCommand(GenerateSample);
            PlotControl.MouseMove += PlotControl_MouseMove;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Signal Signal { get; set; }        
        public WpfPlot PlotControl { get; }
        public Crosshair PlotCrosshair { get; set; }

        private string statusText;
        public string StatusText
        {
            get { return statusText; }
            set
            {
                if (statusText != value)
                {
                    statusText = value;
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }

        public ICommand GenerateCommand { get; private set; }

        private void PlotControl_MouseMove(object sender, MouseEventArgs e)
        {
            if(null == Signal || null == PlotControl)
            {
                return;
            }

            var mousePos = e.GetPosition(PlotControl);

            Pixel mousePixel = new Pixel()
            {
                X = (int)(mousePos.X * PlotControl.DisplayScale),
                Y = (int)(mousePos.Y * PlotControl.DisplayScale)
            };
            
            Coordinates mouseLocation = PlotControl.Plot.GetCoordinates(mousePixel);

            DataPoint nearest = Signal.GetNearest(mouseLocation, PlotControl.Plot.LastRender);

            if(nearest.IsReal == true)
            {
                PlotCrosshair.IsVisible = true;
                PlotCrosshair.Position = nearest.Coordinates;

                StatusText = $"X:{nearest.Coordinates.X:F2}, Y: {nearest.Coordinates.Y:F2}";
                
                PlotControl.Refresh();
            }
        }

        private void GenerateSample()
        {
            Signal = PlotControl.Plot.Add.Signal(Generate.RandomWalk(1_000_000));

            PlotCrosshair = PlotControl.Plot.Add.Crosshair(0, 0);
            PlotCrosshair.IsVisible = false;
            PlotCrosshair.MarkerShape = MarkerShape.OpenCircle;
            PlotCrosshair.MarkerSize = 15;

            PlotControl.Plot.Axes.AutoScale();
            PlotControl.Refresh();
        }



        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
