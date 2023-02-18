using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Model_AP
{
    [ValueConversion(typeof(int), typeof(string))]
    public class IntToStringConverter : IValueConverter
    {
        UserCollection users = UserCollection.GetAllUsers();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            else           
                return (from u in users where u.Id == (int)value select u.UserName).SingleOrDefault();          
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var convertedValue = 0;
            try
            {
                string uiValue = value as string;
                convertedValue = (from u in users where u.UserName == uiValue select u.Id).SingleOrDefault();
            }
            catch (Exception exc)
            {
                //ignoring errors
            }
            return convertedValue;
        }
    }
}