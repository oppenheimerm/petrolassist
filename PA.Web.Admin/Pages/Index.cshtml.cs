using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PA.Web.Admin.TmpData;

namespace PA.Web.Admin.Pages
{
    public class IndexModel : PageModel
    {
        //private readonly IInitRepository _initRepository;

        public IndexModel(/*IInitRepository initRepository*/)
        {
            //_initRepository = initRepository;
        }

        public async Task OnGetAsync()
        {
            //await _initRepository.SeedDB();
        }
    }
}
