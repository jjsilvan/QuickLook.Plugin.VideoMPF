using QuickLook.Common.Annotations;
using QuickLook.Common.Plugin;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using System.Windows.Threading;

namespace QuickLook.Plugin.VideoMPF 
{

    public partial class ViewerPanel : UserControl, IDisposable, INotifyPropertyChanged {
        private readonly ContextObject _context;
        DispatcherTimer timer;
        bool IsSeeked;
        bool IsPlay;
        string pathFile;
        private bool disposedValue;

        public ViewerPanel(ContextObject context)
        {
            InitializeComponent();
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Seek_Timer;
            timer.Start();

            LoadIcon();

            IsSeeked = false;
            IsPlay = false;

            _context = context;

        }

        public event System.Windows.RoutedEventHandler MediaOpened;
        protected virtual void OnMediaOpened() {
            PlayOrPause.ToolTip = MyMediaElement.ActualWidth.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetSource (string path) 
		{
            IsPlay = false;
            pathFile = path;
            _context.IsBusy = false;

            if (File.Exists(pathFile))
            {
                PlaySong();
            }
        }
        private void Seek_Timer(object sender, EventArgs e)
        {
            if (sender== null)
                return;
            if ((MyMediaElement.Source != null) && (MyMediaElement.NaturalDuration.HasTimeSpan) && (!IsSeeked))
            {
                Seeker.Minimum = 0;
                Seeker.Maximum = MyMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                Seeker.Value = MyMediaElement.Position.TotalSeconds;
            }
        }
        private void Seek_Drag_Started(object sender, DragStartedEventArgs e)
        {
            IsSeeked = true;
        }
        private void Seek_Drag_Completed(object sender, DragCompletedEventArgs e)
        {
            IsSeeked = false;
            MyMediaElement.Position = TimeSpan.FromSeconds(Seeker.Value);
        }
        private void Seek_Value_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PlayTime.Text = TimeSpan.FromSeconds(Seeker.Value).ToString(@"hh\:mm\:ss");
        }
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MyMediaElement.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }
        private void LoadIcon()
        {
            try
            {
                PlayOrPause.Content = new Image { Source = new BitmapImage(new Uri(FilePathInfo.Play)) };
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void ActionListener(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            switch (btn.Uid)
            {
                case "Play":
                    if (IsPlay)
                    {
                        PauseSong();
                    }
                    else
                    {                        
                        PlaySong();
                    }
                    break;

            }
            
        }
        public void PlaySong()
        {
            IsPlay = true;
            PlayOrPause.Content = new Image { Source = new BitmapImage(new Uri(FilePathInfo.Pause)) };
            PlayOrPause.ToolTip = "Pause";
            MyMediaElement.Play();
        }
        public void PauseSong()
        {
            IsPlay = false;
            PlayOrPause.Content = new Image { Source = new BitmapImage(new Uri(FilePathInfo.Play)) };
            PlayOrPause.ToolTip = "Play";
            MyMediaElement.Pause();
        }
        public void StopSong()
        {
            MyMediaElement.Stop();
            _context.IsBusy = false;
        }

        public void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {


                }
                try {
                    timer?.Stop();
                    MyMediaElement?.Close();

                    Task.Run(() =>
                    {
                        MyMediaElement = null;
                    });
                    } catch (Exception e) {
                        Debug.WriteLine(e);
                    }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~ViewerPanel()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose() {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
