using QuickLook.Common.Plugin;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace QuickLook.Plugin.VideoMPF
{
    public class Plugin : IViewer
    {
        //private MediaElement _vpf;
        private ViewerPanel _vpf;
        public int Priority => int.MaxValue;


        public void Init()
        {
        }

        public bool CanHandle(string path)
        {
            return !Directory.Exists(path) && path.ToLower().EndsWith(".mp4");
        }

        public void Prepare(string path, ContextObject context) {

                int width = 500;
                int height = 300;

                var windowSize = new Size {
                    Width = Math.Max(100, width == 0 ? 1366 : width),
                    Height = Math.Max(100, height == 0 ? 768 : height)
                };

                context.SetPreferredSizeFit(windowSize, 0.8);

                context.TitlebarAutoHide = true;
                context.Theme = Themes.Dark;
                context.TitlebarBlurVisibility = true;

            context.TitlebarOverlap = true;
        }

        public void View(string path, ContextObject context)
        {
            //var viewer = new Label {Content = "I am a Label. I do nothing at all."};
            //context.ViewerContent = viewer;

            //_vpf = new MediaElement();
            //_vpf.Source = new Uri(Path.GetFileName(path));
            //context.ViewerContent = _vpf;
            //_vpf.Play();
            //context.IsBusy = false;

            _vpf = new ViewerPanel(context, path);
            context.ViewerContent = _vpf;
            context.Title = $"{Path.GetFileName(path)}";
            _vpf.PlaySong();
        }

        public void Cleanup()
        {
            _vpf?.Dispose(true);
            //_vpf?.Close();
            _vpf = null;
        }
    }
}