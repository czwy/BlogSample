using System.Windows.Markup;

namespace ChinesePhoneticizeFuzzyMatch.Converter
{
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
