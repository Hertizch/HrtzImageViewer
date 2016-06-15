using System.Net.Mime;
using System.Windows.Media.Imaging;
using HrtzImageViewer.Extensions;

namespace HrtzImageViewer.Models
{
    public class CurrentImage : ObservableObject
    {
        private BitmapImage _bitmapImage;
        private bool _loadError;
        private string _errorMessage;

        public BitmapImage BitmapImage
        {
            get { return _bitmapImage; }
            set { SetField(ref _bitmapImage, value); }
        }

        public bool LoadError
        {
            get { return _loadError; }
            set { SetField(ref _loadError, value); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetField(ref _errorMessage, value); }
        }
    }
}
