using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BlinkLed
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GpioPin _pin;
        private readonly DispatcherTimer _timer;
        private GpioPinValue _pinValue;
        const int LED_PIN = 27;

        public MainPage()
        {
            this.InitializeComponent();
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _timer.Tick += (s, e) =>
            {
                _pinValue = _pinValue == GpioPinValue.High ? GpioPinValue.Low : GpioPinValue.High;

                _pin.Write(_pinValue);
            };
            InitGPIO();

        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                _pin = null;
                GpioText.Text = "There is no GPIO controller on this device.";
                return;
            }

            _pin = gpio.OpenPin(LED_PIN);
            _pin.Write(GpioPinValue.High);
            _pin.SetDriveMode(GpioPinDriveMode.Output);

            GpioText.Text = "GPIO pin initialized correctly.";

        }

        private void StartStopClick(object sender, RoutedEventArgs e)
        {
            if (_pin != null)
            {
                if (_timer.IsEnabled)
                {
                    _timer.Stop();
                    _pin.Write(GpioPinValue.High);
                    StartStopButton.Content = "Start";
                }
                else
                {
                    _timer.Start();
                    StartStopButton.Content = "Stop";
                }
            }
        }
    }
}
