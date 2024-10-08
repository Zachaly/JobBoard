﻿using FluentValidation;
using JobBoard.Model.Business;

namespace JobBoard.Application.Validation
{
    public class UpdateBusinessRequestValidator : AbstractValidator<UpdateBusinessRequest>
    {
        public UpdateBusinessRequestValidator()
        {
            RuleFor(r => r.Name).Length(1, 100);
        }
    }
}
