using FluentValidation;
using MyLibrary.Business.Dtos.V1.ServiceTypeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Business.Validators.V1.ServiceType
{
    public class CreateServiceTypeDtoValidator : AbstractValidator<CreateServiceTypeDto>
    {
        public CreateServiceTypeDtoValidator()
        {
            RuleFor(x => x.Type).NotEmpty().WithMessage("Service type is required.")
                .MaximumLength(20).WithMessage("Service type maximum length is 20.");
        }
    }
}
