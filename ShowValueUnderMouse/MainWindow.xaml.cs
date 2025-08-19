using ScottPlot;
using ScottPlot.Plottables;
using ShowValueUnderMouse.ViewModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShowValueUnderMouse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainChartVM?    m_ViewModel;
        private Signal?         m_Signal;
        private Crosshair       m_CrossHair;

        public MainWindow()
        {
            InitializeComponent();
            
            m_ViewModel = Resources["vm"] as MainChartVM;

            /// Subscription 
            m_ViewModel.PropertyChanged += (_, e) =>
            {
                switch(e.PropertyName)
                {
                    case nameof(MainChartVM.ChartData):
                        m_Signal = PlotControl.Plot.Add.Signal(m_ViewModel.ChartData);                        
                        break;
                }

                PlotControl.Plot.Axes.AutoScale();
                PlotControl.Refresh();
            };

            InitPlotControl();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {            
            Application.Current.Shutdown();
        }

        private void InitPlotControl()
        {
            Debug.Assert(m_ViewModel != null);

            m_CrossHair = PlotControl.Plot.Add.Crosshair(0, 0);
            m_CrossHair.MarkerShape = MarkerShape.OpenCircle;
            m_CrossHair.MarkerSize = 15;
            m_CrossHair.IsVisible = false;

            PlotControl.MouseMove += PlotControl_MouseMove;
        }

        private void PlotControl_MouseMove(object sender, MouseEventArgs e)
        {
            if(m_Signal != null)
            {
                var mousePos = e.GetPosition(PlotControl);
                Pixel mousePixel = new Pixel()
                {
                    X = (int)(mousePos.X * PlotControl.DisplayScale),
                    Y = (int)(mousePos.Y * PlotControl.DisplayScale)
                };

                Coordinates mouseLocation = PlotControl.Plot.GetCoordinates(mousePixel);

                DataPoint nearest = m_Signal.GetNearest(mouseLocation, PlotControl.Plot.LastRender);
                if (nearest.IsReal == true)
                {
                    m_CrossHair.IsVisible = true;
                    m_CrossHair.Position = nearest.Coordinates;
                    m_ViewModel.StatusText = $"X:{nearest.Coordinates.X:F2}, Y: {nearest.Coordinates.Y:F2}";
                    PlotControl.Refresh();
                }
            }
        }


    }
}