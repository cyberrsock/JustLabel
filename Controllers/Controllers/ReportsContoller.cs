using System;
using Microsoft.AspNetCore.Mvc;
using JustLabel.Models;
using JustLabel.Services.Interfaces;
using JustLabel.DTOModels;
using JustLabel.Exceptions;

namespace JustReport.Controllers;

[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ReportsController : ControllerBase
{
    private IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpPost]
    public object Add(ReportDTOModel model)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        model.CreatorId = userId;

        ReportModel report = new ReportModel
        {
            Id = model.Id,
            MarkedId = model.MarkedId,
            CreatorId = model.CreatorId,
            Comment = model.Comment,
            LoadDatetime = model.LoadDatetime
        };
        
        try
        {
            _reportService.Create(report);
        }
        catch (ReportException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }

        return Ok();
    }

    [HttpGet]
    public object Get()
    {
        return Ok(_reportService.Get());
    }

}
