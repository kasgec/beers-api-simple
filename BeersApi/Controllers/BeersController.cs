﻿using BeersApi.DTOs;
using BeersApi.Models;
using BeersApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeersController : ControllerBase
    {
        private readonly IBeersService _beersService;

        public BeersController(IBeersService beersService)
        {
            _beersService = beersService;
        }

        // GET: api/Beers
        [HttpGet]
        public async Task<IActionResult> GetAllBeers()
        {
            IEnumerable<Beer> beers = await _beersService.GetAll();

            if (beers != null)
            {
                var data = beers.Select(item => new BeerDto
                {
                    BeerId = item.Id,
                    Title = item.Title,
                    Volume = item.Volume,
                    NonAlcohol = item.NonAlcohol,
                    Quantity = item.Quantity
                });

                return new OkObjectResult(data);

            }

            return NoContent();
        }

        // GET: api/Beers/1
        [HttpGet("{beerId}")]
        public async Task<IActionResult> GetBeerById(string beerId)
        {
            if (Guid.TryParse(beerId, out Guid id))
            {
                Beer beer = await _beersService.Get(id);

                if (beer != null)
                {
                    var data = new BeerDto
                    {
                        BeerId = beer.Id,
                        Title = beer.Title,
                        Volume = beer.Volume,
                        NonAlcohol = beer.NonAlcohol,
                        Quantity = beer.Quantity
                    };

                    return new OkObjectResult(data);

                }
            }

            return NoContent();
        }

        // POST: api/Beers
        [HttpPost]
        public IActionResult Post([FromBody] CreateBeerDto add)
        {
            if (add != null)
            {
                var beer = new Beer
                {
                    Id = Guid.NewGuid(),
                    Title = add.Title,
                    NonAlcohol = add.NonAlcohol,
                    Volume = add.Volume,
                    Quantity = add.Quantity
                };

                _beersService.Add(beer);
                return new OkObjectResult(true);
            }

            return new OkObjectResult(false);
        }

        // PUT: api/Beers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateBeerDto update)
        {
            if (Guid.TryParse(id, out Guid consumeId))
            {
                var beer = await _beersService.Get(consumeId);

                if (beer != null)
                {
                    beer.Title = update.Title;
                    beer.NonAlcohol = update.NonAlcohol;
                    beer.Volume = update.Volume;
                    beer.Quantity = update.Quantity;

                    bool increased = await _beersService.Update(beer);

                    return new OkObjectResult(increased);
                }
                else
                {
                    beer = new Beer();

                    beer.Id = Guid.NewGuid();
                    beer.Title = update.Title;
                    beer.NonAlcohol = update.NonAlcohol;
                    beer.Volume = update.Volume;
                    beer.Quantity = update.Quantity;

                    bool increased = await _beersService.Update(beer);

                    return new OkObjectResult(increased);
                }
            }

            return new OkObjectResult(false);
        }

        // PUT: api/ConsumedBeers/5
        [HttpPut("{beerId}/increase")]
        public async Task<IActionResult> Put(string beerId)
        {
            if (Guid.TryParse(beerId, out Guid id))
            {
                var beer = await _beersService.Get(id);

                if (beer != null)
                {
                    beer.Quantity = beer.Quantity + 1;
                    bool increased = await _beersService.Update(beer);

                    return new OkObjectResult(increased);
                }
            }

            return new OkObjectResult(false);
        }

        // DELETE: api/Beers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (Guid.TryParse(id, out Guid beerId))
            {
                var removed = await _beersService.Delete(beerId);
                return new OkObjectResult(removed);
            }

            return new OkObjectResult(false);
        }
    }
}