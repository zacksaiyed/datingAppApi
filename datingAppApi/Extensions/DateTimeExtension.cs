using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace datingAppApi.Extensions
{
    public static class DateTimeExtension
    {
        public static int CalcualteAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddDays(-age)) age--;
            return age;
        }
    }
}
