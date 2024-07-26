using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace DateTimePickerSample.Helper
{
    [ContentProperty("Boundary")]
    public class DateTimeValidationRule : ValidationRule
    {
        public ValidationParams Boundary { get; set; }
        public BoundaryType Type { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime result;
            if (DateTime.TryParse(value.ToString(), out result) == false)
                return new ValidationResult(false, "请输入正确的时间");
            else if (result.Date == default(DateTime))
                return new ValidationResult(false, "请选择有效的时间");
            else if (Boundary != null && Type != BoundaryType.UnKnown)
            {
                DateTime DateFloor = default(DateTime);
                DateTime DateCeiling = default(DateTime);
                if (Boundary.Param1 != null)
                    DateTime.TryParse(Boundary.Param1.ToString(), out DateFloor);
                if (Boundary.Param2 != null)
                    DateTime.TryParse(Boundary.Param2.ToString(), out DateCeiling);
                if (Type == BoundaryType.Floor && DateFloor >= result)
                    return new ValidationResult(false, $"请输入{DateFloor}之后的时间");
                else if (Type == BoundaryType.Ceiling && DateCeiling <= result)
                    return new ValidationResult(false, $"请输入{DateCeiling}之前的时间");
                else if (Type == BoundaryType.Range && (DateFloor >= result || DateCeiling <= result))
                    return new ValidationResult(false, $"请输入{DateFloor}和{DateCeiling}之间的时间");
                else
                    return new ValidationResult(true, null);
            }
            else
                return new ValidationResult(true, null);
        }
    }

    public enum BoundaryType
    {
        /// <summary>
        /// 未知或未定义
        /// </summary>
        UnKnown,
        /// <summary>
        /// 只有最小边界
        /// </summary>
        Floor,
        /// <summary>
        /// 只有最大边界
        /// </summary>
        Ceiling,
        /// <summary>
        /// 包含最小和最大边界
        /// </summary>
        Range
    }
}
