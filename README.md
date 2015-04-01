# LinqToSitefinity
Custom Linq provider for Telerik Sitefinity REST services

## Example Usage

```C#
using (SitefinityContext sitefinityContext = new SitefinityContext("admin", "password", "http://website.com/"))
{
    string searchTerm = "quantum";
    DateTime dateValue = DateTime.UtcNow;

    var pages = sitefinityContext.Pages.Where(p => p.Title.ToUpper().Contains(searchTerm.ToUpper()) && p.DateCreated < dateValue);

    foreach (PageViewModel page in pages)
    {
        Console.WriteLine("[" + page.UrlName + "] (" + page.PageLiveUrl + ")");
    }
}
```
