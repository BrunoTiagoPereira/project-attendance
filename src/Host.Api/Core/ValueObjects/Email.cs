using Host.Api.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Host.Api.Core.ValueObjects
{
    public class Email : ValueObject<string>
    {
        public Email(string value) : base(value)
        {
            if(!new EmailAddressAttribute().IsValid(value))
            {
                throw new DomainException("Email inválido.");
            }
        }
    }
}
