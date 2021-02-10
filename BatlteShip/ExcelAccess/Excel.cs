using BatlteShip.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BatlteShip.ExcelAccess
{
    class Excel
    {
        public Excel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public async Task<List<Fields>> GetOpponentsData(FileInfo fi)
        {
            List<Fields> fieldsContent = new List<Fields>();
            using(var excelPackage = new ExcelPackage(fi))
            {
                await excelPackage.LoadAsync(fi);
                var ws = excelPackage.Workbook.Worksheets[0];
                int i = 1;
                int k = 0;
                char c = 'A';
                while (i <= 15 && c != 'G')
                {
                    string a = $"{c}{i}";
                    if (!String.IsNullOrWhiteSpace(ws.Cells[a].Value?.ToString()))
                    {
                        fieldsContent.Add(new Fields { FieldName=a});
                        k++;
                    }
                    i++;
                    if (i == 15)
                    {
                        i = 1;
                        c++;
                    }
                }
            }
            return (fieldsContent);
        }
        public async Task<int> CompGuess(FileInfo fi)
        {
            Random rnd = new Random();
            List<string> fields = new List<string>();
            int c = 0;
            int k = 0;
            using(var excelPackage = new ExcelPackage(fi))
            {
                await excelPackage.LoadAsync(fi);
                var ws = excelPackage.Workbook.Worksheets[0];
                int i = 1;
                char ca = 'A';
                while (i <= 15 && ca != 'G')
                {
                    string av = $"{ca}{i}";
                    if (!String.IsNullOrWhiteSpace(ws.Cells[av].Value?.ToString()))
                    {
                        fields.Add(av);
                    }
                    i++;
                    if (i == 15)
                    {
                        i = 1;
                        ca++;
                    }
                }
            }

            while (k < 10)
            {
                char first = (char)rnd.Next(65, 69);
                int num = rnd.Next(1, 15);
                if (fields.Contains($"{first}{num}"))
                {
                    c++;
                }
                k++;
            }
            return (c);
        }
    }
}
