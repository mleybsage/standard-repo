﻿// ------------------------------------------------------------
// Copyright (c) James Eastham.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("CleanArchitecture.UnitTest")]

namespace CleanArchitecture.Core.Entities
{
    /// <summary>
    /// Encapsulates all logic for a customer entity.
    /// </summary>
    public class Customer
    {
        private int _age;

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        /// <param name="name">The new customers name.</param>
        /// <param name="address">The new customers address.</param>
        /// <param name="dateOfBirth">The new customers date of birth.</param>
        /// <param name="nationalInsuranceNumber">The new customers national insurance number.</param>
        internal Customer(string name, string address, DateTime dateOfBirth, string nationalInsuranceNumber)
        {
            this.CustomerId = Guid.NewGuid().ToString();
            this.Name = name;
            this.Address = address;
            this.DateOfBirth = dateOfBirth;
            this.NationalInsuranceNumber = nationalInsuranceNumber;
        }

        private Customer()
        {
        }

        /// <summary>
        /// Gets the internal identifier of this customer.
        /// </summary>
        public string CustomerId { get; private set; }

        /// <summary>
        /// Gets the name of the customer.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the address of the customer.
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Gets the customers date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// Gets the national insurance number of the customer.
        /// </summary>
        public string NationalInsuranceNumber { get; private set; }

        /// <summary>
        /// Gets the customers credit score.
        /// </summary>
        public decimal CreditScore { get; private set; }

        /// <summary>
        /// Gets the age of the person based on their <see cref="Customer.DateOfBirth"/>.
        /// </summary>
        public int Age
        {
            get
            {
                if (this._age <= 0)
                {
                    this._age = new DateTime(DateTime.Now.Subtract(this.DateOfBirth).Ticks).Year - 1;
                }

                return this._age;
            }
        }

        internal void UpdateCreditScore(decimal newCreditScore)
        {
            if (newCreditScore != this.CreditScore)
            {
                this.CreditScore = newCreditScore;
            }
        }
    }
}
