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

    public StoryService(IFileService fileService,IMapper mapper,DataContext context)
    {
        _fileService = fileService;
        _mapper = mapper;
        _context = context;
    }
   

    public async Task<Response<GetStoryDto>> AddStory(AddStoryDto file)
    {
        try
        {
            var file1 = _mapper.Map<Story>(file);
            var list = new List<string>();
            foreach (var f in file.Images)
            {
                var fileName = _fileService.CreateFile(f);
            list.Add(fileName.Data);
            }

            file1.Images = list;
            await _context.Stories.AddAsync(file1);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<GetStoryDto>(file1);
            return new Response<GetStoryDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetStoryDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}