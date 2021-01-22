using Delights.Modules.Bookkeeping.GraphQL;
using System;

namespace Delights.Modules.Bookkeeping
{
    public class UrlGenerator
    {
        public string Bookkeeping() => "/bookkeeping";

        public string AccountItemCreate() => "/bookkeeping/create";

        public string AccountItem(IData accountItem) => $"/bookkeeping/{Uri.EscapeDataString(accountItem.Id)}";

        public string AccountItemEdit(IData accountItem) => $"/bookkeeping/{Uri.EscapeDataString(accountItem.Id)}/edit";

        public string Categories() => $"/bookkeeping/categories";

        public string Category(string name) => $"/bookkeeping/categories/{Uri.EscapeDataString(name)}";

        public string Tags() => $"/bookkeeping/tags";

        public string Tag(string name) => $"/bookkeeping/tags/{Uri.EscapeDataString(name)}";
    }
}
