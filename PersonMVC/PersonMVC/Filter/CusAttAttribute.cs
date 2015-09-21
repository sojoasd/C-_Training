using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace personMVC.Filter
{
    public class CusAttAttribute : ValidationAttribute
    {
        private int _CheckAge;

        public CusAttAttribute(int CheckAge)
        {
            this._CheckAge = CheckAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime birthdate = (DateTime)value;
            int age = new DateTime(DateTime.Now.Subtract(birthdate).Ticks).Year - 1;

            if (age >= _CheckAge)
            {
                return ValidationResult.Success;
            }
            else
            {
                var msg = string.Format("你的年齡是({0})，未滿({1})不得申請", age, _CheckAge);
                return new ValidationResult(msg);
            }
        }
    }
}