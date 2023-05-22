using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Zad4
{
    public partial class MainWindow : Window
    {
        Point start;
        Rectangle? currentRectangle;
        double lengthOfMove = 15;
        int colorId = 0;
        public MainWindow() => InitializeComponent();
        private void Rectangle_MouseReleased(object sender, MouseButtonEventArgs e) => start = new();
        private void Rectangle_MousePressed(object sender, MouseButtonEventArgs e)
        {
            start = e.GetPosition(canvas);
            currentRectangle = new Rectangle
            {
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };
            Canvas.SetLeft(currentRectangle, start.X);
            Canvas.SetTop(currentRectangle, start.Y);
            canvas.Children.Add(currentRectangle);
        }
        private void Rectangle_MouseMove(object sender, MouseEventArgs mouse)
        {
            if (mouse.LeftButton == MouseButtonState.Pressed)
            {
                double x, y, width, height;

                if (start.X < mouse.GetPosition(canvas).X) x = start.X;
                else x = mouse.GetPosition(canvas).X;

                if (start.Y < mouse.GetPosition(canvas).Y) y = start.Y;
                else y = mouse.GetPosition(canvas).Y;

                Canvas.SetLeft(currentRectangle, x);
                Canvas.SetTop(currentRectangle, y);

                if (start.X < mouse.GetPosition(canvas).X) width = mouse.GetPosition(canvas).X - start.X;
                else width = start.X - mouse.GetPosition(canvas).X;

                if (start.Y < mouse.GetPosition(canvas).Y) height = mouse.GetPosition(canvas).Y - start.Y;
                else height = start.Y - mouse.GetPosition(canvas).Y;

                if (currentRectangle != null)
                {
                    currentRectangle.Height = height;
                    currentRectangle.Width = width;
                }
            }
        }
        private void KeyPressed(object sender, KeyEventArgs keyEvent)
        {
            if (currentRectangle != null)
            {
                if (keyEvent.Key == Key.Left && Keyboard.Modifiers == ModifierKeys.Shift) currentRectangle.StrokeThickness++;
                else if (keyEvent.Key == Key.Right && Keyboard.Modifiers == ModifierKeys.Shift) currentRectangle.StrokeThickness--;
                else if (keyEvent.Key == Key.Up && Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    colorId++;
                    currentRectangle.Stroke = ChangeColor();
                }
                else if (keyEvent.Key == Key.Down && Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    colorId--;
                    currentRectangle.Stroke = ChangeColor();
                }

                if (keyEvent.Key == Key.Left && Keyboard.Modifiers != ModifierKeys.Shift) Canvas.SetLeft(currentRectangle, Canvas.GetLeft(currentRectangle) - lengthOfMove);
                else if (keyEvent.Key == Key.Right && Keyboard.Modifiers != ModifierKeys.Shift) Canvas.SetLeft(currentRectangle, Canvas.GetLeft(currentRectangle) + lengthOfMove);
                else if (keyEvent.Key == Key.Up && Keyboard.Modifiers != ModifierKeys.Shift) Canvas.SetTop(currentRectangle, Canvas.GetTop(currentRectangle) - lengthOfMove);
                else if (keyEvent.Key == Key.Down && Keyboard.Modifiers != ModifierKeys.Shift) Canvas.SetTop(currentRectangle, Canvas.GetTop(currentRectangle) + lengthOfMove);
            }
        }
        private SolidColorBrush ChangeColor()
        {
            if (colorId-- < 0) colorId = 3;
            else if (colorId++ > 2) colorId = 0;

            if (colorId == 0) return new SolidColorBrush(Colors.Black);
            else if (colorId == 1) return new SolidColorBrush(Colors.Purple);
            else if (colorId == 2) return new SolidColorBrush(Colors.Blue);
            return new SolidColorBrush(Colors.Red);
        }
    }
}

