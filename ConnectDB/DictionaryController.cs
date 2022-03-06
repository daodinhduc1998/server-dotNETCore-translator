using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TranslatorWebAPI.Models;
using TranslatorWebAPI.Services;



namespace TranslatorWebAPI.Controllers
{
    public class DictionaryController : ControllerBase
    {
        private readonly DictionaryService _dictService;

        public DictionaryController(DictionaryService dictService)
        {
            _dictService = dictService;
        }

        [HttpGet]
        public ActionResult<List<Dictionary>> Get() =>
            _dictService.Get();

        [HttpGet("api/dictionaries/{name}", Name = "GetDictionaries")]
        public ActionResult<List<Dictionary>> Get(string name)
        {
            var emp = _dictService.Get(name);

            if (emp == null)
            {
                return NotFound();
            }

            return emp;
        }
    }
}
