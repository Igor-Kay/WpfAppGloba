using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class PickupPoint : INotifyPropertyChanged
    {
        private int _pointId;
        private string _location;
        private string _workingHours;
        private bool _isSelected;

        public int PointId
        {
            get { return _pointId; }
            set
            {
                if (_pointId != value)
                {
                    _pointId = value;
                    OnPropertyChanged(nameof(PointId));
                }
            }
        }

        public string Location
        {
            get { return _location; }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }

        public string WorkingHours
        {
            get { return _workingHours; }
            set
            {
                if (_workingHours != value)
                {
                    _workingHours = value;
                    OnPropertyChanged(nameof(WorkingHours));
                }
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
