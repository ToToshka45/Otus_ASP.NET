using PromoCodeFactory.Core.Domain.Administration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.Core.Contracts.Employee
{
    /// <summary>
    /// Данные для создания нового сотрудника
    /// </summary>
    public class CreateEmployeeRequest
    {

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        //[Required]
        [StringLength( 64, ErrorMessage = "{0} length can't be more than {1} and less than {2}.", MinimumLength = 4 )]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        //[Required]
        [StringLength( 64, ErrorMessage = "{0} length can't be more than {1} and less than {2}.", MinimumLength = 4 )]
        public string LastName { get; set; }

        /// <summary>
        /// Email сотрудника
        /// </summary>
        //[Required]
        [StringLength( 64, ErrorMessage = "{0} length can't be more than {1} and less than {2}.", MinimumLength = 4 )]
        public string Email { get; set; }

    }
}
