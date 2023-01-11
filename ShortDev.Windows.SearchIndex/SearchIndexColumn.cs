namespace ShortDev.Windows.SearchIndex;

public sealed class SearchIndexColumn
{
    public string Name { get; set; }

    /// <summary>
    /// <see href="https://docs.microsoft.com/en-us/windows/win32/properties/core-bumper">All available column names</see>
    /// </summary>
    public SearchIndexColumn(string name)
    {
        this.Name = name;
    }

    public static SearchIndexColumn ItemPathDisplay { get => new("System.ItemPathDisplay"); }
    public static SearchIndexColumn FileExtension { get => new("System.FileExtension"); }
    public static SearchIndexColumn Author { get => new("System.Author"); }
    public static SearchIndexColumn OriginalFileName { get => new("System.OriginalFileName"); }
    public static SearchIndexColumn FileDescription { get => new("System.FileDescription"); }
    public static SearchIndexColumn FileVersion { get => new("System.FileVersion"); }

}
