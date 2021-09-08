using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Helpers
{
    public static class SlugExtension
    {
        public static string EncodeSlug(this string title)
        {
            return title.Replace('-', '_').Replace(' ', '-').Replace(":", "__");
        }
        public static string DecodeSlug(this string slug)
        {
            return slug.Replace("__", ":").Replace('-', ' ').Replace('_', '-');
        }
    }
}
