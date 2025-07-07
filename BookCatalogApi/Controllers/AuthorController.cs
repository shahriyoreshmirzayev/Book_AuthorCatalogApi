using Application.DTOs.AuthorDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace BookCatalogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IValidator<Author> _validator;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _cache;

    private readonly string _Cache_Key = "MyKey";
    public AuthorController(IBookRepository bookRepository, IAuthorRepository authorRepository, IValidator<Author> validator, IMapper mapper, IMemoryCache memoryCache, IDistributedCache cache)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _validator = validator;
        _mapper = mapper;
        _memoryCache = memoryCache;
        _cache = cache;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAuthorById([FromQuery] int id)
    {

        if (_memoryCache.TryGetValue(id.ToString(), out AuthorGetDTO CachedAuthor))
        {
            return Ok(CachedAuthor);
        }
        Author author = await _authorRepository.GetByIdAsync(id);
        if (author == null)
        {
            return NotFound($"Author Id:{id} not found!");
        }
        AuthorGetDTO authorGet = _mapper.Map<AuthorGetDTO>(author);
        _memoryCache.Set(id.ToString(), authorGet);
        return Ok(authorGet);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllAuthors()
    {
        string? CachedAuthors = await _cache.GetStringAsync(_Cache_Key);

        if (string.IsNullOrEmpty(CachedAuthors))
        {
            Task<IQueryable<Author>> Authors = _authorRepository.GetAsync(x => true);
            IEnumerable<AuthorGetDTO> resAuthors = _mapper.Map<IEnumerable<AuthorGetDTO>>(Authors.Result.AsEnumerable());
            await _cache.SetStringAsync(_Cache_Key, JsonSerializer.Serialize(resAuthors), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15)
            });
            return Ok(resAuthors);
        }

        Console.WriteLine("GetStringAsync return json");
        var res = JsonSerializer.Deserialize<IEnumerable<AuthorGetDTO>>(CachedAuthors);
        return Ok(res);


    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreateDTO createDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Author author = _mapper.Map<Author>(createDTO);
        var validresult = _validator.Validate(author);

        if (!validresult.IsValid)
            return BadRequest(validresult);
        author = await _authorRepository.AddAsync(author);
        if (author == null) return NotFound();
        AuthorGetDTO authorGet = _mapper.Map<AuthorGetDTO>(author);
        _memoryCache.Remove(_Cache_Key);
        return Ok(authorGet);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateAuthor([FromBody] AuthorUpdateDTO createDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Author author = _mapper.Map<Author>(createDTO);
        var validationRes = _validator.Validate(author);

        if (!validationRes.IsValid)
            return BadRequest(validationRes);
        author = await _authorRepository.UpdateAsync(author);
        if (author == null) return NotFound();
        AuthorGetDTO authorGet = _mapper.Map<AuthorGetDTO>(author);
        _memoryCache.Remove(authorGet.Id);
        _memoryCache.Remove(_Cache_Key);
        return Ok(authorGet);
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteAuthor([FromQuery] int id)
    {
        bool isDelete = await _authorRepository.DeleteAsync(id);
        _memoryCache.Remove(id);
        _memoryCache.Remove(_Cache_Key);
        return isDelete ? Ok("Deleted successfully")
            : BadRequest("Delete operation failed");
    }
}
