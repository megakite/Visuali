using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Colourful;
using Colourful.Conversion;

namespace LabS {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void SliderColors_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            //Petition to add pure vector method
            LabColor lab = new LabColor(sliderL.Value, slidera.Value, sliderb.Value);
            RGBColor rgb = new ColourfulConverter() { WhitePoint = Illuminants.D50 }.ToRGB(lab);
            buttonColor.Background = new SolidColorBrush(Color.FromRgb(
                Convert.ToByte(rgb.R * 255d),
                Convert.ToByte(rgb.G * 255d),
                Convert.ToByte(rgb.B * 255d)));
            buttonColor.Content = "#" +
                Convert.ToByte(rgb.R * 255d).ToString("x2") +
                Convert.ToByte(rgb.G * 255d).ToString("x2") +
                Convert.ToByte(rgb.B * 255d).ToString("x2");
            buttonColor.Foreground = lab.L > 70 ? Brushes.Black : Brushes.White;
            try {
                sliderS.Value = Math.Sqrt(Math.Pow(lab.a, 2) + Math.Pow(lab.b, 2)) /
                    Math.Sqrt(Math.Pow(lab.L, 2) + Math.Pow(lab.a, 2) + Math.Pow(lab.b, 2)) * 100d;
                double ratio = Math.Min(lab.a, lab.b) / Math.Max(lab.a, lab.b) * 100d;
                sliderS.Maximum = Math.Sqrt(10000 + Math.Pow(ratio, 2)) /
                    Math.Sqrt(Math.Pow(lab.L, 2) + 10000 + Math.Pow(ratio, 2)) * 100d;
            } catch (ArgumentException) {
                sliderS.Value = lab.a == 0 && lab.b == 0 ? 0 : 100;
            }
        }

        private void WindowMain_MouseEnter(object sender, MouseEventArgs e) => windowMain.Opacity = 1;

        private void WindowMain_MouseLeave(object sender, MouseEventArgs e) => windowMain.Opacity = 0.5;

        private void ButtonColor_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(buttonColor.Content.ToString());

        private void DragBar_MouseDown(object sender, MouseButtonEventArgs e) {
            try {
                DragMove();
            } catch (InvalidOperationException) {
                Close();
            }
        }
    }
}
