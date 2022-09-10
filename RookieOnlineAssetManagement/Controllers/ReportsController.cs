using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using RookieOnlineAssetManagement.Service.IServices;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Route("getlist")]
        public async Task<IActionResult> GetList(int? page, int? pageSize, string sortOrder, string sortField)
        {
            var result = await _reportService.GetListReportAsync(page, pageSize, sortOrder, sortField);
            if (result == null) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ExportToExcel()
        {
            var reports = await _reportService.GetReportsAsync();

            // Start exporting to Excel
            var stream = new MemoryStream();

            using (var xlPackage = new ExcelPackage(stream))
            {
                // Define a worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Reports");

                // Styling
                var customStyle = xlPackage.Workbook.Styles.CreateNamedStyle("CustomStyle");
                customStyle.Style.Font.UnderLine = true;
                customStyle.Style.Font.Color.SetColor(Color.Red);

                // First row
                var startRow = 5;
                var row = startRow;

                worksheet.Cells["A1"].Value = "Number Of Assets By Category & State";
                worksheet.Cells["A2"].Value = "Exported Date";
                worksheet.Cells["B2"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);
                using (var r = worksheet.Cells["A1:C1"])
                {
                    r.Merge = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Font.Color.SetColor(Color.Red);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                }

                worksheet.Cells["A4"].Value = "#";
                worksheet.Cells["B4"].Value = "Category";
                worksheet.Cells["C4"].Value = "Total";
                worksheet.Cells["D4"].Value = "Assigned";
                worksheet.Cells["E4"].Value = "Available";
                worksheet.Cells["F4"].Value = "Not Available";
                worksheet.Cells["G4"].Value = "Waiting for recycling";
                worksheet.Cells["H4"].Value = "Recycled";

                worksheet.Cells["A4:H4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A4:H4"].Style.Font.Bold = true;
                worksheet.Cells["A4:H4"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                
                row = 5;
                int count = 1;
                foreach (var report in reports)
                {
                    worksheet.Cells[row, 1].Value = count;
                    worksheet.Cells[row, 2].Value = report.Category;
                    worksheet.Cells[row, 3].Value = report.Total;
                    worksheet.Cells[row, 4].Value = report.Assigned;
                    worksheet.Cells[row, 5].Value = report.Available;
                    worksheet.Cells[row, 6].Value = report.NotAvailable;
                    worksheet.Cells[row, 7].Value = report.WaitingForRecycling;
                    worksheet.Cells[row, 8].Value = report.Recycled;

                    count++;
                    row++;
                }

                worksheet.Cells["A:AZ"].AutoFitColumns();
                xlPackage.Workbook.Properties.Title = "Asset List By Category Report";
                xlPackage.Workbook.Properties.Author = "Rookies";
                
                xlPackage.Save();
            }
            
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reports.xlsx");
        }
    }
}
