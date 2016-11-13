using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Intervals;

namespace IntervalsWpf
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Interval> UserIntervalList { get; private set; }
        //public ObservableCollection<Interval> IntervalList { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            UserIntervalList = new ObservableCollection<Interval>(new[]
            {
                new Interval(1, 3) ,
                new Interval(2, 6) ,
                new Interval(8, 10) ,
                new Interval(10, 15) ,
                });
            ValidateIntervals();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (Interval)Resources["newItem"];
            if (item.Start == item.End)
                MessageBox.Show("El inicio y el final del intervalo deben ser diferentes.");
            else
            {
                UserIntervalList.Add(new Interval(item.Start, item.End));
                ValidateIntervals();
            }
        }

        private void ValidateIntervals()
        {
            Intervals.Items.Clear();
            var newIntervals = new List<Interval>();
            
            foreach (var userInterval in UserIntervalList)
                Intervals.Items.Add(userInterval);

            foreach (var userInterval in UserIntervalList)
            {
                foreach (var item in UserIntervalList.Where(interval => !Interval.Equals(interval, userInterval)))
                    if (item.Start <= userInterval.Start && item.End >= userInterval.Start)
                    {
                        var newInterval = new Interval(Math.Min(userInterval.Start, item.Start), Math.Max(userInterval.End, item.End));
                        newInterval.Paterns[0] = userInterval;
                        newInterval.Paterns[1] = item;


                        if (!Intervals.Items.Any(it => it.Start <= newInterval.Start && it.End >= newInterval.End))
                            newIntervals.Add(newInterval);
                    }

            }
            Intervals.Items.Add(new Interval());

            foreach (var item in newIntervals)
                Intervals.Items.Add(item);

        }
    }
}
