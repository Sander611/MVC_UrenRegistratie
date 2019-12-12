using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using QienUrenMVC.Models;
using QienUrenMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QienUrenMVC.Controllers
{
    public class ExportController : Controller
    {
        private readonly IHoursPerDayRepository hoursperdayRepo;
        private readonly IAccountRepository accountRepo;
        private readonly IHoursFormRepository hoursFormRepo;
        public ExportController(
                                IHoursPerDayRepository HoursPerDayRepo,
                                IAccountRepository AccountRepo,
                                IHoursFormRepository HoursFormRepo
                                )
        {
            accountRepo = AccountRepo;
            hoursperdayRepo = HoursPerDayRepo;
            hoursFormRepo = HoursFormRepo;

        }
        public async Task<FileStreamResult> CreateSpreadSheet(int id)
        {
            List<HoursPerDayModel> hours = await hoursperdayRepo.GetAllDaysForForm(id);
            AccountModel account = await accountRepo.GetAccountByFormId(id);
            HoursFormModel hoursForm = await hoursFormRepo.GetFormById(id);

            var stream = new MemoryStream();
            using (ExcelPackage ock = new ExcelPackage(stream))
            {
                ExcelWorksheet hoursWorksheet = ock.Workbook.Worksheets.Add("Uren Formulier");
                hoursWorksheet.Cells["A1"].Value = "Datum";
                hoursWorksheet.Cells["B1"].Value = "Opdracht";
                hoursWorksheet.Cells["C1"].Value = "Overwerk";
                hoursWorksheet.Cells["D1"].Value = "Verlof";
                hoursWorksheet.Cells["E1"].Value = "Ziek";
                hoursWorksheet.Cells["F1"].Value = "Verlof";
                hoursWorksheet.Cells["G1"].Value = "Overig";
                hoursWorksheet.Cells["H1"].Value = "Verklaring";

                hoursWorksheet.Cells["A1:H1"].Style.Font.Bold = true;

                int currentRow = 2;
                foreach (var hour in hours)
                {
                    hoursWorksheet.Cells["A" + currentRow.ToString()].Value = hour.Day;
                    hoursWorksheet.Cells["B" + currentRow.ToString()].Value = hour.Hours;
                    hoursWorksheet.Cells["C" + currentRow.ToString()].Value = hour.OverTimeHours;
                    hoursWorksheet.Cells["D" + currentRow.ToString()].Value = hour.IsLeave;
                    hoursWorksheet.Cells["E" + currentRow.ToString()].Value = hour.IsSick;
                    hoursWorksheet.Cells["F" + currentRow.ToString()].Value = hour.Training;
                    hoursWorksheet.Cells["G" + currentRow.ToString()].Value = hour.Other;
                    hoursWorksheet.Cells["H" + currentRow.ToString()].Value = hour.Reasoning;

                    currentRow++;
                }
                hoursWorksheet.View.FreezePanes(2, 1);

                hoursWorksheet.Cells["B" + (currentRow).ToString()].Formula = "SUM(B2:B" + (currentRow - 1).ToString() + ")";
                hoursWorksheet.Cells["C" + (currentRow).ToString()].Formula = "SUM(C2:C" + (currentRow - 1).ToString() + ")";
                hoursWorksheet.Cells["D" + (currentRow).ToString()].Formula = "SUM(D2:D" + (currentRow - 1).ToString() + ")";
                hoursWorksheet.Cells["E" + (currentRow).ToString()].Formula = "SUM(E2:E" + (currentRow - 1).ToString() + ")";
                hoursWorksheet.Cells["F" + (currentRow).ToString()].Formula = "SUM(F2:F" + (currentRow - 1).ToString() + ")";
                hoursWorksheet.Cells["G" + (currentRow).ToString()].Formula = "SUM(G2:G" + (currentRow - 1).ToString() + ")";

                hoursWorksheet.Cells["B" + (currentRow).ToString()].Style.Font.Bold = true;
                hoursWorksheet.Cells["C" + (currentRow).ToString()].Style.Font.Bold = true;
                hoursWorksheet.Cells["D" + (currentRow).ToString()].Style.Font.Bold = true;
                hoursWorksheet.Cells["E" + (currentRow).ToString()].Style.Font.Bold = true;
                hoursWorksheet.Cells["F" + (currentRow).ToString()].Style.Font.Bold = true;
                hoursWorksheet.Cells["G" + (currentRow).ToString()].Style.Font.Bold = true;
                hoursWorksheet.Cells["H" + (currentRow).ToString()].Style.Font.Bold = true;

                hoursWorksheet.Cells["B" + (currentRow).ToString()].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                hoursWorksheet.Cells["C" + (currentRow).ToString()].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                hoursWorksheet.Cells["D" + (currentRow).ToString()].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                hoursWorksheet.Cells["E" + (currentRow).ToString()].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                hoursWorksheet.Cells["F" + (currentRow).ToString()].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                hoursWorksheet.Cells["G" + (currentRow).ToString()].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                hoursWorksheet.Cells["H" + (currentRow).ToString()].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                ock.Save();
            }

            stream.Position = 0;
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"UrenFormulier {account.FirstName} {account.LastName} {hoursForm.ProjectMonth} {hoursForm.Year}.xls";
            FileStreamResult s = new FileStreamResult(stream, contentType)
            {
                FileDownloadName = fileName
            };

            return s;
        }
        public async Task<FileContentResult> ExportSCV(int id)
        {
            AccountModel account = await accountRepo.GetAccountByFormId(id);
            HoursFormModel hoursForm = await hoursFormRepo.GetFormById(id);
            var csvString = await GenerateCSVString(id);
            var fileName = $"Uren Formulier {account.LastName} { hoursForm.ProjectMonth} {hoursForm.Year}.csv";
            return File(new System.Text.UTF8Encoding().GetBytes(csvString), "text/csv", fileName);
        }
        private async Task<string> GenerateCSVString(int id)
        {
            List<HoursPerDayModel> hours = await hoursperdayRepo.GetAllDaysForForm(id);
            StringBuilder sb = new StringBuilder();
            sb.Append("Datum");
            sb.Append(",");
            sb.Append("Opdrachgever");
            sb.Append(",");
            sb.Append("Overwerk");
            sb.Append(",");
            sb.Append("Verlof");
            sb.Append(",");
            sb.Append("Ziek");
            sb.Append(",");
            sb.Append("Training");
            sb.Append(",");
            sb.Append("Overig");
            sb.Append(",");
            sb.Append("Verklaring");
            sb.AppendLine();
            foreach (var hour in hours)
            {
                sb.Append(hour.Day);
                sb.Append(",");
                sb.Append(hour.Hours);
                sb.Append(",");
                sb.Append(hour.OverTimeHours);
                sb.Append(",");
                sb.Append(hour.IsLeave);
                sb.Append(",");
                sb.Append(hour.IsSick);
                sb.Append(",");
                sb.Append(hour.Training);
                sb.Append(",");
                sb.Append(hour.Other);
                sb.Append(",");
                sb.Append(hour.Reasoning);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}