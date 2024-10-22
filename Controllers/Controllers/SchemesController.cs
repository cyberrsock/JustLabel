using Microsoft.AspNetCore.Mvc;
using JustLabel.Models;
using System;
using JustLabel.Services.Interfaces;
using JustLabel.Exceptions;
using JustLabel.DTOModels;
using System.Linq;

namespace JustLabel.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SchemesController : ControllerBase
{
    private ISchemeService _schemeService;
    private ILabelService _labelService;

    public SchemesController(ISchemeService schemeService, ILabelService labelService)
    {
        _schemeService = schemeService;
        _labelService = labelService;
    }

    [HttpPost]
    public object Add(SchemeDTOModel model)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        model.CreatorId = userId;

        SchemeModel scheme = new SchemeModel
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            CreatorId = model.CreatorId,
            CreateDatetime = model.CreateDatetime,
            LabelIds = model.LabelIds.Select(labelDto => new LabelModel
            {
                Id = labelDto.Id,
                Title = labelDto.Title
            }).ToList()
        };

        foreach (var lbl in scheme.LabelIds)
        {
            lbl.Id = _labelService.Add(lbl);
        }

        try
        {
            _schemeService.Add(scheme);
        }
        catch (SchemeException ex)
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
        var schemes = _schemeService.Get();
        var labels = _labelService.Get();

        schemes.ForEach(scheme => scheme.LabelIds.ForEach(labelId =>
        {
            var matchingLabel = labels.FirstOrDefault(label => label.Id == labelId.Id);
            if (matchingLabel != null)
            {
                labelId.Title = matchingLabel.Title;
            }
        }));

        return schemes;
    }

    [HttpGet("{id}")]
    public object Get(int id)
    {
        var scheme = _schemeService.Get(id);
        var labels = _labelService.Get();

        scheme.LabelIds.ForEach(labelId =>
        {
            var matchingLabel = labels.FirstOrDefault(label => label.Id == labelId.Id);
            if (matchingLabel != null)
            {
                labelId.Title = matchingLabel.Title;
            }
        });

        return scheme;
    }

    [HttpDelete("{id}")]
    public object Delete(int id)
    {
        _schemeService.Delete(id);

        return Ok();
    }
}
