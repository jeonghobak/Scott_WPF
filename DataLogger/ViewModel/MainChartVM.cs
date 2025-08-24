using DataLogger.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DataLogger.ViewModel
{
    public class MainChartVM : INotifyPropertyChanged
    {
        public ScottPlot.Plottables.DataLogger m_Logger1 { get; private set; }
        public ScottPlot.Plottables.DataLogger m_Logger2 { get; private set; }

        private ScottPlot.DataGenerators.RandomWalker walker1 = new(0, multiplier: 0.01);
        private ScottPlot.DataGenerators.RandomWalker walker2 = new(0, multiplier: 1000);

        readonly DispatcherTimer AddNewDataTimer = new()
        {
            Interval = TimeSpan.FromMilliseconds(10),
            IsEnabled = true
        };

        public ICommand ViewFullCommand { get; private set; }
        public ICommand ViewJumpCommand { get; private set; }
        public ICommand ViewSlideCommand { get; private set; }

        public MainChartVM()
        {
            m_Logger1 = new ScottPlot.Plottables.DataLogger();
            m_Logger2 = new ScottPlot.Plottables.DataLogger();

            ViewFullCommand = new RelayCommand(ViewFull);
            ViewJumpCommand = new RelayCommand(ViewJump);
            ViewSlideCommand = new RelayCommand(ViewSlide);

            AddNewDataTimer.Tick += (s, e) =>
            {
                var data1 = walker1.Next();
                var data2 = walker2.Next();

                m_Logger1.Add(data1);
                m_Logger2.Add(data2);
            };
        }

        //Define Commands

        private void ViewFull()
        {
            m_Logger1.ViewFull();
            m_Logger2.ViewFull();
        }

        private void ViewJump()
        {
            m_Logger1.ViewJump();
            m_Logger2.ViewJump();
        }

        private void ViewSlide()
        {
            m_Logger1.ViewSlide();
            m_Logger2.ViewSlide();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
