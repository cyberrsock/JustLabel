using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Data;
using JustLabel.Data.Converters;
using Microsoft.EntityFrameworkCore;
using JustLabel.Data.Models;
using JustLabel.DataMongoDb.Converters;

namespace JustLabel.Repositories;

public class MarkedRepository : IMarkedRepository
{
    private AppDbContext _context;
    private readonly ILogger _logger;

    public MarkedRepository(AppDbContext context)
    {
        _context = context;
        _logger = Log.ForContext<MarkedRepository>();
    }

    public void Create(MarkedModel model)
    {
        _logger.Debug($"Attempt to create marked for image {model.ImageId}");
        model.CreateDatetime = DateTime.Now;
        var markedDbModel = MarkedConverter.CoreToDbModel(model);
        _context.Marked.Add(markedDbModel);
        _context.SaveChanges();

        foreach (var r in model.AreaModels)
        {
            _context.Areas.Add(AreaConverter.CoreToDbModel(r));
            _context.SaveChanges();
            r.Id = _context.Areas.OrderByDescending(m => m.Id).FirstOrDefault().Id;
        }

        var latestMarked = _context.Marked.OrderByDescending(d => d.CreateDatetime).FirstOrDefault();
        model.Id = latestMarked.Id;
        var labelSchemeDbModels = MarkedConverter.CoreToDbConnectModel(model);
        foreach (var labelSchemeModel in labelSchemeDbModels)
        {
            _context.MarkedAreas.Add(labelSchemeModel);
        }
        _context.SaveChanges();
        _logger.Debug($"Marked for image {model.ImageId} successfully added");
    }

    public void Delete(int id)
    {
        _logger.Debug($"Attempt to delete a marked ID{id}");
        var marked = _context.Marked.FirstOrDefault(u => u.Id == id);
        if (marked is not null)
        {
            _context.Marked.Remove(marked);
            _context.SaveChanges();
        }
        _logger.Debug($"Marked ID{id} successfully deleted");
    }

    public List<MarkedModel> Get_By_DatasetId(int id)
    {
        _logger.Debug($"Attempt to get a marked ID{id}");
        var imagesDbModels = _context.Images.Where(u => u.DatasetId == id).ToList();
        var images = imagesDbModels.Select(model => ImageConverter.DbToCoreModel(model)).Select(i => i.Id).ToList();
        var marks = _context.Marked.Where(m => images.Contains(m.ImageId)).ToList();
        List<MarkedModel> res = marks
           .Select(m => MarkedConverter.DbToCoreModel(m, _context.MarkedAreas.Where(u => u.MarkedId == m.Id).ToList(), _context.Areas.ToList()))
           .ToList();
        _logger.Debug($"Marked ID{id} successfully got");
        return res;
    }

    public List<MarkedModel> Get_By_SchemeId(int id)
    {
        _logger.Debug($"Attempt to get a marked ID{id}");
        var marks = _context.Marked.Where(u => u.SchemeId == id).ToList();
        List<MarkedModel> res = marks
           .Select(m => MarkedConverter.DbToCoreModel(m, _context.MarkedAreas.Where(u => u.MarkedId == m.Id).ToList(), _context.Areas.ToList()))
           .ToList();
        _logger.Debug($"Marked ID{id} successfully got");
        return res;
    }

    public List<MarkedModel> Get_By_Dataset_and_SchemeId(int datasetId, int schemeId)
    {
        var imagesDbModels = _context.Images.Where(u => u.DatasetId == datasetId).ToList();
        var images = imagesDbModels.Select(model => ImageConverter.DbToCoreModel(model)).Select(i => i.Id).ToList();
        var marks = _context.Marked.Where(m => images.Contains(m.ImageId) && m.SchemeId == schemeId).ToList();
        List<MarkedModel> res = marks
           .Select(m => MarkedConverter.DbToCoreModel(m, _context.MarkedAreas.Where(u => u.MarkedId == m.Id).ToList(), _context.Areas.ToList()))
           .ToList();
        return res;
    }

    public MarkedModel Get(MarkedModel model)
    {
         _logger.Debug($"Attempt to get a marked ID{model.Id}");
        MarkedModel res = MarkedConverter.DbToCoreModel(_context.Marked.FirstOrDefault(u => u.ImageId == model.ImageId && u.CreatorId == model.CreatorId && u.SchemeId == model.SchemeId),
        _context.MarkedAreas.ToList(), _context.Areas.ToList());
        _logger.Debug($"Marked ID{model.Id} successfully got");
        return res;
    }

    public void Update_Rects(MarkedModel model)
    {
        _logger.Debug($"Attemp to update marked ID{model.Id}");
        var marked = _context.Marked.FirstOrDefault(u => u.Id == model.Id);

        if (marked is not null)
        {
            var markedAreasToRemove = _context.MarkedAreas.Where(a => a.MarkedId == model.Id);
            var areaIdsToRemove = markedAreasToRemove.Select(a => a.AreaId).ToList();
            var areasToRemove = _context.Areas.Where(a => areaIdsToRemove.Contains(a.Id)).ToList();
            _context.MarkedAreas.RemoveRange(markedAreasToRemove);
            _context.Areas.RemoveRange(areasToRemove);
            _context.SaveChanges();

            if (model.AreaModels.Count > 0)
            {
                marked.CreateDatetime = DateTime.Now;
                marked.IsBlocked = model.IsBlocked;
                foreach (var r in model.AreaModels)
                {
                    _context.Areas.Add(AreaConverter.CoreToDbModel(r));
                    _context.SaveChanges();
                    r.Id = _context.Areas.OrderByDescending(m => m.Id).FirstOrDefault().Id;
                }

                var labelSchemeDbModels = MarkedConverter.CoreToDbConnectModel(model);
                foreach (var labelSchemeModel in labelSchemeDbModels)
                {
                    _context.MarkedAreas.Add(labelSchemeModel);
                }
            }
            else
            {
                _context.Marked.Remove(marked);
            }
            _context.SaveChanges();
        }

        _logger.Debug($"Marked ID{model.Id} successfully updated");
    }

    public void Update_Block(MarkedModel model)
    {
        _logger.Debug($"Attemp to update marked ID{model.Id}");
        var marked = _context.Marked.FirstOrDefault(u => u.Id == model.Id);

        if (marked is not null)
        {
            marked.IsBlocked = model.IsBlocked;
             _context.SaveChanges();
        }

        _logger.Debug($"Marked ID{model.Id} successfully updated");
    }


    public List<MarkedModel> GetAll()
    {
        _logger.Debug($"Attemp to get marked");
        List<MarkedModel> res = _context.Marked
           .Select(m => MarkedConverter.DbToCoreModel(m, _context.MarkedAreas.Where(u => u.MarkedId == m.Id).ToList(), _context.Areas.ToList()))
           .ToList();
        _logger.Debug($"Marked successfully got");
        return res;
    }

    public List<AggregatedModel> GetAggregatedData(int datasetId, int schemeId)
    {
        _logger.Debug($"Attempting to execute calculate_aggregation SQL function {datasetId}, {schemeId}");
        var aggregatedData = _context.Aggregated.FromSqlRaw($"SELECT * FROM calc_aggregation({datasetId}, {schemeId}, 0.5, 2)").AsEnumerable().Select(u => AggregatedConverter.DbToCoreModel(u)).ToList();
        _logger.Debug("calculate_aggregation SQL function executed");
        return aggregatedData;
    }
}
