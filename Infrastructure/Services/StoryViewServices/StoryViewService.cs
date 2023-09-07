using System.Net;
using AutoMapper;
using Domain.Dtos.StoryViewDtos;
using Domain.Entities.Post;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.StoryViewServices;

public class StoryViewService : IStoryViewService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public StoryViewService(DataContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Response<GetStoryViewDto>> AddStoryView(AddStoryViewDto model, string token)
    {
        try
        {
            var story = _context.Stories.FirstOrDefault(e => e.Id == model.StoryId);
            if (story != null)
            {
                var storyView = new StoryView()
                {
                    ViewUserId = token,
                    StoryId = model.StoryId,
                };
                var view = await _context.StoryViews.FirstOrDefaultAsync(e=>e.ViewUserId == story.UserId);
                var stat = _context.StoryStats.FirstOrDefault(e=>e.StoryId == story.Id);
                if (story.UserId == token && view == null )
                {

                    stat.ViewCount++;
                   
                    await _context.StoryViews.AddAsync(storyView);
                    _context.StoryStats.Update(stat);
                    await _context.SaveChangesAsync();
                    var mapped = _mapper.Map<GetStoryViewDto>(storyView);
                    return new Response<GetStoryViewDto>(mapped);
                }
                else
                {
                    var mapped = _mapper.Map<GetStoryViewDto>(storyView);
                    return new Response<GetStoryViewDto>(mapped);
                }
            }
            else
            {
                return new Response<GetStoryViewDto>(HttpStatusCode.BadRequest,"Story not found");
            }
        }
        catch (Exception e)
        {
            return new Response<GetStoryViewDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}