using Microsoft.AspNetCore.Mvc;
using MovieStore.Models.Domain;
using MovieStore.Repositories.Abstract;

namespace MovieStore.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Add(Genre model)
        {
            if (!ModelState.IsValid) 
            { 
                return View(model);
            }
            var result = _genreService.Add(model);
            if (result)
            {
                TempData["msg"] = "Genre Added Successfully";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }


        public IActionResult Edit(int id)
        {
            var data = _genreService.GetById(id);
            return View(data);
        }


        [HttpPost]
        public IActionResult Edit(Genre model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = _genreService.Update(model);
            if (result)
            {
                TempData["msg"] = "Genre Updated Successfully";
                return RedirectToAction(nameof(GenreList));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }


        public IActionResult GenreList()
        {
            var result = _genreService.List().ToList();
            return View(result);
        }


        public IActionResult Delete(int id)
        {
            var result = _genreService.Delete(id);
            TempData["msg"] = "Genre Deleted Successfully";
            return RedirectToAction(nameof(GenreList));
        }
    }
}
