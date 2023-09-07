using System.Net;
using AutoMapper;
using Domain.Dtos.StoryDtos;
using Domain.Dtos.ViewerDtos;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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


    public async Task<Response<GetStoryDto>> GetStoryById(int id,string token,string userName)
    {
        var story = await _context.Stories.FirstOrDefaultAsync(e => e.Id == id);
        if (story != null)
        {
            if (story.UserId == token)
            {
                var viewCount = story.StoryStat.ViewCount;
                var name = (from st in _context.Stories
                    join user in _context.Users on st.UserId equals user.Id
                    join prof in _context.UserProfiles on st.UserId equals prof.UserId
                    select new
                    {
                        Name = prof.FirstName
                    }).ToList();
                var storyView = new ViewerDto()
                {
                UserId = token,
                UserName = userName,
                Name = name[0].Name
                };
                var story2 = new Story()
                {
                    Id = story.Id,
                    FileName = story.FileName,
                    CreateAt = story.CreateAt,
                    UserId = token,
                    ViewCount = viewCount,
                };
                
                story2.ViewerDtos.Add(storyView);
                var mapped = _mapper.Map<GetStoryDto>(story2);
                return new Response<GetStoryDto>(mapped);
            }
            else
            {
                var story2 = new Story()
                {
                    Id = story.Id,
                    FileName = story.FileName,
                    CreateAt = story.CreateAt,
                    UserId = token,
                 
                };
                var mapped = _mapper.Map<GetStoryDto>(story2);
                return new Response<GetStoryDto>(mapped);
            }
        }
        else
        {
            return new Response<GetStoryDto>(HttpStatusCode.BadRequest, "Story not found");
        }
        
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