using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaApp.Services.Models;
using Microsoft.AspNetCore.Mvc;
using CoronaApp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;


using CoronaApp.Dal;
using Newtonsoft.Json;

namespace CoronaApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        private readonly CoronaContext _context;

        private readonly IUrlHelper _urlHelper;
        public LocationController(ILocationService locationService,
            CoronaContext coronaContext,
            IUrlHelper urlHelper)
        {
            _locationService = locationService;
            _context = coronaContext;
            _urlHelper = urlHelper;
        }
        //GET: api/<LocationController>
        [AllowAnonymous]
        [HttpGet]
        public async Task< IEnumerable<Location>> Get([FromQuery] LocationSearch locationSearch = null)
        {
            return await _locationService.GetAsync(locationSearch);
        }
        [AllowAnonymous]
        [HttpGet("paging")]
        public async Task<IEnumerable<Location>> GetByPagingAsync([FromQuery] int pageIndex, [FromQuery]int numForPage)
        {
            return await _locationService.GetByPagingAsync(pageIndex,numForPage);
        }
        [HttpPost]
        public async Task<IEnumerable<Location>> Post(LocationSearch locationSearch)
        {
            return await _locationService.GetAsync(locationSearch);
        }
        [AllowAnonymous]
        //[HttpGet(Name = nameof(GetAll))]
        [HttpGet("page")]
        public IActionResult GetAll([FromQuery] QueryParameters queryParameters)
        {
            List<Location> allCustomers = GetAll2(queryParameters)
                .ToList();
            var allItemCount = _context.Locations.Count();
            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };
            Response.Headers
                .Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));
            var links = CreateLinksForCollection(queryParameters, allItemCount);
            //var toReturn = allCustomers.Select(x => ExpandSingleItem(x));
            return Ok(new
            {
                value = allCustomers,
                links = links
            });
        }
        private List<LinkDto> CreateLinksForCollection(QueryParameters queryParameters, int totalCount)
        {
            var links = new List<LinkDto>();

            links.Add(
             new LinkDto(_urlHelper.Link(nameof(Add), null), "create", "POST"));

            // self 
            links.Add(
             new LinkDto(_urlHelper.Link(nameof(GetAll), new
             {
                 pagecount = queryParameters.PageCount,
                 page = queryParameters.Page,
                 orderby = queryParameters.OrderBy
             }), "self", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAll), new
            {
                pagecount = queryParameters.PageCount,
                page = 1,
                orderby = queryParameters.OrderBy
            }), "first", "GET"));

            links.Add(new LinkDto(_urlHelper.Link(nameof(GetAll), new
            {
                pagecount = queryParameters.PageCount,
                page = queryParameters.GetTotalPages(totalCount),
                orderby = queryParameters.OrderBy
            }), "last", "GET"));

            if (queryParameters.HasNext(totalCount))
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAll), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page + 1,
                    orderby = queryParameters.OrderBy
                }), "next", "GET"));
            }

            if (queryParameters.HasPrevious())
            {
                links.Add(new LinkDto(_urlHelper.Link(nameof(GetAll), new
                {
                    pagecount = queryParameters.PageCount,
                    page = queryParameters.Page - 1,
                    orderby = queryParameters.OrderBy
                }), "previous", "GET"));
            }

            return links;
        }
        public void Add(Location item)
        {
            _context.Locations.Add(item);
        }

        public IQueryable<Location> GetAll2(QueryParameters queryParameters)
        {
            IQueryable<Location> _allItems = _context.Locations
                .OrderBy(queryParameters.OrderBy, queryParameters.IsDescending());
            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.id.ToString().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }


    }

}
