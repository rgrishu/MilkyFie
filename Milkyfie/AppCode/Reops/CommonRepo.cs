using Dapper;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Reops
{
    public class CommonRepo : ICommon
    {
        private IDapperRepository _dapper;
        public CommonRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }







      
    }

}
