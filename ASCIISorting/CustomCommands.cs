
using System.Windows.Input;

namespace ASCIISorting
{
    class CustomCommands
    {
        public static RoutedUICommand Open = new RoutedUICommand("Open", "cmOpen", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.O, ModifierKeys.Alt) });
        public static RoutedUICommand Save = new RoutedUICommand("Save", "cmSave", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.S, ModifierKeys.Alt) });
        public static RoutedUICommand Sort = new RoutedUICommand("Sort", "cmSort", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.A, ModifierKeys.Alt) });
        public static RoutedUICommand Reverse = new RoutedUICommand("Reverse", "cmReverse", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Alt)});
        public static RoutedUICommand Exit = new RoutedUICommand("Exit", "cmExit", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.F4, ModifierKeys.Alt) });

    }
}
