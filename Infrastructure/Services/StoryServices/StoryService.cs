﻿using System.Net;
using AutoMapper;
using Domain.Dtos.StoryDtos;
using Domain.Dtos.ViewerDtos;
using Domain.Entities;
using Domain.Entities.Post;
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

    public StoryService(IFileService fileService, IMapper mapper, DataContext context,
        IWebHostEnvironment hostEnvironment)
    {
        _fileService = fileService;
        _mapper = mapper;
        _context = context;
        _hostEnvironment = hostEnvironment;
    }


    public async Task<Response<GetStoryDto>> GetStoryById(int id, string userId, string userName)
    {
        try
        {
            var story = await _context.Stories.FirstOrDefaultAsync(e => e.Id == id);
            if (story != null)
            {
                var story2 = await (from st in _context.Stories
                    join pf in _context.UserProfiles on st.UserId equals pf.UserId
                    where st.Id == id
                    select new GetStoryDto()
                    {
                        Id = st.Id,
                        FileName = st.FileName,
                        CreateAt = st.CreateAt,
                        UserId = st.UserId,
                        ViewerDtos = story.UserId == userId
                            ? new ViewerDto()
                            {
                                Name = pf.FirstName, 
                                UserName = userName, 
                                ViewCount = st.StoryStat.ViewCount
                            } : null
                    }).FirstAsync();
                return new Response<GetStoryDto>(story2);
            
            }
            else
            {
                return new Response<GetStoryDto>(HttpStatusCode.BadRequest, "Story not found");
            }
        }
        catch (Exception e)
        {
            return new Response<GetStoryDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetStoryDto>> AddStory(AddStoryDto file, string userId)
    {
        try
        {
            var file1 = new Story()
            {
                UserId = userId
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

    public async Task<Response<bool>> StoryLike(AddLikeDto like,string userId)
    {
        try
        {
            var story = await _context.Stories.FindAsync(like.StoryId);
            if (story == null) return new Response<bool>(HttpStatusCode.BadRequest, "Story not found");
            var user = await _context.StoryLikes.FirstOrDefaultAsync(e=>e.UserId == userId && e.StoryId == like.StoryId);
            var stat = await _context.StoryStats.FindAsync(story.Id);
            if (user == null) 
            {
                stat.ViewLike++;
                var storyLike = new StoryLike()
                {
                    StoryId = like.StoryId,
                    UserId = userId,
                };
               await _context.StoryLikes.AddAsync(storyLike);
               await _context.SaveChangesAsync();
               return new Response<bool>(true);
            }
            else
            {
                stat.ViewLike--;
                _context.StoryLikes.Remove(user);  
                await _context.SaveChangesAsync();
                return new Response<bool>(false);
            }
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
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