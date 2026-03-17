using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Dtos
{
    public class ExportFileResultDto
    {
        public byte[] FileContent { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }

        public ExportFileResultDto(byte[] fileContent, string fileName, string contentType)
        {
            FileContent = fileContent;
            FileName = fileName;
            ContentType = contentType;
        }
    }
}