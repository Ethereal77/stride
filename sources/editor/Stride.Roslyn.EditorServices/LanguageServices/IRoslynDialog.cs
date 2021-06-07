namespace Stride.Roslyn.EditorServices.LanguageServices
{
    internal interface IRoslynDialog
    {
        object ViewModel { get; set; }

        bool? Show();
    }
}