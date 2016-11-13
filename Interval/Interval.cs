using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Intervals
{
    public class Interval : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Start
        {
            get { return start; }
            set
            {
                start = value;
                OnPropertyChanged("Start");
                if (end < start)
                    End = Start;
            }
        }
        private int start;

        public int End
        {
            get { return end; }
            set
            {
                end = value;
                OnPropertyChanged("End");
                if (start > end)
                    start = end;
            }
        }
        private int end;

        public Interval[] Paterns { get; private set; }

        public Interval()
        {
            Paterns = new Interval[2];
        }

        public Interval(int start, int end)
            : this()
        {
            this.start = start;
            this.end = end;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged is PropertyChangedEventHandler)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{Start} - {End}";
        }
    }

}