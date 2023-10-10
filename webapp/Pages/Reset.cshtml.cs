using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace webapp.Pages;

[Authorize(Policy = "lecturer")]
public class ResetModel : PageModel {
    private readonly ILogger<ResetModel> _logger;

    public ResetModel(ILogger<ResetModel> logger) {
        _logger = logger;
    }

    
    public void OnGet() {
    }
}
