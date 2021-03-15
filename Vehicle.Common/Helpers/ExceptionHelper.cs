using System;
using System.Data.Entity.Validation;

namespace Vehicle.Common.Helpers
{
    public static class ExceptionHelper
    {
        public static Exception GetValidationException(DbEntityValidationException dbEx)
        {
            var msg = string.Empty;

            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    msg += Environment.NewLine + string.Format("Prorperty: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                }
            }

           return new Exception(msg, dbEx);
        }
    }
}
