using System.Text;

namespace CS.Core.FullVersion.Helpers;

public static class Printers
{
    private const int TitleSignCount = 3;
    private const int SpaceBeforeCount = 3;

    public static void PrintBox(Box2Filler boxFiller, string? name = default)
    {
        Console.WriteLine(GetBoxPrint(boxFiller, name));
    }

    public static string GetBoxPrint(Box2Filler boxFiller, string? name = default)
    {
        var sb = new StringBuilder();
        sb.Append('=', TitleSignCount);
        if (name != null)
        {
            sb.Append(' ');
            sb.Append(name);
            sb.Append(' ');
        }
        sb.Append('=', TitleSignCount);
        sb.AppendLine();

        for (var iy = 0; iy < boxFiller.Box.LY; iy++)
        {
            sb.Append(' ', SpaceBeforeCount);

            for (var ix = 0; ix < boxFiller.Box.LX; ix++)
            {
                var n = boxFiller[ix, iy]?.LX.ToString();
                if (n != null)
                {
                    if (2 - n.Length > 0)
                        sb.Append(' ', 2 - n.Length);
                    sb.Append(n);
                }
                else
                    sb.Append("..");

                sb.Append(' ');
            }

            sb.AppendLine();
        }

        sb.Append('=', TitleSignCount + 1 + (name?.Length ?? -2) + 1 + TitleSignCount);
        sb.AppendLine();

        return sb.ToString();
    }

    public static void PrintBox(Box3Filler boxFiller, string? name = default)
    {
        Console.WriteLine(GetBoxPrint(boxFiller, name));
    }

    public static string GetBoxPrint(Box3Filler boxFiller, string? name = default)
    {
        var sb = new StringBuilder();
        sb.Append('=', TitleSignCount);
        if (name != null)
        {
            sb.Append(' ');
            sb.Append(name);
            sb.Append(' ');
        }
        sb.Append('=', TitleSignCount);
        sb.AppendLine();

        for (var iz = 0; iz < boxFiller.Box.LZ; iz++)
        {
            sb.Append(' ', SpaceBeforeCount);
            sb.Append("- Layer ");
            sb.Append(iz);
            sb.AppendLine(":");
            for (var iy = 0; iy < boxFiller.Box.LY; iy++)
            {
                sb.Append(' ', SpaceBeforeCount);

                for (var ix = 0; ix < boxFiller.Box.LX; ix++)
                {
                    var n = boxFiller[ix, iy, iz]?.LX.ToString();
                    if (n != null)
                    {
                        if (2 - n.Length > 0)
                            sb.Append(' ', 2 - n.Length);
                        sb.Append(n);
                    }
                    else
                        sb.Append("..");

                    sb.Append(' ');
                }

                sb.AppendLine();
            }
        }

        sb.Append('=', TitleSignCount + 1 + (name?.Length ?? -2) + 1 + TitleSignCount);
        sb.AppendLine();

        return sb.ToString();
    }
}