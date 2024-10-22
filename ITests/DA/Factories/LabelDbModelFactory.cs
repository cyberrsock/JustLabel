using JustLabel.Models;
using JustLabel.Data.Models;

namespace IntegrationTests.Factories;

public static class LabelDbModelFactory
{
    public static LabelDbModel Create(int id, string title)
    {
        return new LabelDbModel
        {
            Id = id,
            Title = title
        };
    }

    public static LabelDbModel Create(LabelModel model)
    {
        return new LabelDbModel
        {
            Id = model.Id,
            Title = model.Title
        };
    }
}
