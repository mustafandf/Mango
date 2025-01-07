using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext context;
        private ResponseDto response;
        private IMapper mapper;
        public CouponAPIController(AppDbContext context,IMapper mapper)
        {
            this.context = context;
            response = new ResponseDto();
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets a list of all available coupons
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Get()
        {
            try
            {
                IEnumerable<Coupon> coupons = context.Coupons.ToList();
                response.Result = mapper.Map<IEnumerable<CouponDto>>(coupons);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public object Get(int id)
        {
            try
            {
                var coupon = context.Coupons.First(x => x.CouponId == id);
                response.Result = mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public object GetByCode(string code)
        {
            try
            {
                var coupon = context.Coupons.First(x => x.CouponCode.ToLower() == code.ToLower());
                response.Result = mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost]
        public object Post([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = mapper.Map<Coupon>(couponDto);
                context.Coupons.Add(coupon);
                context.SaveChanges();
                response.Result = mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPut]
        public object Put([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = mapper.Map<Coupon>(couponDto);
                context.Coupons.Update(coupon);
                context.SaveChanges();
                response.Result = mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpDelete]
        public object Delete(int couponId)
        {
            try
            {
                var coupon = context.Coupons.First(x => x.CouponId == couponId);
                context.Coupons.Remove(coupon);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
