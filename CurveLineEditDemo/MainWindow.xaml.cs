using System.Collections.Concurrent;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurveLineEditDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private ConcurrentQueue<Point> _points = new ConcurrentQueue<Point>();
        private void TestCanvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(TestCanvas);
            AddPoint(position);
            _points.Enqueue(position);
        }

        private void CompensateButton_OnClick(object sender, RoutedEventArgs e)
        {
            TestCanvas.Children.Clear();
            //显示补偿点
            var predictPoints = SmoothAlgorithm.GetFittingLinePoints(_points.ToList());
            foreach (var predictPoint in predictPoints)
            {
                AddPoint(predictPoint, Brushes.Red);
            }
            //显示触摸点
            foreach (var point in _points)
            {
                AddPoint(point);
            }
        }

        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            TestCanvas.Children.Clear();
            _points = new ConcurrentQueue<Point>();
        }

        private void AddPoint(Point position, Brush? brush = null)
        {
            var ellipseSize = 20;
            var ellipse = new Ellipse() { Fill = brush ?? Brushes.Green, Width = ellipseSize, Height = ellipseSize };
            var ellipseToolTip = $"{TestCanvas.Children.Count + 1}:({position.X},{position.Y})";
            ellipse.ToolTip = ellipseToolTip;
            Canvas.SetTop(ellipse, position.Y - ellipseSize / 2d);
            Canvas.SetLeft(ellipse, position.X - ellipseSize / 2d);
            TestCanvas.Children.Add(ellipse);
        }
    }
}