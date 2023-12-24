using FluentValidation;
using WebApplicationTicketsCRUD.Db.Models;
using WebApplicationTicketsCRUD.Dto;

namespace WebApplicationTicketsCRUD.Validators;

public class TicketValidator : AbstractValidator<RequestTicketDto>
{
    public TicketValidator()
    {
        RuleFor(x => x.OwnerFirstName).Length(3, 10);
        RuleFor(x => x.OwnerLastName).Length(5, 10);
        RuleFor(x => x.Phone).Matches(@"\+7\([0-9]{3}\)[0-9]{3}-[0-9]{2}-[0-9]{2}");
    }
}