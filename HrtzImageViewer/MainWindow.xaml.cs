using System.Windows;
using System.Windows.Input;

namespace HrtzImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            /*
            BitmapImageCheck bic = new BitmapImageCheck();
            MessageBox.Show(bic.ToString());
            */
        }

        // this is the offset of the mouse cursor from the top left corner of the window
        private Point _offset;

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // capturing the mouse here will redirect all events to this window, even if
            // the mouse cursor should leave the window area
            Mouse.Capture(this, CaptureMode.Element);

            var cursorPos = PointToScreen(Mouse.GetPosition(this));
            var windowPos = new Point(Left, Top);
            _offset = (Point)(cursorPos - windowPos);
        }

        private void MainWindow_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!Equals(Mouse.Captured, this) || Mouse.LeftButton != MouseButtonState.Pressed) return;

            var cursorPos = PointToScreen(Mouse.GetPosition(this));
            var newLeft = cursorPos.X - _offset.X;
            var newTop = cursorPos.Y - _offset.Y;
            Left = newLeft;
            Top = newTop;
        }
    }
}
