﻿using Microsoft.AspNetCore.Mvc;

namespace OnlineBookStore.Infrastructure
{
    [Route("[controller]/[action]", Name = "[controller]_[action]")]
    public abstract class BaseController : Controller
    {
    }
}
