using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.Repositories.Interfaces;

public interface IReportRepository
{
    void Create(ReportModel model);

    List<ReportModel> GetAll();
}
