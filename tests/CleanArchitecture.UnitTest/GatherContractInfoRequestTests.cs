﻿// ------------------------------------------------------------
// Copyright (c) James Eastham.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Requests;
using CleanArchitecture.Core.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace CleanArchitecture.UnitTest
{
    public class GatherContractInfoRequestTests
    {
        private const string Name = "James";
        private const string Address = "My address, my town, my street, AA1 7AA";
        private const string NationalInsuranceNumber = "1236789 C";
        private readonly DateTime dateOfBirth = new DateTime(1907, 01, 01);

        [Fact]
        public void CanCreate()
        {
            var gatherContractInfoRequest = new GatherContactInfoRequest(Name, Address, this.dateOfBirth, NationalInsuranceNumber);

            gatherContractInfoRequest.Should().NotBeNull();
        }

        [Fact]
        public void CanExecute_ShouldComplete()
        {
            var handler = this.CreateHandler(requiredCreditScore: 600);

            var gatherContractInfoRequest = new GatherContactInfoRequest(Name, Address, this.dateOfBirth, NationalInsuranceNumber);

            var gatherContractInfoResponse = handler.Handle(gatherContractInfoRequest);

            gatherContractInfoResponse.Should().NotBeNull();
            gatherContractInfoResponse.Errors.Count().Should().Be(0);
        }

        [Fact]
        public void CanExecute_ShouldReturnErrorOnInvalidName()
        {
            var handler = this.CreateHandler(requiredCreditScore: 600);

            var gatherContractInfoRequest = new GatherContactInfoRequest(string.Empty, Address, this.dateOfBirth, NationalInsuranceNumber);

            var gatherContractInfoResponse = handler.Handle(gatherContractInfoRequest);

            gatherContractInfoResponse.Errors.Count().Should().Be(1);
        }

        [Fact]
        public void CanExecute_ShouldReturnErrorOnEmptyAddress()
        {
            var handler = this.CreateHandler(requiredCreditScore: 600);

            var gatherContractInfoRequest = new GatherContactInfoRequest(Name, string.Empty, this.dateOfBirth, NationalInsuranceNumber);

            var gatherContractInfoResponse = handler.Handle(gatherContractInfoRequest);

            gatherContractInfoResponse.Errors.Count().Should().Be(1);
        }

        [Fact]
        public void CanExecute_ShouldReturnErrorOnAge()
        {
            var handler = this.CreateHandler(requiredCreditScore: 600);

            var gatherContractInfoRequest = new GatherContactInfoRequest(Name, Address, DateTime.Now.AddYears(-1), NationalInsuranceNumber);

            var gatherContractInfoResponse = handler.Handle(gatherContractInfoRequest);

            gatherContractInfoResponse.Errors.Count().Should().Be(1);
        }

        [Fact]
        public void CanExecute_ShouldReturnErrorOnNiNumber()
        {
            var handler = this.CreateHandler(requiredCreditScore: 600);

            var gatherContractInfoRequest = new GatherContactInfoRequest(Name, Address, this.dateOfBirth, string.Empty);

            var gatherContractInfoResponse = handler.Handle(gatherContractInfoRequest);

            gatherContractInfoResponse.Errors.Count().Should().Be(1);
        }

        [Fact]
        public void CanExecute_ShouldReturnErrorOnCreditScore()
        {
            var handler = this.CreateHandler(requiredCreditScore: 150);

            var gatherContractInfoRequest = new GatherContactInfoRequest(Name, Address, this.dateOfBirth, NationalInsuranceNumber);

            var gatherContractInfoResponse = handler.Handle(gatherContractInfoRequest);

            gatherContractInfoResponse.Errors.Count().Should().Be(1);
        }

        private GatherContactInfoInteractor CreateHandler(decimal requiredCreditScore)
        {
            var creditScoreChecker = new Mock<ICreditScoreService>();
            creditScoreChecker.Setup(p => p.GetCreditScore(It.IsAny<string>())).Returns(requiredCreditScore).Verifiable();

            var customerDatabase = new Mock<ICustomerDatabase>();

            return new GatherContactInfoInteractor(creditScoreChecker.Object, customerDatabase.Object);
        }
    }
}
