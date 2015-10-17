using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

namespace AppInsightsTutorial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private TelemetryClient tc = new TelemetryClient();

        public MainWindow()
        {
            tc.InstrumentationKey = "--YOUR INSTRUMENTATION KEY HERE--";
            tc.Context.User.Id = "tutorial1";
            tc.Context.Properties["app_start"] = DateTime.Now.ToString(CultureInfo.InvariantCulture.DateTimeFormat);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            InitializeComponent();
        }

        private int button1Counter = 0;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var metric = new MetricTelemetry("button1", (++button1Counter));
            tc.TrackMetric(metric);
            tc.Flush();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var ev = new EventTelemetry("button2");
            ev.Properties["click_time"] = DateTime.Now.ToString(CultureInfo.InvariantCulture.DateTimeFormat);
            tc.TrackEvent(ev);
            tc.Flush();
        }

        private void buttonCrash_Click(object sender, RoutedEventArgs e)
        {
            object foo = null;
            string crash = foo.ToString();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception) { 
                tc.TrackException(new ExceptionTelemetry((Exception) e.ExceptionObject));
                tc.Flush();
            }
        }

    }
}
