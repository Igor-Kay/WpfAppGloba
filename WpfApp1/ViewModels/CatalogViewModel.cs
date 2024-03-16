using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Model;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    public class CatalogViewModel : ViewModelBase
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<Product> _products;

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public ICommand SaveChangesCommand { get; }
        public ICommand DeleteCommand { get; }

        public CatalogViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadProducts();
            SaveChangesCommand = new RelayCommand(SaveChanges);
            DeleteCommand = new RelayCommand(DeleteSelectedProducts);
        }

        private void LoadProducts()
        {
            Products = new ObservableCollection<Product>(_databaseService.GetProducts());
        }

        private void SaveChanges()
        {
            _databaseService.SaveProducts(Products);
        }

        private void DeleteSelectedProducts()
        {
            var selectedProducts = Products.ToList().Where(p => p.IsSelected).ToList();
            _databaseService.DeleteProducts(selectedProducts);

            foreach (var product in selectedProducts)
            {
                Products.Remove(product);
            }
        }
    }
}
