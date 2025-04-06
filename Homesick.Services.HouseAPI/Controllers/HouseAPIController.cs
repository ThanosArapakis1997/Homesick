using AutoMapper;
using Homesick.Services.HouseAPI.Models.DTO;
using Homesick.Services.HouseAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Homesick.Services.HouseAPI.Repository.IRepository;
using Homesick.Services.HouseAPI.Models;

namespace Homesick.Services.HouseAPI.Controllers
{
    [Route("api/house")]
    [ApiController]
    public class HouseAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IHouseRepository _houseRepository;

        public HouseAPIController(AppDbContext db, IHouseRepository houseRepository)
        {
            _response = new ResponseDto();
            _houseRepository = houseRepository;
        }

        [HttpGet]
        public async Task<ResponseDto> GetAllHouses()
        {
            try
            {
                IEnumerable<HouseDto> houses = await _houseRepository.GetAllHouses();
                _response.Result = houses;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id}")] //specific House
        public async Task<object> GetHouse(int? id)
        {
            try
            {
                HouseDto HouseDto = await _houseRepository.GetHouse(id);
                _response.Result = HouseDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public async Task<object> Add([FromBody] HouseDto HouseDto)
        {
            try
            {
                HouseDto model = await _houseRepository.AddHouse(HouseDto);

                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        public async Task<object> Update([FromBody] HouseDto houseDto)
        {
            try
            {
                HouseDto model = await _houseRepository.UpdateHouse(houseDto);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<object> Delete(int id)
        {
            try
            {
                bool isSuccess = await _houseRepository.RemoveHouse(id);
                _response.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
