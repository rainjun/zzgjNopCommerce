using FluentValidation;
using Nop.Admin.Models.Customers;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Localization;
using System;

namespace Nop.Admin.Validators.Customers
{
    public class CustomerValidator : AbstractValidator<CustomerModel>
    {
        public CustomerValidator(ILocalizationService localizationService, CustomerSettings customerSettings)
        {
            ////rainAdd 增加身份证验证
            RuleFor(x => x.IdentityId).Must((identity) =>
            {
                DateTime? tempDate = null;
                bool isIdentityId = CommonHelper.IsValidIdCardNo(identity, out tempDate);
                return isIdentityId;
            }).WithMessage(localizationService.GetResource("Common.WrongIdentityId"))
                .When(x => !string.IsNullOrWhiteSpace(x.IdentityId));
            if (customerSettings.StateProvinceEnabled)
            {
                RuleFor(x => x.StateProvinceId).NotEmpty().GreaterThan(0).WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.StateProvinceId.Required"));
            }
            //form fields
            if (customerSettings.CompanyRequired && customerSettings.CompanyEnabled)
                RuleFor(x => x.Company).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Company.Required"));
            if (customerSettings.StreetAddressRequired && customerSettings.StreetAddressEnabled) 
                RuleFor(x => x.StreetAddress).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.StreetAddress.Required"));
            if (customerSettings.StreetAddress2Required && customerSettings.StreetAddress2Enabled)
                RuleFor(x => x.StreetAddress2).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.StreetAddress2.Required"));
            if (customerSettings.ZipPostalCodeRequired && customerSettings.ZipPostalCodeEnabled)
                RuleFor(x => x.ZipPostalCode).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.ZipPostalCode.Required"));
            if (customerSettings.CityRequired && customerSettings.CityEnabled)
                RuleFor(x => x.City).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.City.Required"));
            if (customerSettings.PhoneRequired && customerSettings.PhoneEnabled)
                RuleFor(x => x.Phone).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Phone.Required"));
            if (customerSettings.FaxRequired && customerSettings.FaxEnabled) 
                RuleFor(x => x.Fax).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Fax.Required"));
        }
    }
}