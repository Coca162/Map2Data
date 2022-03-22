using System.Text;

namespace Map2Data;

public class Map
{
    public Map((string, string) size, List<SVGData> counties)
    {
        Size = size;
        States = counties;
    }

    public ValueTuple<string, string> Size { get; set; }

    public List<SVGData> States { get; }

    public static Map ProcessSVG(string svg)
    {
        var split = svg.Split("<path")
            .Where(x => x.Contains("d=\"", StringComparison.InvariantCultureIgnoreCase))
            .Where(x => !x.StartsWith('<'));
        var trimmed = split.Select(x => x.Split("/>")[0]);
        var info = trimmed.Select(x => x.Split("d=\""));

        var final = info.Select(x => (x[1].Split('"')[0], x[2].Split('"')[0]));

        var tuple = (svg.Split("width=\"")[1].Split('"')[0], svg.Split("height=\"")[1].Split('"')[0]);

        return new(tuple, final.Select(x => new SVGData(x.Item2, district: x.Item1)).ToList());
    }

    public static Map ProcessCSV(string csv)
    {
        var data = csv.Split('\n');

        var cordinates = data[0].Split(',');
        var tuple = (cordinates[0], cordinates[1]);

        List<SVGData> final = new(data.Length);
        foreach (var item in data.Skip(1))
        {
            var split = item.Split(',');
            final.Add(new SVGData(split[1], name: split[0], district: split[2]));
        }
        return new(tuple, final);
    }

    public static string ToCSV(Map map, bool onlyNamed)
    {
        StringBuilder sb = new();

        sb.Append(map.Size.Item1);
        sb.Append(','); 
        sb.Append(map.Size.Item2);
        sb.Append('\n');

        foreach (var item in map.States.Where(x => !onlyNamed && string.IsNullOrWhiteSpace(x.Name)))
        {
            sb.Append(item.Name);
            sb.Append(',');
            sb.Append(item.Path);
            sb.Append(',');
            sb.Append(item.District);
            sb.Append('\n');
        }
        return sb.ToString();
    }
}

public class SVGData
{
    public SVGData(string path, string name = "", string? district = null)
    {
        Path = path;
        Name = name;
        District = district;
    }

    public string Path { get; }

    public string Name { get; set; } = "";

    public bool Selected { get; set; } = false;

    public string? District { get; }
}
