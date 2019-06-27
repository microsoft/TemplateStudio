using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.WebApi.Models;

namespace Param_RootNamespace.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItemController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Item>> List()
        {
            return _itemRepository.GetAll().ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Item> GetItem(string id)
        {
            Item item = _itemRepository.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Item> Create([FromBody] Item item)
        {
            _itemRepository.Add(item);
            return CreatedAtAction(nameof(GetItem), new { item.Id }, item);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Edit([FromBody] Item item)
        {
            try
            {
                _itemRepository.Update(item);
            }
            catch (Exception)
            {
                return BadRequest("Error while creating");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(string id)
        {
            Item item = _itemRepository.Remove(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
