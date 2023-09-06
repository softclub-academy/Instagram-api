using System.Net;
using AutoMapper;
using Domain.Dtos.StoryDtos;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.StoryServices;

public class StoryService : IStoryService
{
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public StoryService(IFileService fileService,IMapper mapper,DataContext context,IWebHostEnvironment hostEnvironment)
    {
        _fileService = fileService;
        _mapper = mapper;
        _context = context;
        _hostEnvironment = hostEnvironment;
    }
   

    public async Task<Response<GetStoryDto>> AddStory(AddStoryDto file,string token)
    {
        try
        {
            var file1 = new Story()
            {
            UserId = token
            };
                var fileName = _fileService.CreateFile(file.Image).Data;
                file1.FileName = fileName;
                await _context.Stories.AddAsync(file1);
            await _context.SaveChangesAsync();
            var stat = new StoryStat()
            {
                StoryId = file1.Id,
            };
            await _context.StoryStats.AddAsync(stat);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<GetStoryDto>(file1);
            return new Response<GetStoryDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetStoryDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteStory(int id)
    {
        var story = _context.Stories.FirstOrDefault(e => e.Id == id);
        if (story != null)
        {
            var path = Path.Combine(_hostEnvironment.WebRootPath, "images", story.FileName);
            File.Decrypt(path);
            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        else
        {
            return new Response<bool>(false);
        }
        
    }
}