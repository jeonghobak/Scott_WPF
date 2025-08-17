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
        private WpfPlot m_WpfPlot;

        private Signal m_Signal;
        public  Signal Signal
        {
            get { return m_Signal; }
            set
            {
                if (m_Signal != value)
                {
                    m_Signal = value;
                    OnPropertyChanged(nameof(Signal));
                }
            }
        }

        private string m_StatusText;
        public  string StatusText
        {
            get { return m_StatusText; }
            set
            {
                if (m_StatusText != value)
                {
                    m_StatusText = value;
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }

        public ICommand GenerateCommand { get; private set; }

        public MainChartVM(WpfPlot wpfPlot)
        {
            m_WpfPlot = wpfPlot;
            GenerateCommand = new RelayCommand(GenerateSample);
        }

        private void GenerateSample()
        {
            Signal = m_WpfPlot.Plot.Add.Signal(Generate.RandomWalk(1_000_000));
            m_WpfPlot.Plot.Axes.AutoScale();
            m_WpfPlot.Refresh();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
