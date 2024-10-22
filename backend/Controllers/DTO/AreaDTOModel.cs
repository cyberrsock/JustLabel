using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace JustLabel.DTOModels;

public class RectAreaDTOModel
{
    public int Id { get; set; }

    public int LabelId { get; set; }

    public int X1 { get; set; }

    public int Y1 { get; set; }

    public int X2 { get; set; }

    public int Y2 { get; set; }
}

public class AreaDTOModel
{
    public int Id { get; set; }

    public int LabelId { get; set; }

    public PointDTO[] Coords { get; set; }
}

public class PointDTO
{
    public double X { get; set; }
    public double Y { get; set; }
}
