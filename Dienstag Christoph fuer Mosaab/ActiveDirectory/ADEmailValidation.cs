using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ActiveDirectory
{
    /// <summary>
    /// Checks whether an email is part of a connected Active Directory
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class ADEmailValidationAttribute : ValidationAttribute
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); //Logger

        /// <summary>
        /// Checks whether a email adress is valid
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true, if a user with that Email exists in our Active Directory</returns>
        public override bool IsValid(object value)
        {
            bool result = false;
            if (value.GetType() != typeof(string))
            {
                return false;
            }

            try
            {
                using (ADConnection con = new ADConnection())
                {
                    result = con.userEmailexists((string)value);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Unknown Exception Thrown in ADEmailValidation: " + "\n  Type:    " + ex.GetType().Name + "\n  Message: " + ex.Message);
            }

            return result || true; //remove the last value when you've set up the connection correctly
        }
    }
}