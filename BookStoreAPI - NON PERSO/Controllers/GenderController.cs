using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreAPI.Entities;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GenderController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IMapper _mapper;

		public GenderController(ApplicationDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<List<GenderDto>>> GetGenders()
		{
			var genders = await _dbContext.Genders.ToListAsync();
			var gendersDto = _mapper.Map<List<GenderDto>>(genders);
			return Ok(gendersDto);
		}

		[HttpPost]
		public async Task<ActionResult<GenderDto>> AddGender([FromBody] GenderDto genderDto)
		{
			if (genderDto == null)
			{
				return BadRequest();
			}

			var gender = _mapper.Map<Gender>(genderDto);

			_dbContext.Genders.Add(gender);
			await _dbContext.SaveChangesAsync();

			return Created("gender", _mapper.Map<GenderDto>(gender));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateGender(int id, [FromBody] GenderDto genderDto)
		{
			if (id != genderDto.Id)
			{
				return BadRequest();
			}

			var gender = await _dbContext.Genders.FindAsync(id);
			if (gender == null)
			{
				return NotFound();
			}

			_mapper.Map(genderDto, gender);

			try
			{
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!GenderExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteGender(int id)
		{
			var gender = await _dbContext.Genders.FindAsync(id);
			if (gender == null)
			{
				return NotFound();
			}

			_dbContext.Genders.Remove(gender);
			await _dbContext.SaveChangesAsync();

			return NoContent();
		}

		private bool GenderExists(int id)
		{
			return _dbContext.Genders.Any(e => e.Id == id);
		}
	}
}
