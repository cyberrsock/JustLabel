using System.Collections.Generic;
using JustLabel.Data.Models;
using NpgsqlTypes;

namespace IntegrationTests.Builders;

public class AreaDbModelBuilder
{
    private AreaDbModel _areaDbo = new();

    public AreaDbModelBuilder WithId(int id)
	{
		_areaDbo.Id = id;
		return this;
	}

    public AreaDbModelBuilder WithLabelId(int label_id)
	{
		_areaDbo.LabelId = label_id;
		return this;
	}

    public AreaDbModelBuilder WithCoords((double X, double Y)[] coords)
	{
		_areaDbo.Coords = coords.Select(c => new NpgsqlPoint(c.X, c.Y)).ToArray();
		return this;
	}

    public AreaDbModel Build()
	{
		return _areaDbo;
	}
}
