using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.DataMongoDb;
using JustLabel.DataMongoDb.Converters;

namespace JustLabel.Repositories.MongoDb;

public class MarkedRepositoryMongoDb : IMarkedRepository
{
    private AppDbContextMongoDb _context;
    private readonly ILogger _logger;

    public MarkedRepositoryMongoDb(AppDbContextMongoDb context)
    {
        _context = context;
        _logger = Log.ForContext<MarkedRepositoryMongoDb>();
    }

    public void Create(MarkedModel model)
    {
        _logger.Debug($"Attempt to create marked for image {model.ImageId}");
        model.CreateDatetime = DateTime.Now;
        bool g = _context.Marked.Any();
        int newId = g ? _context.Marked.Select(u => u.Id).Max() : 0;
        model.Id = newId + 1;
        var markedDbModel = MarkedConverter.CoreToDbModel(model);
        _context.Marked.Add(markedDbModel);
        _context.SaveChanges();

        bool g2 = _context.Areas.Any();
        int newId2 = g2 ? _context.Areas.Select(u => u.Id).Max() : 0;

        foreach (var r in model.AreaModels)
        {
            newId2++;
            r.Id = newId2;
            var gg = AreaConverter.CoreToDbModel(r);
            _context.Areas.Add(gg);
            _context.SaveChanges();
            r.Id = _context.Areas.OrderByDescending(m => m.Id).FirstOrDefault().Id;
        }

        bool g3 = _context.MarkedAreas.Any();
        int newId3 = g3 ? _context.MarkedAreas.Select(u => u.Id).Max() : 0;

        var labelSchemeDbModels = MarkedConverter.CoreToDbConnectModel(model);
        foreach (var labelSchemeModel in labelSchemeDbModels)
        {
            newId3++;
            labelSchemeModel.Id = newId3;
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
        var gg = _context.Areas.ToList();
        List<MarkedModel> res = marks
           .Select(m => MarkedConverter.DbToCoreModel(m, _context.MarkedAreas.Where(u => u.MarkedId == m.Id).ToList(), gg))
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
        var gg = _context.Marked.FirstOrDefault(u => u.ImageId == model.ImageId && u.CreatorId == model.CreatorId && u.SchemeId == model.SchemeId);
        var ggg = _context.MarkedAreas.ToList();
        var gggg = _context.Areas.ToList();
        MarkedModel res = MarkedConverter.DbToCoreModel(gg, ggg, gggg);
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
        var gg = _context.Areas.ToList();
        List<MarkedModel> res = _context.Marked.AsEnumerable()
           .Select(m => MarkedConverter.DbToCoreModel(m, _context.MarkedAreas.Where(u => u.MarkedId == m.Id).ToList(), gg))
           .ToList();
        _logger.Debug($"Marked successfully got");
        return res;
    }

    public List<AggregatedModel> GetAggregatedData(int datasetId, int schemeId)
    {
        throw new NotImplementedException();
    }
}
