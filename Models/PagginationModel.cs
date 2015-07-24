using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestHandler.Models
{
    public class PaginationModel
    {
        public int Page { get; set; }
        public int LastPage { get; set; }

        public int NumberOfDisplayedPages { get; set; }

        public int NextPage { get { return IsLast ? Page : Page + 1; } }
        public int PreviousPage { get { return IsFirst ? Page : Page - 1; } }

        public bool IsLast { get { return Page >= LastPage; } }
        public bool IsFirst { get { return Page <= 1; } }
        public bool IsValid { get { return Page > 0 && Page <= LastPage; } }

        public int[] GetShownPages()
        {
            if (IsValid && NumberOfDisplayedPages > 0)
            {
                int L = (NumberOfDisplayedPages - 1) / 2;
                int R = NumberOfDisplayedPages - 1 - L;
                int L_corr = Page - 1 - L;
                int R_corr = LastPage - Page - R;

                if (L_corr < 0)
                {
                    L += L_corr;
                    R -= L_corr;
                    if (Page + R > LastPage)
                    {
                        R = LastPage - Page;
                    }
                }
                else if (R_corr < 0)
                {
                    R += R_corr;
                    L -= R_corr;
                    if (Page - L < 1)
                    {
                        L = Page - 1;
                    }
                }
                int begin = Page - L;
                int end = Page + R;
                var result = new int[end - begin + 1];
                for (int i = 0; i < result.Length; i++, begin++)
                {
                    result[i] = begin;
                }
                return result;
            }
            return null;

        }

        public ActionDestination DefaultDestination { get; set; }
        public object ExtraValues { get; set; }
    }
}