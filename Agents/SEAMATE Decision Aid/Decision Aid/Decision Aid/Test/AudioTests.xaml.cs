using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Media;

namespace Decision_Aid.Test
{
    /// <summary>
    /// Interaction logic for AudioTests.xaml
    /// </summary>
    public partial class AudioTests : Window
    {
        private static SoundPlayer _blip;
        private static SoundPlayer _ding;
        private static SoundPlayer _alarm;

        public AudioTests()
        {
            InitializeComponent();
            _blip = new SoundPlayer(Properties.Resources.blip);
            _blip.Load();
            _ding = new SoundPlayer(Properties.Resources.ding);
            _ding.Load();
            _alarm = new SoundPlayer(Properties.Resources.alarm);
            _alarm.Load();

            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //blip
            _blip.Play();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //mid-range
            _ding.Play();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //alarm
            _alarm.Play();
        }
    }
}
