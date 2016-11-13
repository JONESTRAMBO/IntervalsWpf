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
    /// Lógica de interacción para IntervalsView.xaml
    /// </summary>
    public partial class IntervalsView : UserControl
    {
        public static List<SolidColorBrush> BrushList = new List<SolidColorBrush>(new[] {
            new SolidColorBrush(Color.FromArgb(0x7f, 0xf1, 0x5f, 0x74)),
            new SolidColorBrush(Color.FromArgb(0x7f, 0x2c, 0xa8, 0xc2)),
            new SolidColorBrush(Color.FromArgb(0x7f, 0x98, 0xcb, 0x4a)),
            new SolidColorBrush(Color.FromArgb(0x7f, 0x91, 0x3c, 0xcd)),
            new SolidColorBrush(Color.FromArgb(0x7f, 0xf7, 0x6d, 0x3c)),
            new SolidColorBrush(Color.FromArgb(0x7f, 0x54, 0x81, 0xe6)),
            new SolidColorBrush(Color.FromArgb(0x7f, 0xf7, 0xd8, 0x42)),
            new SolidColorBrush(Color.FromArgb(0x7f, 0x83, 0x90, 0x98)),
            new SolidColorBrush(Color.FromArgb(0x7f, 0x83, 0x90, 0x98)),
            new SolidColorBrush(Color.FromArgb(0x7f, 0x83, 0x90, 0x98)),
        });

        public ObservableCollection<Intervals.Interval> Items { get; set; }
        //private List<Rectangle> Lines { get; set; }

        public int Start
        {
            get { return (int)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Start.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartProperty =
            DependencyProperty.Register("Start", typeof(int), typeof(IntervalsView), new PropertyMetadata(0));

        public double Middle
        {
            get { return (double)GetValue(MiddleProperty); }
            set { SetValue(MiddleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Middle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MiddleProperty =
            DependencyProperty.Register("Middle", typeof(double), typeof(IntervalsView), new PropertyMetadata(0d));

        public int End
        {
            get { return (int)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }

        // Using a DependencyProperty as the backing store for End.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndProperty =
            DependencyProperty.Register("End", typeof(int), typeof(IntervalsView), new PropertyMetadata(0));


        public IntervalsView()
        {
            InitializeComponent();
            Items = new ObservableCollection<Interval>();
            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (Intervals.Interval item in e.NewItems)
                    {
                        var line = new Line()
                        {
                            Tag = item,
                            StrokeThickness = 2,
                            Stroke = BrushList[canvas.Children.Count % 10]
                        };

                        canvas.Children.Add(line);
                        Canvas.SetBottom(line, 2 * canvas.Children.Count);
                        StringBuilder toolTip = new StringBuilder($"Intervalo: {item.Start} - {item.End}");
                        if (item.Paterns[0] is Interval && item.Paterns[1] is Interval)
                            toolTip.Append($": resultante de [{item.Paterns[0]}] y [{item.Paterns[1]}]");
                        line.ToolTip = toolTip.ToString();
                        SetPosition(line);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    canvas.Children.Clear();
                    break;
                default:
                    break;
            }
        }

        private void SetPosition(Line line)
        {
            if (line.Tag is Interval)
            {
                var item = (Interval)line.Tag;

                if (Start > item.Start || End < item.End)
                {
                    Start = canvas.Children.OfType<Line>().Select(li => (Intervals.Interval)li.Tag).Min(inter => inter.Start);
                    End = canvas.Children.OfType<Line>().Select(li => (Intervals.Interval)li.Tag).Max(inter => inter.End);
                    Middle = (End - Start) / 2f;
                    foreach (var lineItem in canvas.Children.OfType<Line>())
                        SetPosition(lineItem);
                }
                else
                {
                    int delta = End - Start;


                    double start = Math.Min(item.Start, item.End);

                    double end = Math.Max(item.Start, item.End); //End - item.End;

                    line.X1 = (start - Start) / delta * canvas.Width;
                    line.X2 = canvas.Width - (End - end) / delta * canvas.Width;
                }
            }
        }
    }
}