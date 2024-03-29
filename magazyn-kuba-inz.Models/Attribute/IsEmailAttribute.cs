﻿using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models.Attribute;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
public class IsEmailAttribute : ValidationAttribute
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public IsEmailAttribute(string errorMessage) :base(errorMessage)
    {

    }

    public IsEmailAttribute() : base()
    {

    }

    #endregion


    #region Public method

    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }
        if (value is string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        else
        {
            throw new InvalidCastException();
        }

    }

    #endregion

}