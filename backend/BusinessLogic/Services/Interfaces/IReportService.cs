using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.Services.Interfaces;

public interface IReportService
{
    void Create(ReportModel model);

    List<ReportModel> Get();
}
