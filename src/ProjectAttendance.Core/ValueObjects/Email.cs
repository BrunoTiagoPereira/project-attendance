using ProjectAttendance.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace ProjectAttendance.Core.ValueObjects
{
    public class Email : ValueObject<string>
    {
        public Email(string value) : base(value)
        {
            if (!new EmailAddressAttribute().IsValid(value))
            {
                throw new DomainException("Email inválido.");
            }
        }
    }
}