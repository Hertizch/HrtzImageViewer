using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using HrtzImageViewer.Extensions;

namespace HrtzImageViewer.ViewModels
{
    public class ShellVm : ObservableObject
    {
        public ShellVm()
        { }

        private RelayCommand _cmdCloseApp;
        private RelayCommand _cmdOpenAsDialog;

        public RelayCommand CmdCloseApp
        {
            get
            {
                return _cmdCloseApp ??
                       (_cmdCloseApp = new RelayCommand(ExecuteCmd_CloseApp, p => true));
            }
        }

        public RelayCommand CmdOpenAsDialog
        {
            get
            {
                return _cmdOpenAsDialog ??
                       (_cmdOpenAsDialog = new RelayCommand(p => ExecuteCmd_OpenAsDialog(p as Uri), p => p != null));
            }
        }

        private static void ExecuteCmd_CloseApp(object obj)
        {
            Application.Current?.MainWindow?.Close();
        }

        public static void ExecuteCmd_OpenAsDialog(Uri sourceUri)
        {
            var sourcePath = sourceUri.AbsolutePath.Replace('/', Path.DirectorySeparatorChar);
            var args = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shell32.dll");
            args += ",OpenAs_RunDLL " + sourcePath;

            try
            {
                Process.Start("rundll32.exe", args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
