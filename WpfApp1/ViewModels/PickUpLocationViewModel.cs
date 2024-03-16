using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    public class PickupPointsViewModel : ViewModelBase
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<PickupPoint> _pickupPoints;

        public ObservableCollection<PickupPoint> PickupPoints
        {
            get { return _pickupPoints; }
            set
            {
                _pickupPoints = value;
                OnPropertyChanged(nameof(PickupPoints));
            }
        }

        public PickupPointsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadPickupPoints();
        }

        private void LoadPickupPoints()
        {
            PickupPoints = new ObservableCollection<PickupPoint>(_databaseService.GetPickupPoints());
        }
    }

}
