using System.Web.Mvc;
using UoC_Site.Models;

public static class HtmlExtentions
{
    public static string ConvertGrade(this HtmlHelper<Course> helper, Grade? g)
    {
        switch (g)
        {
            case Grade.Ap:
                return "A+";

            case Grade.A:
                return "A";

            case Grade.Am:
                return "A-";

            case Grade.Bp:
                return "B+";

            case Grade.B:
                return "B";

            case Grade.Bm:
                return "B-";

            case Grade.Cp:
                return "C+";

            case Grade.C:
                return "C";

            case Grade.Cm:
                return "C-";

            case Grade.Dp:
                return "D+";

            case Grade.D:
                return "D";

            case Grade.Dm:
                return "D-";

            case Grade.F:
                return "F";

            default:
                return null;
        }
    }
}