﻿using System.Collections.Generic;
using DOL.WHD.Section14c.Business.Validators;
using DOL.WHD.Section14c.Domain.Models;
using DOL.WHD.Section14c.Domain.Models.Submission;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DOL.WHD.Section14c.Test.Business.Validators
{
    [TestClass]
    public class ApplicationSubmissionValidatorTests
    {
        // dependencies
        private static readonly IAddressValidatorNoCounty AddressValidatorNoCounty = new AddressValidatorNoCounty();
        private static readonly IAddressValidator AddressValidator = new AddressValidator();
        private static readonly IWorkerCountInfoValidator WorkerCountInfoValidator = new WorkerCountInfoValidator();
        private static readonly IEmployerValidatorInitial EmployerValidatorInitial = new EmployerValidatorInitial(AddressValidator);
        private static readonly IEmployerValidatorRenewal EmployerValidatorRenewal = new EmployerValidatorRenewal(AddressValidator, WorkerCountInfoValidator);
        private static readonly ISourceEmployerValidator SourceEmployerValidator = new SourceEmployerValidator(AddressValidatorNoCounty);
        private static readonly IPrevailingWageSurveyInfoValidator PrevailingWageSurveyInfoValidator = new PrevailingWageSurveyInfoValidator(SourceEmployerValidator);
        private static readonly IAlternateWageDataValidator AlternateWageDataValidator = new AlternateWageDataValidator();
        private static readonly IHourlyWageInfoValidator HourlyWageInfoValidator = new HourlyWageInfoValidator(PrevailingWageSurveyInfoValidator, AlternateWageDataValidator);
        private static readonly IPieceRateWageInfoValidator PieceRateWageInfoValidator = new PieceRateWageInfoValidator(PrevailingWageSurveyInfoValidator, AlternateWageDataValidator);
        private static readonly IEmployeeValidator EmployeeValidator = new EmployeeValidator();
        private static readonly IWorkSiteValidatorInitial WorkSiteValidatorInitial = new WorkSiteValidatorInitial(AddressValidatorNoCounty);
        private static readonly IWorkSiteValidatorRenewal WorkSiteValidatorRenewal = new WorkSiteValidatorRenewal(AddressValidatorNoCounty, EmployeeValidator);
        private static readonly IWIOAWorkerValidator WIOAWorkerValidator = new WIOAWorkerValidator();
        private static readonly IWIOAValidator WIOAValidator = new WIOAValidator(WIOAWorkerValidator);
        private static readonly ISignatureValidator SignatureValidator = new SignatureValidator();

        private static readonly ApplicationSubmissionValidator ApplicationSubmissionValidator = new ApplicationSubmissionValidator(SignatureValidator, EmployerValidatorInitial, EmployerValidatorRenewal, HourlyWageInfoValidator, PieceRateWageInfoValidator, WorkSiteValidatorInitial, WorkSiteValidatorRenewal, WIOAValidator);

        [TestMethod]
        public void Should_Require_EIN()
        {
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.EIN, "");
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.EIN, "30-123457");
        }

        [TestMethod]
        public void Should_Require_ApplicationTypeId()
        {
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.ApplicationTypeId, null as int?);
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.ApplicationTypeId, 2);
        }

        [TestMethod]
        public void Should_Require_HasPreviousApplication()
        {
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.HasPreviousApplication, null as bool?);
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.HasPreviousApplication, false);
        }

        [TestMethod]
        public void Should_Require_EstablishmentType()
        {
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.EstablishmentType, null as ICollection<ApplicationSubmissionEstablishmentType>);
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.EstablishmentType, new List<ApplicationSubmissionEstablishmentType>());
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.EstablishmentType, new List<ApplicationSubmissionEstablishmentType> { new ApplicationSubmissionEstablishmentType { EstablishmentTypeId = ResponseIds.EstablishmentType.PatientWorkers } });
        }

        [TestMethod]
        public void Should_Require_ContactName()
        {
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.ContactFirstName, "");
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.ContactFirstName, "Contact First Name");
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.ContactLastName, "");
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.ContactLastName, "Contact Last Name");
        }

        [TestMethod]
        public void Should_Require_ContactPhone()
        {
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.ContactPhone, "");
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.ContactPhone, "123-456-7890");
        }

        [TestMethod]
        public void Should_Not_Require_ContactFax()
        {
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.ContactFax, "");
        }

        [TestMethod]
        public void Should_Require_ContactEmail()
        {
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.ContactEmail, "");
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.ContactEmail, "foo@bar.com");
        }

        [TestMethod]
        public void Should_Not_Require_PayTypeId_Initial()
        {
            var model = new ApplicationSubmission { ApplicationTypeId = ResponseIds.ApplicationType.Initial, PayTypeId = null };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.PayTypeId, model);
            model = new ApplicationSubmission { ApplicationTypeId = ResponseIds.ApplicationType.Renewal, PayTypeId = ResponseIds.PayType.PieceRate };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.PayTypeId, model);
        }

        [TestMethod]
        public void Should_Require_PayTypeId_Renewal()
        {
            var model = new ApplicationSubmission { ApplicationTypeId = ResponseIds.ApplicationType.Renewal, PayTypeId = null };
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.PayTypeId, model);
            model = new ApplicationSubmission { ApplicationTypeId = ResponseIds.ApplicationType.Renewal, PayTypeId = ResponseIds.PayType.PieceRate };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.PayTypeId, model);
        }

        [TestMethod]
        public void Should_Require_TotalNumWorkSites()
        {
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.TotalNumWorkSites, null as int?);
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.TotalNumWorkSites, 5);
        }

        [TestMethod]
        public void Should_Require_Employer()
        {
            var model = new ApplicationSubmission {ApplicationTypeId = ResponseIds.ApplicationType.Initial, Employer = null};
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.Employer, model);
            model = new ApplicationSubmission { ApplicationTypeId = ResponseIds.ApplicationType.Initial, Employer = new EmployerInfo() };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.Employer, model);
        }

        [TestMethod]
        public void Should_Require_WorkSites()
        {
            var model = new ApplicationSubmission { ApplicationTypeId = ResponseIds.ApplicationType.Initial, WorkSites = null };
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.WorkSites, model);
            model = new ApplicationSubmission { ApplicationTypeId = ResponseIds.ApplicationType.Initial, WorkSites = new List<WorkSite>() };
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.WorkSites, model);
            model = new ApplicationSubmission { ApplicationTypeId = ResponseIds.ApplicationType.Initial, WorkSites = new List<WorkSite> { new WorkSite() } };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.WorkSites, model);
        }

        [TestMethod]
        public void Should_Require_WIOA()
        {
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.WIOA, null as WIOA);
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.WIOA, new WIOA());
        }

        [TestMethod]
        public void Should_Require_PreviousCertificateNumber()
        {
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.PreviousCertificateNumber, "");
            var model = new ApplicationSubmission {HasPreviousCertificate = true, PreviousCertificateNumber = null};
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.PreviousCertificateNumber, model);
            model = new ApplicationSubmission { HasPreviousCertificate = true, PreviousCertificateNumber = "12345" };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.PreviousCertificateNumber, model);
        }

        [TestMethod]
        public void Should_Require_HourlyWageInfo()
        {
            var model = new ApplicationSubmission { PayTypeId = ResponseIds.PayType.PieceRate, HourlyWageInfo = null };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.HourlyWageInfo, model);
            model = new ApplicationSubmission { PayTypeId = ResponseIds.PayType.Hourly, HourlyWageInfo = new HourlyWageInfo() };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.HourlyWageInfo, model);
            model = new ApplicationSubmission { PayTypeId = ResponseIds.PayType.Both, HourlyWageInfo = new HourlyWageInfo() };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.HourlyWageInfo, model);
            model = new ApplicationSubmission {PayTypeId = ResponseIds.PayType.Hourly, HourlyWageInfo = null};
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.HourlyWageInfo, model);
            model = new ApplicationSubmission { PayTypeId = ResponseIds.PayType.Both, HourlyWageInfo = null };
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.HourlyWageInfo, model);
        }

        [TestMethod]
        public void Should_Require_PieceRateWageInfo()
        {
            var model = new ApplicationSubmission { PayTypeId = ResponseIds.PayType.Hourly, PieceRateWageInfo = null };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.PieceRateWageInfo, model);
            model = new ApplicationSubmission { PayTypeId = ResponseIds.PayType.PieceRate, PieceRateWageInfo = new PieceRateWageInfo() };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.PieceRateWageInfo, model);
            model = new ApplicationSubmission { PayTypeId = ResponseIds.PayType.Both, PieceRateWageInfo = new PieceRateWageInfo() };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.PieceRateWageInfo, model);
            model = new ApplicationSubmission { PayTypeId = ResponseIds.PayType.PieceRate, PieceRateWageInfo = null };
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.PieceRateWageInfo, model);
            model = new ApplicationSubmission { PayTypeId = ResponseIds.PayType.Both, PieceRateWageInfo = null };
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.PieceRateWageInfo, model);
        }

        [TestMethod]
        public void Should_Validate_ContactEmail()
        {
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.ContactEmail, "foo123");
        }

        [TestMethod]
        public void Should_Validate_ApplicationType()
        {
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.ApplicationTypeId, 5);
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.ApplicationTypeId, ResponseIds.ApplicationType.Renewal);
        }

        [TestMethod]
        public void Should_Validate_EstablishmentType()
        {
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.EstablishmentType, new List<ApplicationSubmissionEstablishmentType> { new ApplicationSubmissionEstablishmentType { EstablishmentTypeId = 9 } });
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.EstablishmentType, new List<ApplicationSubmissionEstablishmentType> { new ApplicationSubmissionEstablishmentType { EstablishmentTypeId = ResponseIds.EstablishmentType.PatientWorkers } });
        }

        [TestMethod]
        public void Should_Validate_PayType()
        {
            var model = new ApplicationSubmission { ApplicationTypeId = ResponseIds.ApplicationType.Renewal, PayTypeId = 30 };
            ApplicationSubmissionValidator.ShouldHaveValidationErrorFor(x => x.PayTypeId, model);
            model = new ApplicationSubmission { ApplicationTypeId = ResponseIds.ApplicationType.Renewal, PayTypeId = ResponseIds.PayType.PieceRate };
            ApplicationSubmissionValidator.ShouldNotHaveValidationErrorFor(x => x.PayTypeId, model);
        }
    }
}
