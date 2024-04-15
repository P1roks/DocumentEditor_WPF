using DocumentEditor_WPF.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumExtensions
{
    public static class FilterOptionsExtensions
    {
        public static string toString(this FilterOptions filter)
        {
            switch (filter) {
                case FilterOptions.All:
                    return "wszystkie";
                case FilterOptions.Xps:
                    return "xps";
                case FilterOptions.Xaml:
                    return "xaml";
                case FilterOptions.Pdf:
                    return "pdf";
                case FilterOptions.Txt:
                    return "txt";
                default:
                    return null;
            }
        }
    }

    public static class SortOptionsExtensions{
        public static string toString(this SortOptions sort)
        {
            switch(sort){
                case SortOptions.NameDesc:
                    return "Nazwy (malejąco)";
                case SortOptions.NameAsc:
                    return "Nazwy (rosnąco)";
                case SortOptions.ExtensionDesc:
                    return "Rozszerzenia (malejąco)";
                case SortOptions.ExtensionAsc:
                    return "Rozszerzenia (rosnąco)";
                default:
                    return null;
            }
        }
    }
}
