using System;
using System.Windows;
using System.Windows.Media.Imaging;
using HrtzImageViewer.Extensions;
using HrtzImageViewer.Models;

namespace HrtzImageViewer.ViewModels
{
    public class CurrentImageVm : ObservableObject
    {
        public CurrentImageVm()
        {
            CurrentImage = new CurrentImage();
        }

        public static CurrentImageVm Instance { get; set; } = new CurrentImageVm();

        private CurrentImage _currentImage;

        public CurrentImage CurrentImage
        {
            get { return _currentImage; }
            set { SetField(ref _currentImage, value); }
        }
    }
}
