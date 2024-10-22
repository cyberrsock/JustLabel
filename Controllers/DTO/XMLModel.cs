using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace JustLabel.DTOModels;

public class Size
{
    public int Width { get; set; }
    public int Height { get; set; }
}

public class Object
{
    public string Name { get; set; }
    public int Xmin { get; set; }
    public int Ymin { get; set; }
    public int Xmax { get; set; }
    public int Ymax { get; set; }
}

public class Annotation
{
    public string Folder { get; set; }
    public string Filename { get; set; }
    public Size Size { get; set; }
    public List<Object> Objects { get; set; }
}
