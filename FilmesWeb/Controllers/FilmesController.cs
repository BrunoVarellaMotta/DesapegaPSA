﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades.Model;
using Entidades.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Negocio;

namespace FilmesWeb.Controllers
{

    [Authorize]
    public class FilmesController : Controller
    {
        public readonly AdmFacade _negocio;

        public readonly UserManager<ApplicationUser> _userManager;

        private IWebHostEnvironment _environment;

        public FilmesController(AdmFacade negocio, 
                                UserManager<ApplicationUser> userManager, 
                                IWebHostEnvironment environment)
        {
            _negocio = negocio;
            _userManager = userManager;
            _environment = environment;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            List<Movie> filmes = _negocio.TodosFilmes();
            return View(filmes);
        }

        [AllowAnonymous]
        public IActionResult roteiroAutenticacao()
        { 
            return View();
        }

        
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            Movie movie = _negocio.getMovie(id);
            if (movie == null)
                return    RedirectToAction("Index", "Filmes");

            else
                return View(movie);

        }

        public async Task<IActionResult> AddReview(int MovieId, string revString)
        {
            var usuario = await _userManager.GetUserAsync(User);

            ViewBag.Id = usuario.Id;
            ViewBag.UserName = usuario.UserName;

            Review novo = new Review()
            {
                Coment = revString,
                User = usuario.UserName,
                MovieId = MovieId
            };

            _negocio.addReview(novo);

            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Details", "Filmes", new { Id=MovieId });

        }



        public IActionResult relFilmes()
        {
            List<RelFilmes> consolidado = _negocio.relatorioFilmes();

            return View(consolidado);

        }

        public async Task<IActionResult> dadosUsuario()
        {
            var usuario = await _userManager.GetUserAsync(User);

            ViewBag.Id = usuario.Id;
            ViewBag.UserName = usuario.UserName;

            return View();
            
        }
    }
}