using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using JustLabel.Models;
using JustLabel.Services.Interfaces;
using JustLabel.DTOModels;
using JustLabel.Exceptions;
using System.Linq;

namespace JustLabel.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MarksController : ControllerBase
{
    private IMarkedService _markedService;

    public MarksController(IMarkedService markedService)
    {
        _markedService = markedService;
    }

    [HttpPost]
    [MapToApiVersion("1.0")]
    public object Add(RectMarkDTOModel model)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        model.CreatorId = userId;

        MarkedModel markModel = new MarkedModel
        {
            Id = model.Id,
            SchemeId = model.SchemeId,
            ImageId = model.ImageId,
            CreatorId = model.CreatorId,
            IsBlocked = model.IsBlocked,
            CreateDatetime = model.CreateDatetime,
            AreaModels = model.AreaModels.Select(areaDto => new AreaModel
            {
                Id = areaDto.Id,
                LabelId = areaDto.LabelId,
                Coords = new[]
                {
                    new Point { X = areaDto.X1, Y = areaDto.Y1 },
                    new Point { X = areaDto.X2, Y = areaDto.Y1 },
                    new Point { X = areaDto.X2, Y = areaDto.Y2 },
                    new Point { X = areaDto.X1, Y = areaDto.Y2 }
                }
            }).ToList()
        };

        MarkedModel findModel = _markedService.Get(markModel);

        if (findModel is null && markModel.AreaModels.Count > 0)
        {
            try
            {
                _markedService.Create(markModel);
            }
            catch (MarkedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        else
        {
            markModel.Id = findModel.Id;
            try
            {
                _markedService.Update_Rects(markModel);
            }
            catch (MarkedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        return Ok();
    }

    [HttpPut]
    [MapToApiVersion("1.0")]
    public object UpdateBlock(RectMarkDTOModel model)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        model.CreatorId = userId;

        MarkedModel markModel = new MarkedModel
        {
            Id = model.Id,
            SchemeId = model.SchemeId,
            ImageId = model.ImageId,
            CreatorId = model.CreatorId,
            IsBlocked = model.IsBlocked,
            CreateDatetime = model.CreateDatetime,
            AreaModels = model.AreaModels.Select(areaDto => new AreaModel
            {
                Id = areaDto.Id,
                LabelId = areaDto.LabelId,
                Coords = new[]
                {
                    new Point { X = areaDto.X1, Y = areaDto.Y1 },
                    new Point { X = areaDto.X2, Y = areaDto.Y1 },
                    new Point { X = areaDto.X2, Y = areaDto.Y2 },
                    new Point { X = areaDto.X1, Y = areaDto.Y2 }
                }
            }).ToList()
        };

        try
        {
            _markedService.Update_Block(markModel);
        }
        catch (MarkedException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }

        return Ok();
    }

    [HttpPost]
    [MapToApiVersion("2.0")]
    public object Add2(MarkDTOModel model)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        model.CreatorId = userId;

        MarkedModel markModel = new MarkedModel
        {
            Id = model.Id,
            SchemeId = model.SchemeId,
            ImageId = model.ImageId,
            CreatorId = model.CreatorId,
            IsBlocked = model.IsBlocked,
            CreateDatetime = model.CreateDatetime,
            AreaModels = model.AreaModels.Select(areaDto => new AreaModel
            {
                Id = areaDto.Id,
                LabelId = areaDto.LabelId,
                Coords = areaDto.Coords.Select(coord => new Point { X = coord.X, Y = coord.Y }).ToArray()
            }).ToList()
        };

        MarkedModel findModel = _markedService.Get(markModel);

        if (findModel is null && markModel.AreaModels.Count > 0)
        {
            try
            {
                _markedService.Create(markModel);
            }
            catch (MarkedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        else
        {
            markModel.Id = findModel.Id;
            try
            {
                _markedService.Update_Rects(markModel);
            }
            catch (MarkedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        return Ok();
    }

    [HttpPut]
    [MapToApiVersion("2.0")]
    public object UpdateBlock2(MarkDTOModel model)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        model.CreatorId = userId;

        MarkedModel markModel = new MarkedModel
        {
            Id = model.Id,
            SchemeId = model.SchemeId,
            ImageId = model.ImageId,
            CreatorId = model.CreatorId,
            IsBlocked = model.IsBlocked,
            CreateDatetime = model.CreateDatetime,
            AreaModels = model.AreaModels.Select(areaDto => new AreaModel
            {
                Id = areaDto.Id,
                LabelId = areaDto.LabelId,
                Coords = areaDto.Coords.Select(coord => new Point { X = coord.X, Y = coord.Y }).ToArray()
            }).ToList()
        };

        try
        {
            _markedService.Update_Block(markModel);
        }
        catch (MarkedException ex)
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
    public object Get([FromQuery] List<int> ids)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        var res = _markedService.GetAll(userId);
        List<MarkGroupDTOModel> foundMarks = res
            .Where(item => ids.Contains(item.CreatorId))
            .GroupBy(item => item.CreatorId)
            .Select(group => new MarkGroupDTOModel
            {
                UserId = group.Key,
                Marks = group.ToList()
            }).ToList();
        return Ok(foundMarks);
    }

    [HttpGet("{id}")]
    public object Get(int id)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        return Ok(_markedService.Get(id, userId));
    }

    [HttpDelete("{id}")]
    public object Delete(int id)
    {
        _markedService.Delete(id);
        return Ok();
    }
}