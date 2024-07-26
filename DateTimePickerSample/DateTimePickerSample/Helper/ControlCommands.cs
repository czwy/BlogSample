using System.Windows.Input;

namespace DateTimePickerSample.Helper
{
    /// <summary>
    /// 控件库自定义的命令
    /// </summary>
    public static class ControlCommands
    {
        /// <summary>
        ///     搜索
        /// </summary>
        public static RoutedCommand Search { get; } = new RoutedCommand(nameof(Search), typeof(ControlCommands));

        /// <summary>
        ///     清除
        /// </summary>
        public static RoutedCommand Clear { get; } = new RoutedCommand(nameof(Clear), typeof(ControlCommands));

        /// <summary>
        ///     切换
        /// </summary>
        public static RoutedCommand Switch { get; } = new RoutedCommand(nameof(Switch), typeof(ControlCommands));

    }
}
