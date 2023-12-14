using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using UraniumUI;

namespace AppInterface.ViewModels;

public class Folder : ObservableObject
{
    private string path;
    public Folder(string name, string path, bool isFile = false)
    {
        Name = name;
        this.path = path;
        IsFile = isFile;
    }
    public string Name { get; set; }
    public bool IsFile { get; set; }
    public IList<Folder> Children { get; set; } = new ObservableCollection<Folder>();
}
