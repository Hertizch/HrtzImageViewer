using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using HrtzImageViewer.Helpers;
using HrtzImageViewer.ViewModels;

namespace HrtzImageViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            if (e != null && e.Args.Length > 0)
            {
                var imageSource = e.Args[0];

                if (!string.IsNullOrEmpty(imageSource))
                {
                    if (FileHelpers.IsValidImage(imageSource))
                    {
                        CurrentImageVm.Instance.CurrentImage.BitmapImage = new BitmapImage(new Uri(imageSource));
                    }
                    else
                    {
                        Debug.WriteLine("Not a valid image file");
                        CurrentImageVm.Instance.CurrentImage.LoadError = true;
                        CurrentImageVm.Instance.CurrentImage.ErrorMessage = "Not a valid image file";
                    }
                }
                else
                {
                    Debug.WriteLine("No startup arguments provided");
                    CurrentImageVm.Instance.CurrentImage.LoadError = true;
                    CurrentImageVm.Instance.CurrentImage.ErrorMessage = "No startup arguments provided";
                }
            }
            else
            {
                Debug.WriteLine("No startup arguments provided");
                CurrentImageVm.Instance.CurrentImage.LoadError = true;
                CurrentImageVm.Instance.CurrentImage.ErrorMessage = "No startup arguments provided";
            }

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
