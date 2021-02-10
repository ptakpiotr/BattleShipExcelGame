using BatlteShip.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatlteShip.Validation
{
    class FieldsRules:AbstractValidator<Fields>
    {
        public FieldsRules()
        {
            RuleFor(fi=>fi.FieldName).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("{PropertyName} cannot be empty!").Must(FieldNameValidation).
                WithMessage("Invalid field!");
        }
        private bool FieldNameValidation(string a)
        {
            int c = (int)a[0];
            return (a.Length>=2&&a.Length<=3&&c>=65&&c<=70&&Char.IsDigit(a[a.Length-1]));
        }
    }
}
