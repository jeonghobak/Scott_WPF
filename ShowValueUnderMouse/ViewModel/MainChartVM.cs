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
        private double[] m_ChartData;
        public double[] ChartData
        {
            get { return m_ChartData; }
            set
            {
                m_ChartData = value;
                OnPropertyChanged(nameof(ChartData));
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

        public MainChartVM()
        {
            GenerateCommand = new RelayCommand(GenerateSample);
        }

        private void GenerateSample()
        {            
            ChartData = ScottPlot.Generate.RandomWalk(1_000_000);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
