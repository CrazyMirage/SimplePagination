using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestHandler.Models
{
    public class UsersPageModel
    {
        public IEnumerable<UserModel> Users { get; set; }
        public PaginationModel PagginationData { get; set; }

    }
}