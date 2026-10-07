using System.Windows.Controls;

namespace CadBridgeDesigner.Tool.Loger;

public partial class LogerView : UserControl
{
    public LogerView(LogerViewModel logerViewModel)
    {
        InitializeComponent();
        DataContext = logerViewModel;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        LogTextBox.ScrollToEnd();
    }
}

