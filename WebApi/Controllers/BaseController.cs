<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Authorization;
=======
using Microsoft.AspNetCore.Authorization;
>>>>>>> main
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
[Authorize]
public class BaseController :ControllerBase
{
    [NonAction]
    protected List<string> ModelStateErrors() => ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        
  
}