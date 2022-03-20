using System.Text;

namespace Map2Data;

public class SVGData
{
    public SVGData(string path) => Path = path;

    public SVGData(string name, string path) : this(path) => Name = name;

    public string Path { get; }

    public string Name { get; set; } = "";

    public bool Selected { get; set; } = false;

    public static IEnumerable<SVGData> ProcessSVG(string svg)
    {
        var split = svg.Split("d=\"").Where(x => x.StartsWith("m "));
        var paths = split.Where(x => x.StartsWith("m "));
        var trimmed = paths.Select(x => x.Split(" z\" />")[0] + " z");
        return trimmed.Select(x => new SVGData(x));
    }

    public static List<SVGData> ProcessCSV(string csv)
    {
        var data = csv.Split('\n');
        List<SVGData> final = new(data.Length);
        foreach (var item in data)
        {
            var split = item.Split(',');
            final.Add(new SVGData(split[0], split[1]));
        }
        return final;
    }

    public static string ToCSV(IEnumerable<SVGData> data)
    {
        StringBuilder sb = new();
        foreach(var item in data)
        {
            sb.Append(item.Name);
            sb.Append(',');
            sb.Append(item.Path);
            sb.Append('\n');
        }
        return sb.ToString();
    }
}
