using System;
using Microsoft.AspNetCore.Mvc;
using JustLabel.Models;
using JustLabel.Services.Interfaces;
using JustLabel.Exceptions;
using JustLabel.DTOModels;

namespace JustLabel.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LabelsController : ControllerBase
{
    private ILabelService _labelService;

    public LabelsController(ILabelService labelService)
    {
        _labelService = labelService;
    }

    [HttpPost]
    public object Add(LabelDTOModel model)
    {
        LabelModel labelModel = new LabelModel
        {
            Id = model.Id,
            Title = model.Title
        };
        _labelService.Add(labelModel);
        return Ok();
    }

    [HttpGet]
    public object Get()
    {
        return Ok(_labelService.Get());
    }

}
