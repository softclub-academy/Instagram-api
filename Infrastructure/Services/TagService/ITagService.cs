using System.Runtime.CompilerServices;
using Domain.Dtos.TagDto;
using Domain.Filters.TagFilter;
using Domain.Responses;

namespace Infrastructure.Services.TagService;

public interface ITagService
{
    Task<PagedResponse<List<TagDto>>> GetTags(TagFilter filter);
    Task<Response<TagDto>> GetTagById(int id);
    Task<Response<TagDto>> AddTag(TagDto addTag);
    Task<Response<TagDto>> UpdateTag(TagDto addTag);
    Task<Response<bool>> DeleteTag(int id);
}