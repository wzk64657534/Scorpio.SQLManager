using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scorpio.Core.Model;
using Scorpio.Core.Service;
using Microsoft.AspNetCore.Authorization;

namespace Scorpio.Web.Controllers
{
    [Authorize]
    public class SQLManagerController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
        public ActionResult GetSQLTableList()
        {
            List<Table> tableList = SQLManagerService.GetSQLTableList();

            ResultModel<List<Table>> result = new ResultModel<List<Table>>()
            {
                code = 0,
                msg = string.Empty,
                count = 0,
                data = tableList
            };
            return Json(result);
        }
        public IActionResult Table(string tableName)
        {
            List<TableStructure> tableStructures = SQLManagerService.GetTableStructure(tableName);
            if (tableStructures != null && tableStructures.Count > 0)
            {
                for (int i = 0; i < tableStructures.Count; i++)
                {
                    tableStructures[i].TableName = tableStructures[0].TableName;
                }
            }
            ResultModel<List<TableStructure>> result = new ResultModel<List<TableStructure>>()
            {
                code = 0,
                msg = string.Empty,
                count = 0,
                data = tableStructures
            };
            return Json(result);
        }

        [HttpPost]
        public IActionResult AddOrUpdateTableFieldExplain(string table, string column, string value,int operationType)
        {
            int count = 0;
            if (operationType == 0)
            {
                count = SQLManagerService.AddTableFieldExplain(table, column, value);
            }
            else
            {
                count= SQLManagerService.UpdateTableFieldExplain(table, column, value);
            }               
            return Json(count);
        }
        [HttpPost]
        public IActionResult AddOrUpdateTableExplain(string table, string value, int operationType)
        {
            int count = 0;
            if (operationType == 0)
            {
                count = SQLManagerService.AddTableExplain(table, value);
            }
            else
            {
                count = SQLManagerService.UpdateTableExplain(table, value);
            }
            return Json(count);
        }
    }
}