using System.Linq;
using System.Windows;

namespace Stride.Roslyn.EditorServices.Presentation
{
    internal static class WindowExtensions
    {
        public static void SetOwnerToActive(this Window window)
        {
            window.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
        }
    }
}
