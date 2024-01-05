using FluentValidation;
using WebApplicationTicketsCRUD.Dto;

namespace WebApplicationTicketsCRUD.Validators;

public class UserValidator : AbstractValidator<RequestUserDto>
{
    public UserValidator()
    {
        RuleFor(x => x.Email).Matches( @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)");
    }
}