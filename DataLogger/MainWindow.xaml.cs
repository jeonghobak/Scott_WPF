using DataLogger.ViewModel;
using ScottPlot;
using ScottPlot.AxisPanels;
using ScottPlot.Plottables;
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
using System.Windows.Threading;

namespace DataLogger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainChartVM m_ViewModel;

        // timer that triggers plot updates
        readonly DispatcherTimer UpdatePlotTimer = new() { Interval = TimeSpan.FromMilliseconds(50), IsEnabled = true };

        public MainWindow()
        {
            InitializeComponent();
            m_ViewModel = Resources["vm"] as MainChartVM;
            ChartInit();
        }

        private void ChartInit()
        {
            var logger1 = PlotControl.Plot.Add.Plottable(m_ViewModel.m_Logger1);
            var logger2 = PlotControl.Plot.Add.Plottable(m_ViewModel.m_Logger2);

            RightAxis axis1 = (RightAxis)PlotControl.Plot.Axes.Right;
            m_ViewModel.m_Logger1.Axes.YAxis = axis1;
            m_ViewModel.m_Logger2.Color = ScottPlot.Colors.Crimson;
            axis1.Color(ScottPlot.Colors.Crimson);

            RightAxis axis2 = (RightAxis)PlotControl.Plot.Axes.AddRightAxis();
            m_ViewModel.m_Logger2.Axes.YAxis = axis2;
            axis2.Color(ScottPlot.Colors.Blue);                       
             
            UpdatePlotTimer.Tick += (s, e) =>
            {   
                if(m_ViewModel.m_Logger1.HasNewData || m_ViewModel.m_Logger2.HasNewData)
                {
                    PlotControl.Refresh();
                }
            };
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}