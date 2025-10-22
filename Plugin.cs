using QuickLook.Common.Plugin;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace QuickLook.Plugin.VideoMPF
{
    public class Plugin : IViewer
    {
        private ViewerPanel _vpf;
        public int Priority => 0;

        public void Init()
        {
        }

        public bool CanHandle(string path)
        {
            return !Directory.Exists(path) && path.ToLower().EndsWith(".zzz");
        }

        public void Prepare(string path, ContextObject context)
        {
            context.PreferredSize = new Size {Width = 600, Height = 400};
        }

        public void View(string path, ContextObject context)
        {
            //var viewer = new Label {Content = "I am a Label. I do nothing at all."};
            //context.ViewerContent = viewer;

            _vpf = new ViewerPanel(context);
            context.ViewerContent = _vpf;
            context.Title = $"{Path.GetFileName(path)}";
            _vpf.SetSource(path);

            //context.IsBusy = false;
        }

        public void Cleanup()
        {
            _vpf?.Dispose(true);
            _vpf = null;
        }
    }
}