using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.WPF;
using ScottStudy2.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScottStudy2.ViewModel
{
    public class MainChartVM : INotifyPropertyChanged
    {
        private const float cMouseRadiusPx = 10;    // Radius in pixels for mouse interaction                                                    

        private AxisLine? PlottableBeingDragged = null;

        public MainChartVM()
        {
            AddSampleDataCommand = new RelayCommand(AddSampleData);

            var v1 = PlotControl.Plot.Add.VerticalLine(0, 2, color: Color.FromColor(System.Drawing.Color.Red));
            v1.IsDraggable = true;
            v1.Text = "Vertical Line";

            var v2 = PlotControl.Plot.Add.HorizontalLine(0, 2, color: Color.FromColor(System.Drawing.Color.Green));
            v2.IsDraggable = true;
            v2.Text = "Horizontal Line";

            PlotControl.MouseDown += PlotControl_MouseDown;
            PlotControl.MouseUp += PlotControl_MouseUp;
            PlotControl.MouseMove += PlotControl_MouseMove;
        }

        private void PlotControl_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(PlotControl);
            float sx = (float)(pos.X * PlotControl.DisplayScale);
            float sy = (float)(pos.Y * PlotControl.DisplayScale);
            float sr = cMouseRadiusPx * (float)PlotControl.DisplayScale;

            CoordinateRect rect = PlotControl.Plot.GetCoordinateRect(sx, sy, sr);

            if (PlottableBeingDragged is null)
            {
                AxisLine? lineUnderMouse = GetLineUnderMouse(sx, sy, sr);
                if(lineUnderMouse is null)
                {
                    PlotControl.Cursor = Cursors.Arrow; // no line under mouse, reset cursor
                }
                else if(lineUnderMouse.IsDraggable && lineUnderMouse is VerticalLine)
                {
                    PlotControl.Cursor = Cursors.SizeWE; // vertical line under mouse, set cursor to horizontal resize
                }
                else if(lineUnderMouse.IsDraggable && lineUnderMouse is HorizontalLine)
                {
                    PlotControl.Cursor = Cursors.SizeNS; // horizontal line under mouse, set cursor to vertical resize
                }
            }
            else
            {
                if(PlottableBeingDragged is HorizontalLine hLine)
                {
                    hLine.Y = rect.VerticalCenter;
                    hLine.Text = $"Y: {hLine.Y:F2}";
                }
                else if(PlottableBeingDragged is VerticalLine vLine)
                {
                    vLine.X = rect.HorizontalCenter;
                    vLine.Text = $"X: {vLine.X:F2}";
                }
            }

            PlotControl.Refresh();
        }

        private void PlotControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PlottableBeingDragged = null;
            PlotControl.UserInputProcessor.Enable(); // re-enable panning after dragging a line
            PlotControl.Refresh();
        }

        private void PlotControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(PlotControl);

            float sx = (float)(pos.X * PlotControl.DisplayScale);
            float sy = (float)(pos.Y * PlotControl.DisplayScale);
            float sr = cMouseRadiusPx * (float)PlotControl.DisplayScale;

            AxisLine? lineUnderMouse = GetLineUnderMouse(sx, sy, sr);

            if(lineUnderMouse is not null)
            {
                PlottableBeingDragged = lineUnderMouse;
                PlotControl.UserInputProcessor.Disable(); // disable panning while dragging a line
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public WpfPlot PlotControl { get; } = new WpfPlot();
        public ICommand AddSampleDataCommand { get; private set; }

        private void AddSampleData()
        {
            var sig1 = PlotControl.Plot.Add.Signal(Generate.Sin());
            sig1.Color = Color.FromColor(System.Drawing.Color.Blue);
            sig1.LineWidth = 2;
            sig1.LegendText = "Sine Wave";

            var sig2 = PlotControl.Plot.Add.Signal(Generate.Cos());
            sig2.Color = Color.FromColor(System.Drawing.Color.Orange);
            sig2.LineWidth = 2;
            sig2.LegendText = "Cosine Wave";

            PlotControl.Plot.Axes.AutoScale();
            PlotControl.Plot.ShowLegend();

            PlotControl.Refresh();
        }       

        private AxisLine? GetLineUnderMouse(float x, float y, float radiousPx)
        {
            CoordinateRect rect = PlotControl.Plot.GetCoordinateRect(x, y, radiousPx);
           
            foreach(AxisLine axLine in PlotControl.Plot.GetPlottables<AxisLine>().Reverse())
            {
                if(axLine.IsUnderMouse(rect))
                {
                    return axLine;
                }
            }

            return null;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
