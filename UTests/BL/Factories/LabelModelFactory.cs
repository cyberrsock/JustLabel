using JustLabel.Models;
using JustLabel.Data.Models;

namespace UnitTests.Factories;

public static class LabelModelFactory
{
    public static LabelModel Create(int id, string title)
    {
        return new LabelModel
        {
            Id = id,
            Title = title
        };
    }

    public static LabelModel Create(LabelDbModel model)
    {
        return new LabelModel
        {
            Id = model.Id,
            Title = model.Title
        };
    }
}
